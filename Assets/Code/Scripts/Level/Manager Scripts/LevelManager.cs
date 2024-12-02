using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public static LevelManager main;

    [System.Serializable]
    public class Koridor
    {
        public Transform[] yolNoktalari; // Her koridor i�in yol noktalar�
    }

    public Koridor[] koridorlar; // Birden fazla koridoru destekleyen liste

    public int para;
    public int suankiOyuncuCani;
    [SerializeField] private int maksimumOyuncuCani = 100;
    public int dusmanHasar = 20;

    public CanBari canBari;
    public Image failedEkrani; // B�l�m sonu ekran�n� temsil eden Image nesnesi

    private void Awake()
    {
        main = this;
    }

    private void Start()
    {
        suankiOyuncuCani = maksimumOyuncuCani;
        canBari.MaksimumCaniAyarla(maksimumOyuncuCani);
        para = 300;
    }

    private void Update()
    {
        OyunuKontrolEt();
        canBari.CaniGuncelle(suankiOyuncuCani);
    }

    public void ParaArtir(int miktar)
    {
        para += miktar;
    }

    public bool ParaHarca(int miktar)
    {
        if (miktar <= para)
        {
            para -= miktar;
            return true;
        }
        else
        {
            Debug.Log("Bu e�yay� almak i�in yeterli paran�z yok.");
            return false;
        }
    }

    private void OyunuKontrolEt()
    {
        if (suankiOyuncuCani <= 0)
        {
            UIManager.main.DalgaDurumunuGuncelle(false, EnemySpawner.main.guncelDalga); // Son dalga oldu�u i�in aray�ze bildir
            failedEkrani.gameObject.SetActive(true); // B�l�m sonu ekran�n� aktif et
        }
        else
        {
            failedEkrani.gameObject.SetActive(false); // B�l�m sonu ekran�n� deaktif et
        }
    }
}
