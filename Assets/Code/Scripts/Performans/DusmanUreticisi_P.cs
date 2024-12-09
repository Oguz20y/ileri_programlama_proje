using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class DusmanUreticisi_P : MonoBehaviour
{
    public static UnityEvent dusmanYokEdildi = new UnityEvent();

    [Header("D��man Ayarlar�")]
    [SerializeField] private GameObject[] dusmanPrefablari; // Olu�turulacak d��man prefab'leri

    [Header("Dalga Ayarlar�")]
    [SerializeField] private int baslangicDusmanSayisi = 5; // �lk dalgada ka� d��man olacak
    [SerializeField] private float dalgaArasiSure = 5f; // Dalga aras�ndaki bekleme s�resi
    [SerializeField] private int toplamDalga = 10; // Toplam ka� dalga olacak
    [SerializeField] private float zorlukCarpani = 1.2f; // Dalga ba��na d��man art���

    [Header("Spawn Ayarlar�")]
    [SerializeField] private float saniyedeUretilecekDusmanSayisi = 1f; // Saniyede ka� d��man olu�turulacak (d��manlar/saniye)

    private int mevcutDalga = 1;

    private void Start()
    {
        StartCoroutine(DalgaYoneticisi());
    }

    private IEnumerator DalgaYoneticisi()
    {
        while (mevcutDalga <= toplamDalga)
        {
            yield return new WaitForSeconds(2);
            ArayuzYoneticisi.main.DalgaDurumunuGuncelle(true, mevcutDalga); // Dalga ba�lang�c�n� UI'ya bildir

            yield return new WaitForSeconds(dalgaArasiSure);

            // Mevcut dalgadaki toplam d��man say�s�n� hesapla
            int toplamDusman = Mathf.RoundToInt(baslangicDusmanSayisi * Mathf.Pow(mevcutDalga, zorlukCarpani));

            // Spawn i�lemini ba�lat
            yield return StartCoroutine(DusmanlariSpawnEt(toplamDusman));

            mevcutDalga++;
        }
    }

    private IEnumerator DusmanlariSpawnEt(int toplamDusman)
    {
        float spawnArasiSure = 1f / saniyedeUretilecekDusmanSayisi; // Spawn edilme s�resi

        for (int i = 0; i < toplamDusman; i++)
        {
            DusmanOlustur();
            yield return new WaitForSeconds(spawnArasiSure);
        }
    }

    private void DusmanOlustur()
    {
        // Rastgele bir koridor se�
        int koridorIndeksi = Random.Range(0, LevelYoneticisi_P.anaYonetic.koridorlar.Length);
        var koridor = LevelYoneticisi_P.anaYonetic.koridorlar[koridorIndeksi];

        // Rastgele bir d��man prefab se�
        if (dusmanPrefablari == null || dusmanPrefablari.Length == 0)
        {
            Debug.LogError("D��man prefablar� tan�ml� de�il!");
            return;
        }

        GameObject dusmanPrefab = dusmanPrefablari[Random.Range(0, dusmanPrefablari.Length)];

        // Koridorun ba�lang�� noktas�na d��man� olu�tur
        if (koridor.yolNoktalari.Length > 0 && koridor.yolNoktalari[0] != null)
        {
            Instantiate(dusmanPrefab, koridor.yolNoktalari[0].position, Quaternion.identity);
        }
        else
        {
            Debug.LogError("Koridor yol tan�ml� de�il!");
        }
    }
}
