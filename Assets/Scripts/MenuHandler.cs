using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class MenuHandler : MonoBehaviour
{
    #region Audio
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

    public void GetSlider(Slider slider) //dynamic function to grab slider info and adjust settings
    {
        temp.slider = slider;
        temp.volume = slider.value;
        if (!audioGroup.ContainsKey(currentSlider))
        {
            audioGroup.Add(currentSlider, temp);
        }
    }

    public void Toggle(bool isMuted) //adjusts mute function and toggle interaction
    {
        if (isMuted)
        {
            audioGroup[currentSlider] = temp;
            masterAudio.SetFloat(currentSlider, -80);
            audioGroup[currentSlider].slider.interactable = false;
            PlayerPrefs.SetInt(currentSlider + "mute", 1);
        }
        else
        {
            masterAudio.SetFloat(currentSlider, audioGroup[currentSlider].volume);
            audioGroup[currentSlider].slider.interactable = true;
            PlayerPrefs.SetInt(currentSlider + "mute", 0);
        }
    }

    public void CurrentSlider(string sliderName) //adjusts currentSlider to match passed in stringname
    {
        currentSlider = sliderName;
    }

    public void ChangeVolume(float volume) //adjusts the volume of the current slider to the passed in float value
    {
        masterAudio.SetFloat(currentSlider, volume);
        PlayerPrefs.SetFloat(currentSlider, volume);
    }

    [SerializeField] Slider sliderMasterVolume;
    [SerializeField] Slider sliderMusicVolume;
    [SerializeField] Slider sliderSFXVolume;
    [SerializeField] Toggle muteMaster;

    private void LoadAudioSettings() //loads audio data and adjusts sliders and toggles to match
    {
        float savedSetting;
        savedSetting = PlayerPrefs.GetFloat("masterVolume", 80f);
        masterAudio.SetFloat("masterVolume", savedSetting);
        sliderMasterVolume.value = savedSetting;
        bool muted = PlayerPrefs.GetInt(currentSlider + "mute", 0) == 0 ? false : true;
        muteMaster.isOn = muted;

        savedSetting = PlayerPrefs.GetFloat("musicVolume", 80f);
        masterAudio.SetFloat("musicVolume", savedSetting);
        sliderMusicVolume.value = savedSetting;

        savedSetting = PlayerPrefs.GetFloat("sfxVolume", 80f);
        masterAudio.SetFloat("sfxVolume", savedSetting);
        sliderSFXVolume.value = savedSetting;
    }
    #endregion
    #region Quality and Resolution
    public Resolution[] resolutions;
    public Dropdown resDropDown;
    [SerializeField] Dropdown qualityDropdown;
    [SerializeField] Toggle fullscreenToggle;
    public static int tempQuality, tempResolution;
    public static bool tempFullscreen;

    public void Quality(int qualityIndex) //adjusts quality to match the passed in quality index
    {
        QualitySettings.SetQualityLevel(qualityIndex);
        tempQuality = qualityIndex;
        PlayerPrefs.SetInt("quality", tempQuality);
    }

    public void FullScreenToggle(bool isFullscreen) //toggles fullscreen mode and saves the data
    {
        Screen.fullScreen = isFullscreen;
        tempFullscreen = isFullscreen;
        int value = isFullscreen == false ? 0 : 1;
        PlayerPrefs.SetInt("fullscreen", value);
        tempFullscreen = PlayerPrefs.GetInt("fullscreen") == 0 ? false : true;
    }

    void LoadResolution() // loads resolution related data
    {
        FullScreenToggle(tempFullscreen);
        resDropDown.value = PlayerPrefs.GetInt("resolution");
        resDropDown.RefreshShownValue();
        Quality(PlayerPrefs.GetInt("quality", 0));
    }

    public void SetResolution(int resolutionIndex) //sets the resolution from the passed in index
    {
        Resolution res = resolutions[resolutionIndex];
        Screen.SetResolution(res.width, res.height, Screen.fullScreen);
        PlayerPrefs.SetInt("resolution", resolutionIndex);
    }

    void CreateResolutionValues() //creates the information for the resolution drop down
    {
        resolutions = Screen.resolutions;
        resDropDown.ClearOptions();
        List<string> options = new List<string>();
        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);
            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
            resDropDown.AddOptions(options);
            resDropDown.value = currentResolutionIndex;
            resDropDown.RefreshShownValue();
        }

        qualityDropdown.value = tempQuality;
        fullscreenToggle.isOn = tempFullscreen;
    }
    #endregion
    #region Buttons and Load Info
    public static int fileToLoad;
    public static bool shouldLoad = false;

    public void ContinueGame() // sets shouldLoad to true for loading into a file
    {
        shouldLoad = true;
    }
    public void LoadFileAcrossScene(int index) //sets should load to true for loading into a file and then adjusts filetoLoad value to be the passed in index
    {
        shouldLoad = true;
        fileToLoad = index;
    }
    public void ChangeScene(int sceneNumber) //loads a scene with the passed in index
    {
        SceneManager.LoadScene(sceneNumber);
    }
    public void Quit() //quits the application 
    {
#if UNITY_EDITOR //stops playmode in editor
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();

    }
    #endregion

    public void Start()
    {
        LoadAudioSettings();
        LoadResolution();

        if (shouldLoad)
        {
            GetComponent<SaveHandler>().LoadGame(fileToLoad);
            LoadResolution();
            shouldLoad = false;
        }
    }
}
