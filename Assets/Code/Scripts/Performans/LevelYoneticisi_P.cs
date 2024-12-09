using UnityEngine;
using UnityEngine.UI;

public class LevelYoneticisi_P : MonoBehaviour
{
    public static LevelYoneticisi_P anaYonetic;

    [System.Serializable]
    public class Koridor
    {
        public Transform[] yolNoktalari; // Her koridor için yol noktalarý
        public float zorlukCarpani; // Koridorun zorluk çarpaný
    }

    public Koridor[] koridorlar; // Tüm koridorlarý tutan liste
    public int mevcutPara;
    public int mevcutCan;
    [SerializeField] private int maksimumCan = 100;
    public int dusmanHasar = 20;

    [SerializeField] private Image kaybetmeEkrani; // Oyuncunun kaybettiði ekran

    private void Awake()
    {
        anaYonetic = this; // Singleton
    }

    private void Start()
    {
        mevcutCan = maksimumCan;
        mevcutPara = 300;
    }

    private void Update()
    {
        OyununBitisiniKontrolEt();
    }

    public void ParaArttir(int miktar)
    {
        mevcutPara += miktar;
    }

    public bool ParaHarca(int miktar)
    {
        if (miktar <= mevcutPara)
        {
            mevcutPara -= miktar;
            return true;
        }
        else
        {
            Debug.Log("Yeterli paranýz yok!");
            return false;
        }
    }

    private void OyununBitisiniKontrolEt()
    {
        if (mevcutCan <= 0)
        {
            kaybetmeEkrani.gameObject.SetActive(true);
        }
    }
}
