using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner main;

    [Header("Referanslar")]
    [SerializeField] private GameObject[] dusmanPrefablar;

    [Header("Ayarlar")]
    [SerializeField] private int temelDusmanSayisi = 8;
    [SerializeField] private int buDalgadakiDusmanSayisi;
    [SerializeField] private float dusmanlarSaniyede = 0.5f;
    [SerializeField] private float dalgalarArasiSure = 5f;
    [SerializeField] private float zorlukCarpani = 0.75f;
    [SerializeField] private int sonDalga = 10;

    public static UnityEvent dusmanYokEdildi = new UnityEvent();

    public int guncelDalga = 1;
    private float sonOlusturmaZamani;
    private int hayattaKalanDusmanSayisi;
    private int olusturulacakDusmanSayisi;
    private bool olusturuluyor = false;
    public bool dalgaBitti = false;

    public float[] olusturmaAgirliklari = { 0.5f, 0.2f, 0.3f };
    private float toplamAgirlik;

    public Image bolumSonuEkrani;
    private int oyuncuCani;

    private void Awake()
    {
        main = this;

        dusmanYokEdildi.AddListener(DusmanYokEdildi);

        foreach (float agirlik in olusturmaAgirliklari)
        {
            toplamAgirlik += agirlik;
        }
    }

    private void Start()
    {
        buDalgadakiDusmanSayisi = temelDusmanSayisi;
        OyunSonunuKontrolEt();
        StartCoroutine(DalgaBaslat());
    }

    private void Update()
    {
        OyunSonunuKontrolEt();
        if (!olusturuluyor || dalgaBitti) return;

        sonOlusturmaZamani += Time.deltaTime;

        if (sonOlusturmaZamani >= (1f / dusmanlarSaniyede) && olusturulacakDusmanSayisi > 0)
        {
            DusmanOlustur();
            olusturulacakDusmanSayisi--;
            hayattaKalanDusmanSayisi++;
            sonOlusturmaZamani = 0f;
        }

        if (hayattaKalanDusmanSayisi == 0 && olusturulacakDusmanSayisi == 0)
        {
            DalgaBitir();
            UIManager.main.DalgaDurumunuGuncelle(false, guncelDalga); // Dalga biti�ini bildir
        }
    }

    private IEnumerator DalgaBaslat()
    {
        yield return new WaitForSeconds(2);

        UIManager.main.DalgaDurumunuGuncelle(true, guncelDalga); // Dalga ba�lang�c�n� bildir

        yield return new WaitForSeconds(dalgalarArasiSure);
        olusturuluyor = true;
        olusturulacakDusmanSayisi = DalgaDusmanSayisi();
    }

    private void DalgaBitir()
    {
        olusturuluyor = false;
        sonOlusturmaZamani = 0f;

        if (guncelDalga >= sonDalga)
        {
            dalgaBitti = true;
            UIManager.main.DalgaDurumunuGuncelle(false, guncelDalga); // Son dalga oldu�u i�in bildir
            return;
        }

        guncelDalga++;
        OlusturmaAgirliklariniGuncelle();
        StartCoroutine(DalgaBaslat());
    }

    private void OlusturmaAgirliklariniGuncelle()
    {
        float temelTankArtisOrani = 0.02f;
        float temelHizliArtisOrani = 0.03f;

        float tankArtisOrani = temelTankArtisOrani * zorlukCarpani;
        float hizliArtisOrani = temelHizliArtisOrani * zorlukCarpani;

        float toplamArtis = tankArtisOrani + hizliArtisOrani;

        olusturmaAgirliklari[0] -= toplamArtis;
        olusturmaAgirliklari[1] += hizliArtisOrani;
        olusturmaAgirliklari[2] += tankArtisOrani;

        float toplam = 0f;
        foreach (float agirlik in olusturmaAgirliklari)
            toplam += agirlik;

        for (int i = 0; i < olusturmaAgirliklari.Length; i++)
            olusturmaAgirliklari[i] /= toplam;
    }

    private void DusmanOlustur()
    {
        int koridorIndex = Random.Range(0, LevelManager.main.koridorlar.Length);
        var koridor = LevelManager.main.koridorlar[koridorIndex];

        GameObject prefabToOlustur = RastgeleDusmanSec();
        Instantiate(prefabToOlustur, koridor.yolNoktalari[0].position, Quaternion.identity);
    }

    private void DusmanYokEdildi()
    {
        hayattaKalanDusmanSayisi--;
    }

    private int DalgaDusmanSayisi()
    {
        buDalgadakiDusmanSayisi = Mathf.RoundToInt(temelDusmanSayisi * Mathf.Pow(guncelDalga, zorlukCarpani));
        return buDalgadakiDusmanSayisi;
    }

    private GameObject RastgeleDusmanSec()
    {
        float rastgeleDeger = Random.Range(0f, toplamAgirlik);
        float birikenAgirlik = 0f;

        for (int i = 0; i < dusmanPrefablar.Length; i++)
        {
            birikenAgirlik += olusturmaAgirliklari[i];
            if (rastgeleDeger <= birikenAgirlik)
            {
                return dusmanPrefablar[i];
            }
        }

        return dusmanPrefablar[0];
    }

    private void OyunSonunuKontrolEt()
    {
        oyuncuCani = LevelManager.main.suankiOyuncuCani;
        if (dalgaBitti && oyuncuCani > 0)
        {
            bolumSonuEkrani.gameObject.SetActive(true);
        }
        else
        {
            bolumSonuEkrani.gameObject.SetActive(false);
        }
    }
}
