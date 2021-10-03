using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

using static GravityManager;

/// <summary>
/// Manage LifeCycle of each Level.
/// </summary>
public class LifeCycleManager : MonoBehaviour
{
    public int maxMoves = 0;
    
    public UnityEvent OnUndo;
    public UnityEvent OnRedo;

    private List<Directions> pastMoves = new List<Directions>();
    private int lastMoveIndex = -1;

    private GravityManager gravityManager;

    public void Start()
    {
        gravityManager = FindObjectOfType<GravityManager>();
        if (gravityManager != null)
        {
            gravityManager.OnChangeGravity.AddListener(OnChangeGravity);
        }
    }

    private void Update()
    {
        if (Input.GetButton("Modifier"))
        {
            if (Input.GetButtonDown("ZUNDO") && CanUndo())
            {
                Undo();
            } else if (Input.GetButtonDown("YREDO"))
            {
                Redo();
            }
        }
        else if (Input.GetButtonDown("Restart"))
        {
            RestartLevel();
        }

        if (GetMovesLeft() > 0)
        {
            var horizontal = Input.GetAxis("Horizontal");
            var vertical = Input.GetAxis("Vertical");
            if (horizontal != 0)
            {
                gravityManager.ChangeDirection(horizontal > 0 ? Directions.RIGHT : Directions.LEFT);
            } else if (vertical != 0)
            {
                gravityManager.ChangeDirection(vertical > 0 ? Directions.UP : Directions.DOWN);
            }
        }
    }

    // Load the next scene in the build order
    public void WinLevel()
    {
        Debug.Log("Win");
        // TODO: Show UI, then have ui button trigger new scene load.
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public bool CanUndo()
    {
        return lastMoveIndex >= 0;
    }
    
    public void Undo()
    {
        if (!gravityManager.AllGravityObjectsDoneMoving() || !CanUndo())
        {
            return;
        }
        
        Debug.Log(lastMoveIndex);
        
        OnUndo.Invoke();
        lastMoveIndex--;
    }

    public bool CanRedo()
    {
        return lastMoveIndex < pastMoves.Count - 1;
    }
    
    public void Redo()
    {
        if (!CanRedo() || !gravityManager.AllGravityObjectsDoneMoving())
        {
            return;
        }
        
        OnRedo.Invoke();
        lastMoveIndex++;
    }

    private void OnChangeGravity(Directions direction)
    {
        if (lastMoveIndex < pastMoves.Count - 1)
        {
            pastMoves.RemoveRange(lastMoveIndex + 1, pastMoves.Count - (lastMoveIndex + 1));
        }
        pastMoves.Add(direction);
        lastMoveIndex += 1;
    }

    public int GetMovesLeft()
    {
        return maxMoves - (lastMoveIndex + 1);
    }
}
