using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuOptions : MonoBehaviour
{
	public static bool isPaused = false;
	public GameObject menuToOpen;

	[SerializeField]
	TMPro.TMP_Dropdown resolutionDropdown;

	Resolution[] resolutions;
	GameObject lastMenu;
	

	void Start()
	{
		resolutions = Screen.resolutions;

		resolutionDropdown.ClearOptions();

		List<string> optionList = new List<string>();

		int curResIndex = 0;

		for (int i = 0; i < resolutions.Length; i++)
		{
			string option = resolutions[i].width + " x " + resolutions[i].height;
			optionList.Add(option);

			if (resolutions[i].width == Screen.currentResolution.width &&
				resolutions[i].height == Screen.currentResolution.height)
				curResIndex = i;
		}

		resolutionDropdown.AddOptions(optionList);
		resolutionDropdown.value = curResIndex;
		resolutionDropdown.RefreshShownValue();
	}

	void Update()
	{
		if (SceneManager.GetActiveScene().name != "MainMenu")
		{	
			if (Input.GetKeyDown(KeyCode.Escape))
			{
				if (!isPaused)
					Pause();
				else
					Resume();
			}
		} 
		else
		{
			isPaused = true;
		}

		if (isPaused)
		{
			Cursor.lockState = CursorLockMode.Confined;
		}
		else
		{
			Cursor.lockState = CursorLockMode.Locked;
		}
	}

	void Pause()
	{
		isPaused = true;
		menuToOpen.SetActive(true);
		Time.timeScale = 0.0f;
	}

	public void Resume()
	{
		isPaused = false;
		menuToOpen.SetActive(false);
		Time.timeScale = 1.0f;
	}

	public void OpenMenu(GameObject toMenu)
	{
		menuToOpen.SetActive(false);
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
		if(isPaused)
			isPaused = false;

		SceneManager.LoadScene("MainMenu");
	}

	public void SetResolution(int resIndex)
	{
		Resolution newResolution = resolutions[resIndex];
		Screen.SetResolution(newResolution.width, newResolution.height, Screen.fullScreen);
	}

	public void SetFullScreen(bool isFull)
	{
		Screen.fullScreen = isFull;
	}

	public void StartGame()
	{
		Time.timeScale = 1.0f;
		isPaused = false;
		SceneManager.LoadScene("MazeLevel");
	}
}
