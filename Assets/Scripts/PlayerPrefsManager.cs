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
        // TODO: when actually built, do this!
        //DontDestroyOnLoad(gameObject);
    }

    private String GetLevelKey()
    {
        return "high-score-" + SceneManager.GetActiveScene().buildIndex;
    }
    
    public void SetLevelScore(LifeCycleManager.Score score)
    {
        var key = GetLevelKey();

        if ((LifeCycleManager.Score) PlayerPrefs.GetInt(key) < score)
        {
            PlayerPrefs.SetInt(key, (int) score);
            PlayerPrefs.Save();
        }
    }
}
