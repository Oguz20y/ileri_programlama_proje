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
        // Animator bileþenini al
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

    [SerializeField] private AudioSource clickSound; // Ses efekti kaynaðý

    public void SesliButon_SceneDegistirmek(int scene)
    {
        StartCoroutine(PlaySoundAndLoadScene(scene));
    }
    private IEnumerator PlaySoundAndLoadScene(int scene)
    {
        Time.timeScale = 1f;  // PauseMenu'ye özel kod satýrý!
        clickSound.Play();
        yield return new WaitForSeconds(clickSound.clip.length);
        // Yeni sahneyi yükle
        SceneManager.LoadScene(scene);
    }



    // Ses çalma metodunu oluþtur
    public void PlayAudioTusSesi(float volume)
    {
        if (bolumSonuTusSesi != null && bolumSonuTusSesi.clip != null)
        {
            // Ses seviyesini ayarla
            bolumSonuTusSesi.volume = Mathf.Clamp(volume, 0f, 1f); // 0 ile 1 arasýnda sýnýrla
            bolumSonuTusSesi.Play();
        }
        else
        {
            Debug.LogWarning("AudioSource veya AudioClip bulunamadý!");
        }
    }

    public void PlayAudioYildizSesi(float volume)
    {
        if (bolumSonuYildizSesi != null && bolumSonuYildizSesi.clip != null)
        {
            // Ses seviyesini ayarla
            bolumSonuYildizSesi.volume = Mathf.Clamp(volume, 0f, 1f); // 0 ile 1 arasýnda sýnýrla
            bolumSonuYildizSesi.Play();
        }
        else
        {
            Debug.LogWarning("AudioSource veya AudioClip bulunamadý!");
        }
    }
}
