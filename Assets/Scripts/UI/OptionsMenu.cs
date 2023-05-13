using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.LowLevel;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    [SerializeField]
    TMPro.TMP_Dropdown resolutionDropdown;
    public Slider mouseSensSlider;
    public PlayerLook playerLook;

    Resolution[] resolutions;

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

    public void SetResolution(int resIndex)
    {
        Resolution newResolution = resolutions[resIndex];
        Screen.SetResolution(newResolution.width, newResolution.height, Screen.fullScreen);
    }

    public void SetFullScreen(bool isFull)
    {
        Screen.fullScreen = isFull;
    }

    public void ChangeMouseSensitivity()
    {
        playerLook.mouseSens = mouseSensSlider.value;
    }
}
