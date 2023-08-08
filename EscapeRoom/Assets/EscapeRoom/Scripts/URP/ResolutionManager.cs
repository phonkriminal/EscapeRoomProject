using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.Rendering;


public class ResolutionManager : MonoBehaviour
{
    [SerializeField]
    Dropdown ddResolution;
    [SerializeField]
    Dropdown resolutionQuality;
    private Resolution[] screenResolutions;
    // Start is called before the first frame update

    private void Awake()
    {
#if UNITY_EDITOR
        screenResolutions = Screen.resolutions.Where(resolution =>
                         (double)resolution.width / (double)resolution.height == 16d / 9d).ToArray();
#else
        screenResolutions = Screen.resolutions.Where(resolution => resolution.refreshRate == Screen.currentResolution.refreshRate &&
                         (double)resolution.width / (double)resolution.height == 16d / 9d).ToArray();
#endif
    }
    void Start()
    {
        LoadUI();    
    }

    // Update is called once per frame
    void Update()
    {
        float resolutionRatioX = 0f;
        float resolutionRatioY = 0f;
        switch (resolutionQuality.value)
        {
            case 0:
                resolutionRatioX = 1f;
                resolutionRatioY = 1f;
                break;
            case 1:
                resolutionRatioX = 0.7f;
                resolutionRatioY = 0.7f;
                break;
            case 2:
                resolutionRatioX = 0.5f;
                resolutionRatioY = 0.5f;
                break;
            case 3:
                resolutionRatioX = 0.3f;
                resolutionRatioY = 0.4f;
                break;
        }
        string _resolution = $"Width: {(int)(PlayerPrefs.GetInt("OriginalX") * resolutionRatioX)}  Height: {(int)(PlayerPrefs.GetInt("OriginalY") * resolutionRatioY)}";
    }

    private void LoadUI()
    {
        List<Dropdown.OptionData> options = new List<Dropdown.OptionData>();

        ddResolution.ClearOptions();

        for (int i = 0; i < screenResolutions.Length; i++)
        {
            string option = screenResolutions[i].width + " x " + screenResolutions[i].height;
            Dropdown.OptionData optionData = new Dropdown.OptionData(option);
            options.Add(optionData);
        }
        ddResolution.AddOptions(options);

        int _width = PlayerPrefs.GetInt("OriginalX", Screen.currentResolution.width);
        int _height = PlayerPrefs.GetInt("OriginalY", Screen.currentResolution.height);
        string _match = $"{_width} x {_height}"; 
        int index = ddResolution.options.FindIndex((i) => { return i.text.Equals(_match); });
        ddResolution.value = index;
    }

    public void SetResolution()
    {
        float resolutionRatioX = 0f;
        float resolutionRatioY = 0f;
        switch (resolutionQuality.value)
        {
            case 0:
                resolutionRatioX = 1f;
                resolutionRatioY = 1f;
                break;
            case 1:
                resolutionRatioX = 0.7f;
                resolutionRatioY = 0.7f;
                break;
            case 2:
                resolutionRatioX = 0.5f;
                resolutionRatioY = 0.5f;
                break;
            case 3:
                resolutionRatioX = 0.3f;
                resolutionRatioY = 0.4f;
                break;
        }
                int resolutionIndex = ddResolution.value;
        Resolution resolution = screenResolutions[resolutionIndex];
                int _width = (int)(resolution.width * resolutionRatioX);
                int _height = (int)(resolution.height * resolutionRatioY);
        bool fullScreen = true;
        if (resolutionRatioX == 1f) fullScreen = true;
        Screen.SetResolution(_width, _height, fullScreen);
        
        PlayerPrefs.SetInt("OriginalX", resolution.width);
        PlayerPrefs.SetInt("OriginalY", resolution.height);
    }
}
