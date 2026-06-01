using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuPanel;
    public bool isPaused;


    void Start()
    {
        if (pauseMenuPanel != null)
        {
            pauseMenuPanel.SetActive(false);
        }
    }

    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        if (pauseMenuPanel != null)
        {
            pauseMenuPanel.SetActive(true);
        }
        
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void ResumeGame()
    {
        if (pauseMenuPanel != null)
        {
            pauseMenuPanel.SetActive(false);
        }
        
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void Retry()
    {
        //todo
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