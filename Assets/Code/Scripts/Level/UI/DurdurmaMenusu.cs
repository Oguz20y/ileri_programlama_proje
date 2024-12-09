using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DurdurmaMenusu : MonoBehaviour
{

    [Header("Referanslar")]
    [SerializeField] private GameObject duraklatmaMenusu; // Duraklatma men�s� UI'si
    [SerializeField] private AudioSource arkaPlanMuzik;  // Arka plan m�zi�i
    [SerializeField] private AudioSource butonSesi;     // T�klama ses efekti

    private int mevcutSahneIndex;

    private void Start()
    {
        // Bulundu�unuz sahnenin index'ini al
        mevcutSahneIndex = SceneManager.GetActiveScene().buildIndex;
        OyunaDevamEt(); // Oyun ba�lad���nda men� kapal� olacak �ekilde ba�la
    }

    private void Update()
    {
        // Sahne index'ini her karede kontrol et (gerekliyse)
        mevcutSahneIndex = SceneManager.GetActiveScene().buildIndex;

        // Esc tu�una bas�ld���nda duraklatma men�s�n� a�/kapat
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
        duraklatmaMenusu.SetActive(true);  // Duraklatma men�s�n� aktif et
        Time.timeScale = 0f;              // Oyun durdurulur
        if (arkaPlanMuzik != null)
        {
            arkaPlanMuzik.pitch = 0f;     // Arka plan m�zi�i durdurulur
        }
    }

    public void OyunaDevamEt()
    {
        duraklatmaMenusu.SetActive(false); // Duraklatma men�s�n� kapat
        Time.timeScale = 1f;               // Oyun devam eder
        if (arkaPlanMuzik != null)
        {
            arkaPlanMuzik.pitch = 1f;      // Arka plan m�zi�i devam eder
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
        Time.timeScale = 1f;  // Oyun h�z�n� normal hale getir
        if (butonSesi != null)
        {
            butonSesi.Play(); // Buton sesi oynat
            yield return new WaitForSeconds(butonSesi.clip.length); // Sesin bitmesini bekle
        }
        SceneManager.LoadScene(sahneIndex); // Yeni sahneyi y�kle
    }
}
