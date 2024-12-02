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

        // Audio Source bile�enini al
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("AudioSource bile�eni bulunamad�.");
            return;
        }
    }

    private void Update()
    {
        Load();
        CheckAndLoadScene(); // Her frame bu metodu �al��t�r
    }

    // IEnumerator ile ses �ald�ktan sonra yeni sahneye ge�mek
    public IEnumerator PlaySoundAndLoadScene(int scene)
    {
        audioSource.Play();
        yield return new WaitForSeconds(audioSource.clip.length);
        // Yeni sahneyi y�kle
        SceneManager.LoadScene(scene);
    }
    public void PlaySound_LoadGameScene(int scene)
    {
        StartCoroutine(PlaySoundAndLoadScene(scene));
    }

    // Ses �almadan yeni sahneye ge�mek
    public void LoadGameScene(int scene)
    {
        // Yeni sahneyi y�kle
        SceneManager.LoadScene(scene);
    }

    // IEnumerator's�z ses �ald�ktan sonra yeni sahneye ge�mek
    public void PlayLoadButton(int scene)
    {
        // Ses �al
        audioSource.Play();
        // Timer'� ba�lat
        isPlaying = true;
        timer = 0f;
        sceneToLoad = scene;
    }
    private void CheckAndLoadScene()
    {
        if (isPlaying)
        {
            // Timer'� g�ncelle
            timer += Time.deltaTime;

            // Sesin uzunlu�u kadar s�re ge�tikten sonra sahneyi y�kle
            if (timer >= audioSource.clip.length)
            {
                isPlaying = false;
                SceneManager.LoadScene(sceneToLoad);
            }
        }
    }

    // IEnumerator ile ses �ald�ktan sonra oyundan tamamen ��kmak
    public IEnumerator PlaySoundAndExit()
    {
        audioSource.Play();
        yield return new WaitForSeconds(audioSource.clip.length);
        // Oyundan ��k
        Application.Quit();
#if UNITY_EDITOR
        // Unity Edit�r i�inde test ediyorsan�z edit�rden ��k��� sim�le etmek i�in ekleyin
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
