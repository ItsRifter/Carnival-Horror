using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool isPaused = false;
    public GameObject pauseMenu;

    GameObject lastMenu;

	void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPaused)
                Pause();
            else
                Resume();
        }
    }

    void Pause()
    {
        isPaused = true;
        pauseMenu.SetActive(true);
        Time.timeScale = 0.0f;
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void Resume()
    {
        isPaused = false;
        pauseMenu.SetActive(false);
        Time.timeScale = 1.0f;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void OpenMenu(GameObject toMenu)
    {
        pauseMenu.SetActive(false);
        toMenu.SetActive(true);

        lastMenu = toMenu;
	}

    public void Back(GameObject toMenu)
    {
		lastMenu.SetActive(false);
		toMenu.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quitting game");
    }

    public void ReturnToMenu()
    {
        isPaused = false;
        //TODO: Create a Main Menu
        //SceneManager.LoadScene("Menu");
    }
}
