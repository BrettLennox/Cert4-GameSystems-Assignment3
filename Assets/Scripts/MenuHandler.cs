using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class MenuHandler : MonoBehaviour
{
    public AudioMixer masterAudio;
    public string currentSlider;

    [System.Serializable]
    public struct AudioGroup
    {
        public Slider slider;
        public float volume;
        public bool isMute;
    }

    public Dictionary<string, AudioGroup> audioGroup = new Dictionary<string, AudioGroup>();
    public AudioGroup temp;

    public void GetSlider(Slider slider)
    {
        temp.slider = slider;
        temp.volume = slider.value;
        if (!audioGroup.ContainsKey(currentSlider))
        {
            audioGroup.Add(currentSlider, temp);
        }
    }

    public void Toggle(bool isMuted)
    {
        if (isMuted)
        {
            audioGroup[currentSlider] = temp;
            masterAudio.SetFloat(currentSlider, -80);
            audioGroup[currentSlider].slider.interactable = false;
        }
        else
        {
            masterAudio.SetFloat(currentSlider, audioGroup[currentSlider].volume);
            audioGroup[currentSlider].slider.interactable = true;
        }
    }

    public void CurrentSlider(string sliderName)
    {
        currentSlider = sliderName;
    }

    public void ChangeVolume(float volume)
    {
        masterAudio.SetFloat(currentSlider, volume);
    }

    public void ChangeScene(int sceneNumber)
    {
        SceneManager.LoadScene(sceneNumber);
    }

    public void Quality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void FullScreenToggle(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    public Resolution[] resolutions;
    public Dropdown resDropDown;

    public void Start()
    {
        resolutions = Screen.resolutions;
        resDropDown.ClearOptions();
        List<string> options = new List<string>();
        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);
            if(resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
            resDropDown.AddOptions(options);
            resDropDown.value = currentResolutionIndex;
            resDropDown.RefreshShownValue();
        }
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution res = resolutions[resolutionIndex];
        Screen.SetResolution(res.width, res.height, Screen.fullScreen);
    }

    public void Quit()
    {
        Application.Quit();
    }
}