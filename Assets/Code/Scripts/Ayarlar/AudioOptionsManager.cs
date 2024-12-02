using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class AudioOptionsManager : MonoBehaviour
{
    public static float musicVolume { get; private set; }
    public static float sfxVolume { get; private set; }

    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
    //[SerializeField] private TextMeshProUGUI musicSliderText;
    //[SerializeField] private TextMeshProUGUI sfxSliderText;
    private void Start()
    {
        Load();
        UpdateUI();
        AudioManager.Instance.UpdateMixerVolume();
    }

    public void OnMusicSliderValueChange(float value)
    {
        musicVolume = value;
        //musicSliderText.text = ((int)(value * 100)).ToString();
        AudioManager.Instance.UpdateMixerVolume();
        Save();
    }
    public void OnSFXSliderValueChange(float value)
    {
        sfxVolume = value;
        //sfxSliderText.text = ((int)(value * 100)).ToString();
        AudioManager.Instance.UpdateMixerVolume();
        Save();
    }

    private void Load()
    {
        musicVolume = PlayerPrefs.GetFloat("musicVolume"); // Default value 0.75 if not set
        sfxVolume = PlayerPrefs.GetFloat("sfxVolume"); // Default value 0.75 if not set
    }

    private void Save()
    {
        PlayerPrefs.SetFloat("musicVolume", musicVolume);
        PlayerPrefs.SetFloat("sfxVolume", sfxVolume);
        PlayerPrefs.Save();
    }
    private void UpdateUI()
    {
        musicSlider.value = musicVolume;
        sfxSlider.value = sfxVolume;
        //musicSliderText.text = ((int)(musicVolume * 100)).ToString();
        //sfxSliderText.text = ((int)(sfxVolume * 100)).ToString();
    }
}
