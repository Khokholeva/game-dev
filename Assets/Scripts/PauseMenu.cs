using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuPanel;
    public GameObject controlPanel;
    public bool isPaused;
    public bool inControls;
    public GameObject[] colorHide;
    public GameObject colorSelector;
    public GameObject player;
    public PlayerController playerController;

    void Start()
    {
        playerController = player.GetComponent<PlayerController>();
        if (pauseMenuPanel != null)
        {
            pauseMenuPanel.SetActive(false);
        }
        if (pauseMenuPanel != null)
        {
            controlPanel.SetActive(false);
        }
    }

    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (inControls)
            {
                controlPanel.SetActive(false);
                inControls = false;
            }
            else if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void UnlockColor(int color)
    {
        colorHide[color].SetActive(false);
    }

    public void ChooseColor(int color)
    {
        colorSelector.GetComponent<RectTransform>().anchoredPosition = new Vector3(-500 + color * 70, 220, 0);  
    }

    public void PauseGame()
    {
        if (pauseMenuPanel != null)
        {
            pauseMenuPanel.SetActive(true);
        }

        Time.timeScale = 0f;
        isPaused = true;
        playerController.freezeControls = true;
    }

    public void ResumeGame()
    {
        if (pauseMenuPanel != null)
        {
            pauseMenuPanel.SetActive(false);
        }

        Time.timeScale = 1f;
        isPaused = false;
        playerController.freezeControls = false;
    }

    public void Retry()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        // Reload it cleanly from scratch
        SceneManager.LoadScene(currentSceneIndex);

        Time.timeScale = 1f;
        isPaused = false;
        playerController.freezeControls = false;
    }

    public void Controls()
    {
        controlPanel.SetActive(true);
        inControls = true;
    }

    public void GoToMainMenu()
    {
        //Time.timeScale = 1f;
        //SceneManager.LoadScene("MainMenu");
    }

    public void Quit()
    {
        Application.Quit();
    }
}