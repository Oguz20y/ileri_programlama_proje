using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BolumSonuMenu : MonoBehaviour
{
    private int suankiSahneIndex;

    /*
    [SerializeField] private Image yildiz_1;
    [SerializeField] private Image yildiz_2;
    [SerializeField] private Image yildiz_3;
    */

    private float suankiOyuncuCani;

    [SerializeField] private AudioSource bolumSonuTusSesi;
    [SerializeField] private AudioSource bolumSonuYildizSesi;

    private Animator animator;
    void Start()
    {
        // Animator bile�enini al
        animator = GetComponent<Animator>();


        this.gameObject.SetActive(false);

        suankiOyuncuCani = LevelYoneticisi.main.suankiOyuncuCani;
        // Bulundu�unuz sahnenin index'ini al
        suankiSahneIndex = SceneManager.GetActiveScene().buildIndex;

        YildizSayisiAyarlamak();
    }

    private void Update()
    {
        suankiSahneIndex = SceneManager.GetActiveScene().buildIndex;
        YildizSayisiAyarlamak();
    }

    public void Restart()
    {
        StartCoroutine(PlaySoundAndLoadScene(suankiSahneIndex));
    }
    public void Next()
    {
        StartCoroutine(PlaySoundAndLoadScene(suankiSahneIndex + 1));
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

    private void YildizSayisiAyarlamak()
    {
        if (suankiOyuncuCani <= 30)
        {
            animator.SetInteger("Sonuc", 0);
            /*
            yildiz_1.gameObject.SetActive(true);
            yildiz_2.gameObject.SetActive(false);
            yildiz_3.gameObject.SetActive(false);
            */
        }
        else if (suankiOyuncuCani > 30 && suankiOyuncuCani <= 60)
        {
            animator.SetInteger("Sonuc", 1);
            /*
            yildiz_2.gameObject.SetActive(true);
            yildiz_1.gameObject.SetActive(false);
            yildiz_3.gameObject.SetActive(false);
            */
        }
        else
        {
            animator.SetInteger("Sonuc", 2);
            /*
            yildiz_3.gameObject.SetActive(true);
            yildiz_1.gameObject.SetActive(false);
            yildiz_2.gameObject.SetActive(false);
            */
        }
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

    /*
    private void YildizSayisiAyarlamak() 
    {
        if (endWave == true)
        {
            this.gameObject.SetActive(true);
            if (suankiOyuncuCani <= 30)
            {
                yildiz_1.gameObject.SetActive(true);
            }
            else if (suankiOyuncuCani > 30 && suankiOyuncuCani <= 60)
            {
                yildiz_2.gameObject.SetActive(true);
            }
            else
            {
                yildiz_3.gameObject.SetActive(true);
            }
        }
        else 
        {
            this.gameObject.SetActive(false);
        }
    }
    */
}
