using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using static GravityManager;

/// <summary>
/// Manage LifeCycle of each Level.
/// </summary>
public class LifeCycleManager : MonoBehaviour
{
    public enum Score
    {
        LOW,
        MEDIUM,
        HIGH
    }
    
    public int highScoreMaxMoveCount;
    public int midScoreMaxMoveCount;
    
    public UnityEvent OnUndo;
    public UnityEvent OnRedo;
    public UnityEvent OnWin;
    
    
    private List<Directions> pastMoves = new List<Directions>();
    private int lastMoveIndex = -1;

    private GravityManager gravityManager;

    private bool won = false;
    
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
        if (won)
        {
            return;
        }
        
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

    // Load the next scene in the build order
    public void WinLevel()
    {
        won = true;
        var playerprefsmanager = FindObjectOfType<PlayerPrefsManager>();
        if (playerprefsmanager != null)
        {
            playerprefsmanager.SetLevelScore(GetScore());
        }
        
        OnWin.Invoke();
    }

    public void GotoNextLevel()
    {
        var sceneToLoad = SceneManager.GetActiveScene().buildIndex + 1;
        if (SceneManager.sceneCountInBuildSettings == sceneToLoad)
        {
            sceneToLoad = 0;
        }
        SceneManager.LoadScene(sceneToLoad);
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GoToMap()
    {
        SceneManager.LoadScene(SceneManager.sceneCountInBuildSettings -1); // Map should always be the last scene
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

    public int GetMoveCount()
    {
        return lastMoveIndex + 1;
    }

    public Score GetScore()
    {
        var moveCount = GetMoveCount();
        if (moveCount <= highScoreMaxMoveCount)
        {
            return Score.HIGH;
        }
        if (moveCount <= midScoreMaxMoveCount)
        {
            return Score.MEDIUM;
        }

        return Score.LOW;
    }
}
