using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIController : MonoBehaviour
{
    private static readonly Color DEFAULT_TINT = new Color(1f, 1f, 1f, 1f);
    private static readonly Color GREYEDOUT_TINT = new Color(0.2941177f, 0.2666667f, 0.2666667f, 1f);

    private static readonly ManipulatorActivationFilter DEFAULT_FILTER = new ManipulatorActivationFilter{ button = MouseButton.LeftMouse };

    private Button undoButton;
    private Button restartButton;
    private Button redoButton;

    private Button menuButton;

    private LifeCycleManager lifeCycleManager;
    void Start()
    {
        var rootVisualElement = GetComponent<UIDocument>().rootVisualElement;

        undoButton = rootVisualElement.Q<Button>("UndoButton");
        restartButton = rootVisualElement.Q<Button>("RestartButton");
        redoButton = rootVisualElement.Q<Button>("RedoButton");
        menuButton = rootVisualElement.Q<Button>("MenuButton");

        lifeCycleManager = FindObjectOfType<LifeCycleManager>();
        if (lifeCycleManager != null)
        {
            undoButton.RegisterCallback<ClickEvent>(ev => lifeCycleManager.Undo());
            restartButton.RegisterCallback<ClickEvent>(ev => lifeCycleManager.RestartLevel());
            redoButton.RegisterCallback<ClickEvent>(ev => lifeCycleManager.Redo());
            menuButton.RegisterCallback<ClickEvent>(ev => ShowMenu());
        }
    }

    void ShowMenu()
    {
        
    }
    
    void Update()
    {
        UpdateButton(redoButton, lifeCycleManager.CanRedo());
        UpdateButton(undoButton, lifeCycleManager.CanUndo());
    }

    private void UpdateButton(Button button, bool active)
    {
        if (active)
        {
            if (button.clickable.activators.Count == 0)
            {
                button.clickable.activators.Add(DEFAULT_FILTER);
                button.style.unityBackgroundImageTintColor = DEFAULT_TINT;
            }       
        }
        else
        {
            button.clickable.activators.Clear();
            button.style.unityBackgroundImageTintColor = GREYEDOUT_TINT;
        }
    }
}
