using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class WorldMapManager : MonoBehaviour
{
    public Texture2D filledInBackground;
    public Texture2D emptyBackground;
    public Texture2D oneCrown;
    public Texture2D twoCrowns;
    public Texture2D threeCrowns;
    
    public float levelCircleRadius;
    public Vector2 midpoint;
    
    private UIDocument document;
    private int levelCount;

    private PlayerPrefsManager playerPrefsManager;
    
    // Start is called before the first frame update
    void OnEnable()
    {
        levelCount = SceneManager.sceneCountInBuildSettings - 2;
        document = GetComponent<UIDocument>();
        playerPrefsManager = FindObjectOfType<PlayerPrefsManager>();
        if (document == null || playerPrefsManager == null)
        {
            Debug.LogError("NOO");
            return;
        }

        var rootVisualElement = document.rootVisualElement;
        var holder =  rootVisualElement.Q<VisualElement>("LevelHolder");
        
        float angles = 2f * Mathf.PI / levelCount;
        
        
        var template = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/UI Toolkit/CompletedLevel.uxml");

        for (var i = 0; i < levelCount; i++)
        {
            var x = midpoint.x + levelCircleRadius * Mathf.Cos(i * angles - 1.5708f);
            var y = midpoint.y + levelCircleRadius * Mathf.Sin(i * angles - 1.5708f);

            var element = template.Instantiate();
            element.style.position = new StyleEnum<Position>(Position.Absolute);
            element.style.top = y;
            element.style.left = x;
            var crownVE = element.Q<VisualElement>("Crowns");
            var numberVE = element.Q<Label>("Number");
            
            int levelToCheck = i + 1; // build index 0 is title, so we add + 1

            numberVE.text = levelToCheck.ToString();

            if (!playerPrefsManager.IsLevelCompleted(levelToCheck))
            {
                element.Q<VisualElement>("Icon").style.backgroundImage = emptyBackground;
                crownVE.parent.Remove(crownVE);
                numberVE.style.color = Color.white;
                if (levelToCheck == 1 || playerPrefsManager.IsLevelCompleted(levelToCheck - 1))
                {
                    element.RegisterCallback<ClickEvent>(evt => LoadLevel(levelToCheck));
                }
            }
            else
            {
                var score = playerPrefsManager.GetLevelScore(levelToCheck);
                Debug.Log(score);
                switch (score)
                {
                    case LifeCycleManager.Score.HIGH:
                        crownVE.style.backgroundImage = threeCrowns;
                        break;
                    case LifeCycleManager.Score.MEDIUM:
                        crownVE.style.backgroundImage = twoCrowns;
                        break;
                    default:
                        crownVE.style.backgroundImage = oneCrown;
                        break;
                }
                element.RegisterCallback<ClickEvent>(evt => LoadLevel(levelToCheck));
            }
            
            holder.Add(element);
        }
    }

    public void LoadLevel(int buildIndexNumber)
    {
        SceneManager.LoadScene(buildIndexNumber);
    }
}
