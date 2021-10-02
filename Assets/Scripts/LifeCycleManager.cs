using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

/// <summary>
/// Manage LifeCycle of each Level.
/// </summary>
public class LifeCycleManager : MonoBehaviour
{
    public UnityEvent OnUndo;
    public UnityEvent OnRedo;

    private List<GravityManager.Directions> pastMoves = new List<GravityManager.Directions>();
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
        if (!gravityManager.AllGravityObjectsDoneMoving() && CanUndo())
        {
            return;
        }
        
        OnUndo.Invoke();
        lastMoveIndex--;
    }

    public bool CanRedo()
    {
        return lastMoveIndex < pastMoves.Count - 1;
    }
    
    public void Redo()
    {
        if (!CanRedo() && gravityManager.AllGravityObjectsDoneMoving())
        {
            Debug.LogError("Cant redo? How happen");
            return;
        }
        
        OnRedo.Invoke();
        lastMoveIndex++;
    }

    private void OnChangeGravity(GravityManager.Directions direction)
    {
        if (lastMoveIndex < pastMoves.Count - 1)
        {
            pastMoves.RemoveRange(lastMoveIndex + 1, pastMoves.Count - (lastMoveIndex - 1));
        }
        pastMoves.Add(direction);
        lastMoveIndex += 1;
    }
}
