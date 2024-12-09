using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DurdurmaMenusu : MonoBehaviour
{

    [Header("Referanslar")]
    [SerializeField] private GameObject duraklatmaMenusu; // Duraklatma menüsü UI'si
    [SerializeField] private AudioSource arkaPlanMuzik;  // Arka plan müziði
    [SerializeField] private AudioSource butonSesi;     // Týklama ses efekti

    private int mevcutSahneIndex;

    private void Start()
    {
        // Bulunduðunuz sahnenin index'ini al
        mevcutSahneIndex = SceneManager.GetActiveScene().buildIndex;
        OyunaDevamEt(); // Oyun baþladýðýnda menü kapalý olacak þekilde baþla
    }

    private void Update()
    {
        // Sahne index'ini her karede kontrol et (gerekliyse)
        mevcutSahneIndex = SceneManager.GetActiveScene().buildIndex;

        // Esc tuþuna basýldýðýnda duraklatma menüsünü aç/kapat
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Time.timeScale == 0f)
            {
                OyunaDevamEt();
            }
            else
            {
                Duraklat();
            }
        }
    }

    public void Duraklat()
    {
        duraklatmaMenusu.SetActive(true);  // Duraklatma menüsünü aktif et
        Time.timeScale = 0f;              // Oyun durdurulur
        if (arkaPlanMuzik != null)
        {
            arkaPlanMuzik.pitch = 0f;     // Arka plan müziði durdurulur
        }
    }

    public void OyunaDevamEt()
    {
        duraklatmaMenusu.SetActive(false); // Duraklatma menüsünü kapat
        Time.timeScale = 1f;               // Oyun devam eder
        if (arkaPlanMuzik != null)
        {
            arkaPlanMuzik.pitch = 1f;      // Arka plan müziði devam eder
        }
    }

    public void SahneyiYenidenBaslat()
    {
        StartCoroutine(SesOynatVeSahneYukle(mevcutSahneIndex));
    }

    public void SahneDegistir(int sahneIndex)
    {
        StartCoroutine(SesOynatVeSahneYukle(sahneIndex));
    }

    private IEnumerator SesOynatVeSahneYukle(int sahneIndex)
    {
        Time.timeScale = 1f;  // Oyun hýzýný normal hale getir
        if (butonSesi != null)
        {
            butonSesi.Play(); // Buton sesi oynat
            yield return new WaitForSeconds(butonSesi.clip.length); // Sesin bitmesini bekle
        }
        SceneManager.LoadScene(sahneIndex); // Yeni sahneyi yükle
    }
}
