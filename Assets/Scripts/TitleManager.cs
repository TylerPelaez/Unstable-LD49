using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class TitleManager : MonoBehaviour
{
    private UIDocument document;
    private VisualTreeAsset buttons;
    private VisualTreeAsset credits;

    private VisualElement buttonContainer;
    private VisualElement creditsContainer;
    
    // Start is called before the first frame update
    void OnEnable()
    {
        document = GetComponent<UIDocument>();

        var rve = document.rootVisualElement;

        buttonContainer = rve.Q<VisualElement>("ButtonContainer");
        creditsContainer = rve.Q<VisualElement>("CreditsContainer");
        
        rve.Q<Label>("Play").RegisterCallback<ClickEvent>(evt => SceneManager.LoadScene(SceneManager.sceneCountInBuildSettings -1)); // Load Map
        rve.Q<Label>("Credits").RegisterCallback<ClickEvent>(evt => ToCredits());
        rve.Q<Label>("Quit").RegisterCallback<ClickEvent>(evt => Application.Quit());
        
        rve.Q<Label>("Back").RegisterCallback<ClickEvent>(evt => ToButtons());
        ToButtons();
    }
    
    void Hide(VisualElement container)
    {
        container.style.display = DisplayStyle.None;
    }

    void Show(VisualElement container)
    {
        container.style.display = DisplayStyle.Flex;
    }
    
    void ToCredits()
    {
        Hide(buttonContainer);
        Show(creditsContainer);
    }

    void ToButtons()
    {
        Hide(creditsContainer);
        Show(buttonContainer);
    }
}
