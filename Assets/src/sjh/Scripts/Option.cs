using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Option : MonoBehaviour
{
    FullScreenMode screenMode;
    public TMP_Dropdown resolutionDropdown;
    public Toggle fullscreenBtn;
    List<Resolution> resolutions = new List<Resolution>();
    public int resolutionNum;
    private void Start()
    {
        resolutionDropdown = GetComponent<TMP_Dropdown>();
        InitUI();
    }
    void InitUI()
    {
        for(int i=0;i<Screen.resolutions.Length;i++)
        {
            if ((Screen.resolutions[i].width / 16 * 9) != Screen.resolutions[i].height) continue;
            resolutions.Add(Screen.resolutions[i]);
        }
        resolutionDropdown.options.Clear();

        int optionNum = 0;
        for(; optionNum < resolutions.Count;)
        {
            TMP_Dropdown.OptionData option = new TMP_Dropdown.OptionData();
            option.text = resolutions[optionNum].width + "x" + resolutions[optionNum].height + " " + resolutions[optionNum].refreshRate + "hz";
            resolutionDropdown.options.Add(option);

            if (resolutions[optionNum].width == Screen.width && resolutions[optionNum].height == Screen.height)
                resolutionDropdown.value = optionNum;
            optionNum++;
        }
        resolutionDropdown.RefreshShownValue();

        fullscreenBtn.isOn = Screen.fullScreenMode.Equals(FullScreenMode.FullScreenWindow) ? true : false;
    }

    public void DropboxOptionChange(int x)
    {
        resolutionNum = x;
    }

    public void FullScreenBtn(bool isFull)
    {
        screenMode = isFull ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
    }

    public void OkBtnClick()
    {
        Screen.SetResolution(resolutions[resolutionNum].width,
            resolutions[resolutionNum].height,
            screenMode);
    }
}
