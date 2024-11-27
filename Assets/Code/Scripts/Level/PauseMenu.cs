using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    
    [Header("References")]
    [SerializeField] GameObject pauseMenu;

    /*
    [SerializeField] private Sound[] sounds;
  
    private Dictionary<AudioSource, float> originalVolumes = new Dictionary<AudioSource, float>();
    */
    private int suankiSahneIndex;
    [SerializeField] private AudioSource _backgroundMusic;
    //[SerializeField] private AudioSource _helicopterSound;
    void Start()
    {
        // Bulundu�unuz sahnenin index'ini al
        suankiSahneIndex = SceneManager.GetActiveScene().buildIndex;
        Resume();
    }
    
    public void Pause()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        _backgroundMusic.pitch = 0f;
        //_helicopterSound.pitch = 0f;
    }
    public void Resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        _backgroundMusic.pitch = 1f;
        //_helicopterSound.pitch = 1f;
    }
    
    private void Update()
    {
        suankiSahneIndex = SceneManager.GetActiveScene().buildIndex;
    }
    /*
    public void Pause()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;

        // T�m sesleri durdur
        foreach (Sound sound in sounds)
        {
            if (sound.source != null)
            {
                sound.source.pitch = 0f; // Sesi tamamen k�s
            }
        }
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;

        // T�m sesleri eski haline getir
        foreach (Sound sound in sounds)
        {
            sound.source.pitch = 1f; // Sesi tamamen a�
        }
    }
    */
    
    public void Restart()
    {
        StartCoroutine(PlaySoundAndLoadScene(suankiSahneIndex));
    }


    [SerializeField] private AudioSource clickSound; // Ses efekti kayna��

    public void SesliButon_SceneDegistirmek(int scene)
    {
        StartCoroutine(PlaySoundAndLoadScene(scene));
    }
    private IEnumerator PlaySoundAndLoadScene(int scene)
    {
        Time.timeScale = 1f;  // PauseMenu'ye �zel kod sat�r�!
        clickSound.Play();
        yield return new WaitForSeconds(clickSound.clip.length);
        // Yeni sahneyi y�kle
        SceneManager.LoadScene(scene);
    }
}
