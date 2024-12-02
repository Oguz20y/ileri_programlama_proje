using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Button : MonoBehaviour
{
    private float musicVolume;
    private float sfxVolume;

    private float timer = 0f;
    private bool isPlaying = false;
    private int sceneToLoad;
    private AudioSource audioSource;

    private int currentSceneIndex;

    private void Start()
    {
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        Load();
        CheckAndLoadScene();

        // Audio Source bileþenini al
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("AudioSource bileþeni bulunamadý.");
            return;
        }
    }

    private void Update()
    {
        Load();
        CheckAndLoadScene(); // Her frame bu metodu çalýþtýr
    }

    // IEnumerator ile ses çaldýktan sonra yeni sahneye geçmek
    public IEnumerator PlaySoundAndLoadScene(int scene)
    {
        audioSource.Play();
        yield return new WaitForSeconds(audioSource.clip.length);
        // Yeni sahneyi yükle
        SceneManager.LoadScene(scene);
    }
    public void PlaySound_LoadGameScene(int scene)
    {
        StartCoroutine(PlaySoundAndLoadScene(scene));
    }

    // Ses çalmadan yeni sahneye geçmek
    public void LoadGameScene(int scene)
    {
        // Yeni sahneyi yükle
        SceneManager.LoadScene(scene);
    }

    // IEnumerator'süz ses çaldýktan sonra yeni sahneye geçmek
    public void PlayLoadButton(int scene)
    {
        // Ses çal
        audioSource.Play();
        // Timer'ý baþlat
        isPlaying = true;
        timer = 0f;
        sceneToLoad = scene;
    }
    private void CheckAndLoadScene()
    {
        if (isPlaying)
        {
            // Timer'ý güncelle
            timer += Time.deltaTime;

            // Sesin uzunluðu kadar süre geçtikten sonra sahneyi yükle
            if (timer >= audioSource.clip.length)
            {
                isPlaying = false;
                SceneManager.LoadScene(sceneToLoad);
            }
        }
    }

    // IEnumerator ile ses çaldýktan sonra oyundan tamamen çýkmak
    public IEnumerator PlaySoundAndExit()
    {
        audioSource.Play();
        yield return new WaitForSeconds(audioSource.clip.length);
        // Oyundan çýk
        Application.Quit();
#if UNITY_EDITOR
        // Unity Editör içinde test ediyorsanýz editörden çýkýþý simüle etmek için ekleyin
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
    public void PlaySound_ExitButton()
    {
        StartCoroutine(PlaySoundAndExit());
    }
    private void Save()
    {
        PlayerPrefs.SetFloat("musicVolume", musicVolume);
        PlayerPrefs.SetFloat("sfxVolume", sfxVolume);
        PlayerPrefs.Save();
    }

    private void Load()
    {
        musicVolume = PlayerPrefs.GetFloat("musicVolume", 0.75f); // Default value 0.75 if not set
        sfxVolume = PlayerPrefs.GetFloat("sfxVolume", 0.75f); // Default value 0.75 if not set
    }
}
