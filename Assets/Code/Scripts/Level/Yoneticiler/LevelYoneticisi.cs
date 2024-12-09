using UnityEngine;
using UnityEngine.UI;

public class LevelYoneticisi : MonoBehaviour
{
    public static LevelYoneticisi main;

    [System.Serializable]
    public class Koridor
    {
        public Transform[] yolNoktalari; // Her koridor için yol noktalarý
    }

    public Koridor[] koridorlar; // Birden fazla koridoru destekleyen liste

    public int para = 300;
    public int suankiOyuncuCani;
    [SerializeField] private int maksimumOyuncuCani = 100;
    public int dusmanHasar = 20;

    public CanBari canBari;
    public Image failedEkrani; // Bölüm sonu ekranýný temsil eden Image nesnesi

    private void Awake()
    {
        main = this;
    }

    private void Start()
    {
        suankiOyuncuCani = maksimumOyuncuCani;
        canBari.MaksimumCaniAyarla(maksimumOyuncuCani);
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
            Debug.Log("Bu eþyayý almak için yeterli paranýz yok.");
            return false;
        }
    }

    private void OyunuKontrolEt()
    {
        if (suankiOyuncuCani <= 0)
        {
            ArayuzYoneticisi.main.DalgaDurumunuGuncelle(false, DusmanUreticisi.main.guncelDalga); // Son dalga olduðu için arayüze bildir
            failedEkrani.gameObject.SetActive(true); // Bölüm sonu ekranýný aktif et
        }
        else
        {
            failedEkrani.gameObject.SetActive(false); // Bölüm sonu ekranýný deaktif et
        }
    }
}
