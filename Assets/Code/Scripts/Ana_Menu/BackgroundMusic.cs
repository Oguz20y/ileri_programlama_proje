using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class BackgroundMusic : MonoBehaviour
{
    [SerializeField] private AudioMixerGroup musicMixerGroup;
    [SerializeField] private AudioMixerGroup sfxMixerGroup;

    public static BackgroundMusic bgMusic;
    private float musicVolume;
    private float sfxVolume;

    private void Awake()
    {
        if (bgMusic != null)
        {
            Destroy(gameObject);
        }
        else
        {
            bgMusic = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    private void Start()
    {
        Load();
        UpdateMixerVolume();
        Volume_Music();
    }
    private void Update()
    {
        Load();
        UpdateMixerVolume();
        Volume_Music();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }


    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Sahne 2 ise müziði kapat
        if (scene.buildIndex == 2)
        {
            StopMusic();
        }
        else
        {
            StartMusic();
        }

        // Sahne 1 ise müziði durdur
        if (scene.buildIndex == 1)
        {
            PauseMusic();
        }
        else
        {
            unPauseMusic();
        }

        // Sahne 3 ve daha fazla ise müziði kapat
        if (scene.buildIndex >= 3)
        {
            StopMusic();
        }
    }

    public void PauseMusic()
    {
        // Müziði durdur
        if (bgMusic != null && bgMusic.GetComponent<AudioSource>() != null)
        {
            bgMusic.GetComponent<AudioSource>().Pause();
        }
    }
    public void unPauseMusic()
    {
        // Müziði durdur
        if (bgMusic != null && bgMusic.GetComponent<AudioSource>() != null)
        {
            bgMusic.GetComponent<AudioSource>().UnPause();
        }
    }
    public void StopMusic()
    {
        // Müziði durdur
        if (bgMusic != null && bgMusic.GetComponent<AudioSource>() != null)
        {
            bgMusic.GetComponent<AudioSource>().Stop();
        }
    }

    public void StartMusic()
    {
        // Müziði baþlat (müzik durduðunda yeniden baþlatmak için)
        if (bgMusic != null && bgMusic.GetComponent<AudioSource>() != null && !bgMusic.GetComponent<AudioSource>().isPlaying)
        {
            bgMusic.GetComponent<AudioSource>().Play();
        }
    }
    public void Volume_Music()
    {
        if (bgMusic != null && bgMusic.GetComponent<AudioSource>() != null)
        {
            bgMusic.GetComponent<AudioSource>().volume = musicVolume;
            Save();
        }

    }
    public void UpdateMixerVolume()
    {
        musicMixerGroup.audioMixer.SetFloat("Music Volume", Mathf.Log10(musicVolume) * 20);
        sfxMixerGroup.audioMixer.SetFloat("SFX Volume", Mathf.Log10(sfxVolume) * 20);
    }

    private void Save()
    {
        PlayerPrefs.SetFloat("musicVolume", musicVolume);
        PlayerPrefs.SetFloat("sfxVolume", sfxVolume);
        PlayerPrefs.Save();
    }

    private void Load()
    {
        musicVolume = PlayerPrefs.GetFloat("musicVolume"); // Default value 0.75 if not set
        sfxVolume = PlayerPrefs.GetFloat("sfxVolume"); // Default value 0.75 if not set
    }

}
