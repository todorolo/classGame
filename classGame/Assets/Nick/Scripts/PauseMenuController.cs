using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;

public class PauseMenuController : MonoBehaviour
{
    [Header("Pause Menu UI")]
    public GameObject pauseMenuPanel;

    [Header("Main Menu Reference")]
    public GameObject mainMenuUI;

    [Header("Sensitivity UI")]
    public Slider lookSensitivitySlider;
    public TMP_Text lookSensitivityValueLabel;

    [Header("Input")]
    public InputActionReference pauseAction;

    [Header("Player Reference")]
    public FirstPersonLook playerLook;

    [Header("Look Sensitivity Settings")]
    public float minLookSensitivity = 0.1f;
    public float maxLookSensitivity = 10f;

    [Header("Scene Settings")]
    public string mainMenuSceneName = "MainMenu";

    private bool isPaused = false;

    private void OnEnable()
    {
        if (pauseAction != null)
            pauseAction.action.Enable();
    }

    private void OnDisable()
    {
        if (pauseAction != null)
            pauseAction.action.Disable();
    }

    private void Start()
    {
        if (pauseMenuPanel != null)
            pauseMenuPanel.SetActive(false);

        SetupSensitivitySlider();

        // Keep cursor usable if main menu is open
        if (mainMenuUI != null && mainMenuUI.activeSelf)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            ResumeGame();
        }
    }

    private void Update()
    {
        // Block pause while main menu is active
        if (mainMenuUI != null && mainMenuUI.activeSelf)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            return;
        }

        bool pausePressed = pauseAction != null && pauseAction.action.WasPressedThisFrame();

        if (pausePressed)
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    private void SetupSensitivitySlider()
    {
        if (lookSensitivitySlider == null)
            return;

        lookSensitivitySlider.minValue = minLookSensitivity;
        lookSensitivitySlider.maxValue = maxLookSensitivity;

        if (playerLook != null)
            lookSensitivitySlider.value = playerLook.sensitivityX;

        lookSensitivitySlider.onValueChanged.RemoveListener(SetLookSensitivity);
        lookSensitivitySlider.onValueChanged.AddListener(SetLookSensitivity);

        SetLookSensitivity(lookSensitivitySlider.value);
    }

    public void SetLookSensitivity(float value)
    {
        if (playerLook != null)
        {
            playerLook.sensitivityX = value;
            playerLook.sensitivityY = value;
        }

        if (lookSensitivityValueLabel != null)
            lookSensitivityValueLabel.text = value.ToString("F2");
    }

    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f;

        if (pauseMenuPanel != null)
            pauseMenuPanel.SetActive(true);

        if (playerLook != null)
            playerLook.DisableLook();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;

        if (pauseMenuPanel != null)
            pauseMenuPanel.SetActive(false);

        if (playerLook != null)
            playerLook.EnableLook();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // 🔥 RETURN TO MAIN MENU
    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuSceneName);
    }

    // 🔥 QUIT GAME
    public void QuitGame()
    {
        Debug.Log("Quitting Game...");

        Application.Quit();

        // This helps when testing in Unity Editor
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}