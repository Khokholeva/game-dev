using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System.Collections;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuPanel;
    public GameObject controlPanel;
    public GameObject uiPanel;
    public bool isPaused;
    public bool inControls;
    public bool inIntro;
    public bool inVictory;
    public GameObject[] colorHide;
    public GameObject[] shapeSelect;
    public GameObject colorSelector;
    public GameObject player;
    public PlayerController playerController;
    public GameObject introImage;
    public GameObject VictoryScreen;

    void Start()
    {
        playerController = player.GetComponent<PlayerController>();
        playerController.freezeControls = true;
        inIntro = true;
        if (pauseMenuPanel != null)
        {
            pauseMenuPanel.SetActive(false);
        }
        if (controlPanel != null)
        {
            controlPanel.SetActive(false);
        }
        if (uiPanel != null)
        {
            uiPanel.SetActive(false);
        }
        StartCoroutine(Intro());
    }

    IEnumerator Intro()
    {
        yield return new WaitForSeconds(20);
        if (uiPanel != null)
        {
            uiPanel.SetActive(true);
        }
        playerController.freezeControls = false;
        introImage.SetActive(false);
    }

    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (inVictory)
            {
                playerController.freezeControls = false;
                VictoryScreen.SetActive(false);
                inVictory = false;
            }
            else if (inIntro)
            {
                inIntro = false;
                playerController.freezeControls = false;
                introImage.SetActive(false);
                uiPanel.SetActive(true);
            }
            else if (inControls)
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
        colorSelector.GetComponent<RectTransform>().anchoredPosition = new Vector3(-500 + color * 70, 255, 0);  
    }

    public void ShowVictory()
    {
        playerController.freezeControls = true;
        VictoryScreen.SetActive(true);
        inVictory = true;
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

    public void ChooseShape(int shape)
    {
        foreach (var obj in shapeSelect)
        {
            obj.SetActive(false);
        }
        shapeSelect[shape].SetActive(true);
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