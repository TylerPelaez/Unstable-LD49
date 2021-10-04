using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerPrefsManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    private String GetLevelKey(int levelNumber)
    {
        return "high-score-" + levelNumber;
    }
    
    public void SetLevelScore(LifeCycleManager.Score score)
    {
        Debug.Log("score: " + score.ToString());
        var key = GetLevelKey(SceneManager.GetActiveScene().buildIndex);
        Debug.Log(PlayerPrefs.GetInt(key));

        if ((LifeCycleManager.Score) PlayerPrefs.GetInt(key) <= score)
        {
            PlayerPrefs.SetInt(key, (int) score);
            PlayerPrefs.Save();
        }
    }

    public bool IsLevelCompleted(int levelNumber)
    {
        return PlayerPrefs.HasKey(GetLevelKey(levelNumber));
    }

    public LifeCycleManager.Score GetLevelScore(int levelNumber)
    {
        if (!IsLevelCompleted(levelNumber))
        {
            return LifeCycleManager.Score.LOW;
        }
        
        var score = PlayerPrefs.GetInt(GetLevelKey(levelNumber));
        return (LifeCycleManager.Score) score;
    }
}
