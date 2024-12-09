using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class DusmanUreticisi_P : MonoBehaviour
{
    public static UnityEvent dusmanYokEdildi = new UnityEvent();

    [Header("Düþman Ayarlarý")]
    [SerializeField] private GameObject[] dusmanPrefablari; // Oluþturulacak düþman prefab'leri

    [Header("Dalga Ayarlarý")]
    [SerializeField] private int baslangicDusmanSayisi = 5; // Ýlk dalgada kaç düþman olacak
    [SerializeField] private float dalgaArasiSure = 5f; // Dalga arasýndaki bekleme süresi
    [SerializeField] private int toplamDalga = 10; // Toplam kaç dalga olacak
    [SerializeField] private float zorlukCarpani = 1.2f; // Dalga baþýna düþman artýþý

    [Header("Spawn Ayarlarý")]
    [SerializeField] private float saniyedeUretilecekDusmanSayisi = 1f; // Saniyede kaç düþman oluþturulacak (düþmanlar/saniye)

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
            ArayuzYoneticisi.main.DalgaDurumunuGuncelle(true, mevcutDalga); // Dalga baþlangýcýný UI'ya bildir

            yield return new WaitForSeconds(dalgaArasiSure);

            // Mevcut dalgadaki toplam düþman sayýsýný hesapla
            int toplamDusman = Mathf.RoundToInt(baslangicDusmanSayisi * Mathf.Pow(mevcutDalga, zorlukCarpani));

            // Spawn iþlemini baþlat
            yield return StartCoroutine(DusmanlariSpawnEt(toplamDusman));

            mevcutDalga++;
        }
    }

    private IEnumerator DusmanlariSpawnEt(int toplamDusman)
    {
        float spawnArasiSure = 1f / saniyedeUretilecekDusmanSayisi; // Spawn edilme süresi

        for (int i = 0; i < toplamDusman; i++)
        {
            DusmanOlustur();
            yield return new WaitForSeconds(spawnArasiSure);
        }
    }

    private void DusmanOlustur()
    {
        // Rastgele bir koridor seç
        int koridorIndeksi = Random.Range(0, LevelYoneticisi_P.anaYonetic.koridorlar.Length);
        var koridor = LevelYoneticisi_P.anaYonetic.koridorlar[koridorIndeksi];

        // Rastgele bir düþman prefab seç
        if (dusmanPrefablari == null || dusmanPrefablari.Length == 0)
        {
            Debug.LogError("Düþman prefablarý tanýmlý deðil!");
            return;
        }

        GameObject dusmanPrefab = dusmanPrefablari[Random.Range(0, dusmanPrefablari.Length)];

        // Koridorun baþlangýç noktasýna düþmaný oluþtur
        if (koridor.yolNoktalari.Length > 0 && koridor.yolNoktalari[0] != null)
        {
            Instantiate(dusmanPrefab, koridor.yolNoktalari[0].position, Quaternion.identity);
        }
        else
        {
            Debug.LogError("Koridor yol tanýmlý deðil!");
        }
    }
}
