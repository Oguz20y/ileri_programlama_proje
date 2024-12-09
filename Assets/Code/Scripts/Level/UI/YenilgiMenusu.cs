using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class YenilgiMenusu : MonoBehaviour
{
    private int suankiSahneIndex;

    [SerializeField] private AudioSource bolumSonuTusSesi;
    [SerializeField] private AudioSource bolumSonuYildizSesi;

    private Animator animator;

    void Start()
    {
        // Animator bile�enini al
        animator = GetComponent<Animator>();

        suankiSahneIndex = SceneManager.GetActiveScene().buildIndex;
    }

    private void Update()
    {
        suankiSahneIndex = SceneManager.GetActiveScene().buildIndex;
    }

    public void Restart()
    {
        StartCoroutine(PlaySoundAndLoadScene(suankiSahneIndex));
    }
    public void Next()
    {
        StartCoroutine(PlaySoundAndLoadScene(suankiSahneIndex+1));
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



    // Ses �alma metodunu olu�tur
    public void PlayAudioTusSesi(float volume)
    {
        if (bolumSonuTusSesi != null && bolumSonuTusSesi.clip != null)
        {
            // Ses seviyesini ayarla
            bolumSonuTusSesi.volume = Mathf.Clamp(volume, 0f, 1f); // 0 ile 1 aras�nda s�n�rla
            bolumSonuTusSesi.Play();
        }
        else
        {
            Debug.LogWarning("AudioSource veya AudioClip bulunamad�!");
        }
    }

    public void PlayAudioYildizSesi(float volume)
    {
        if (bolumSonuYildizSesi != null && bolumSonuYildizSesi.clip != null)
        {
            // Ses seviyesini ayarla
            bolumSonuYildizSesi.volume = Mathf.Clamp(volume, 0f, 1f); // 0 ile 1 aras�nda s�n�rla
            bolumSonuYildizSesi.Play();
        }
        else
        {
            Debug.LogWarning("AudioSource veya AudioClip bulunamad�!");
        }
    }
}
