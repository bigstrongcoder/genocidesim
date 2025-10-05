using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class TaxCollectorMenu : MonoBehaviour
{
    [Header("Menu Panels")]
    public GameObject mainMenuPanel;
    public GameObject creditsPanel;

    [Header("UI Elements")]
    public Button playButton;
    public Button creditsButton;
    public Button quitButton;
    public Button backButton;

    [Header("Settings")]
    public string gameSceneName = "GameScene";
    public float buttonHoverScale = 1.1f;

    void Start()
    {
        // Show main menu, hide credits
        ShowMainMenu();

        // Setup button listeners
        playButton.onClick.AddListener(StartGame);
        creditsButton.onClick.AddListener(ShowCredits);
        quitButton.onClick.AddListener(QuitGame);
        backButton.onClick.AddListener(ShowMainMenu);

        // Add silly hover effects
        AddHoverEffect(playButton);
        AddHoverEffect(creditsButton);
        AddHoverEffect(quitButton);
        AddHoverEffect(backButton);

        // Set funny text
       
    }

    void StartGame()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    void ShowCredits()
    {
        mainMenuPanel.SetActive(false);
        creditsPanel.SetActive(true);
    }

    void ShowMainMenu()
    {
        mainMenuPanel.SetActive(true);
        creditsPanel.SetActive(false);
    }

    void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }

    void AddHoverEffect(Button btn)
    {
        var et = btn.gameObject.AddComponent<UnityEngine.EventSystems.EventTrigger>();

        // Hover enter
        var enter = new UnityEngine.EventSystems.EventTrigger.Entry();
        enter.eventID = UnityEngine.EventSystems.EventTriggerType.PointerEnter;
        enter.callback.AddListener((data) => {
            btn.transform.localScale = Vector3.one * buttonHoverScale;
        });
        et.triggers.Add(enter);

        // Hover exit
        var exit = new UnityEngine.EventSystems.EventTrigger.Entry();
        exit.eventID = UnityEngine.EventSystems.EventTriggerType.PointerExit;
        exit.callback.AddListener((data) => {
            btn.transform.localScale = Vector3.one;
        });
        et.triggers.Add(exit);
    }
}