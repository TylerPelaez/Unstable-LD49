using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    public GameObject mainUI;
    public GameObject winScreenUI;
    
    private static readonly Color GREYEDOUT_TINT = new Color(0.2941177f, 0.2666667f, 0.2666667f, 1f);
    private static readonly ManipulatorActivationFilter DEFAULT_FILTER = new ManipulatorActivationFilter{ button = MouseButton.LeftMouse };

    private const float lowScoreFillWidthPct = 28f;
    private const float midScoreFillWidthPct = 60f;
    private const float highScoreFillWidthPct = 100f;
    
    private const float lowScoreMarkerLeftPct = 16f;
    private const float midScoreMarkerLeftPct = 6f;
    private const float highScoreMarkerLeftPct = 4f;

    private Button undoButton;
    private Button restartButton;
    private Button redoButton;
    private Button menuButton;
    private Label gameMoveCountLabel;

    
    // Win Screen items
    private VisualElement scoreFill;
    private VisualElement scoreMarker;

    private VisualElement crown1;
    private VisualElement crown2;
    private VisualElement crown3;

    private Label winScreenMoveCount;

    private Button winScreenRetryButton;
    private Button winScreenNextLevelButton;
    
    private static readonly Color GreyCrownTint = new Color(0.4196078f, 0.4196078f, 0.4196078f, 1f);
    
    private LifeCycleManager lifeCycleManager;
    void Start()
    {
        var rootVisualElement = mainUI.GetComponent<UIDocument>().rootVisualElement;

        undoButton = rootVisualElement.Q<Button>("UndoButton");
        restartButton = rootVisualElement.Q<Button>("RestartButton");
        redoButton = rootVisualElement.Q<Button>("RedoButton");
        menuButton = rootVisualElement.Q<Button>("MenuButton");
        gameMoveCountLabel = rootVisualElement.Q<Label>("MoveCount");
        rootVisualElement.Q<Label>("Level").text = "Level " + SceneManager.GetActiveScene().buildIndex;

        lifeCycleManager = FindObjectOfType<LifeCycleManager>();
        if (lifeCycleManager != null)
        {
            undoButton.RegisterCallback<ClickEvent>(ev => lifeCycleManager.Undo());
            restartButton.RegisterCallback<ClickEvent>(ev => lifeCycleManager.RestartLevel());
            redoButton.RegisterCallback<ClickEvent>(ev => lifeCycleManager.Redo());
            menuButton.RegisterCallback<ClickEvent>(ev => ShowMenu());
            lifeCycleManager.OnWin.AddListener(ShowWinScreen);
        }
    }

    void ShowMenu()
    {
        
    }
    
    void Update()
    {
        UpdateButton(redoButton, lifeCycleManager.CanRedo());
        UpdateButton(undoButton, lifeCycleManager.CanUndo());

        int moveCount = lifeCycleManager.GetMoveCount();
        gameMoveCountLabel.text = moveCount.ToString().PadLeft(3, '0');
    }

    private void UpdateButton(Button button, bool active)
    {
        if (active)
        {
            if (button.clickable.activators.Count == 0)
            {
                button.clickable.activators.Add(DEFAULT_FILTER);
                button.style.unityBackgroundImageTintColor = Color.white;
            }       
        }
        else
        {
            button.clickable.activators.Clear();
            button.style.unityBackgroundImageTintColor = GREYEDOUT_TINT;
        }
    }

    private void ShowWinScreen()
    {
        winScreenUI.SetActive(true);

        var rootVisualElement = winScreenUI.GetComponent<UIDocument>().rootVisualElement;
        
        scoreFill = rootVisualElement.Q<VisualElement>("SliderFill");
        scoreMarker = rootVisualElement.Q<VisualElement>("SliderMarker");

        crown1 = rootVisualElement.Q<VisualElement>("Crown1");
        crown2 = rootVisualElement.Q<VisualElement>("Crown2");
        crown3 = rootVisualElement.Q<VisualElement>("Crown3");

        winScreenMoveCount = rootVisualElement.Q<Label>("MoveCount");
        
        winScreenRetryButton = rootVisualElement.Q<Button>("Retry");
        winScreenNextLevelButton = rootVisualElement.Q<Button>("NextButton");
        var winScreenReturnToMap = rootVisualElement.Q<Button>("Map");
                    
        winScreenRetryButton.RegisterCallback<ClickEvent>(ev => lifeCycleManager.RestartLevel());
        winScreenNextLevelButton.RegisterCallback<ClickEvent>(ev => lifeCycleManager.GotoNextLevel());
        winScreenReturnToMap.RegisterCallback<ClickEvent>(ev => lifeCycleManager.GoToMap());
        
        var finalMoveCount = lifeCycleManager.GetMoveCount();
        var score = lifeCycleManager.GetScore();

        float fillPct;
        float markerLeftPct;
        
        switch (score)
        {
            case LifeCycleManager.Score.HIGH:
                fillPct = highScoreFillWidthPct;
                markerLeftPct = highScoreMarkerLeftPct;
                break;
            
            case LifeCycleManager.Score.MEDIUM:
                fillPct = midScoreFillWidthPct;
                markerLeftPct = midScoreMarkerLeftPct;
                crown3.style.unityBackgroundImageTintColor = GreyCrownTint;
                break;
            
            default:
                fillPct = lowScoreFillWidthPct;
                markerLeftPct = lowScoreMarkerLeftPct;
                crown3.style.unityBackgroundImageTintColor = GreyCrownTint;
                crown2.style.unityBackgroundImageTintColor = GreyCrownTint;
                break;
        }

        scoreFill.style.width = new StyleLength(Length.Percent(fillPct));
        scoreMarker.style.left = new StyleLength(Length.Percent(markerLeftPct));
        winScreenMoveCount.text = finalMoveCount.ToString();
    }
}
