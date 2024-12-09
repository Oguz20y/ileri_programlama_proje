using UnityEngine;
using UnityEngine.UI;

public class LevelYoneticisi_P : MonoBehaviour
{
    public static LevelYoneticisi_P anaYonetic;

    [System.Serializable]
    public class Koridor
    {
        public Transform[] yolNoktalari; // Her koridor i�in yol noktalar�
        public float zorlukCarpani; // Koridorun zorluk �arpan�
    }

    public Koridor[] koridorlar; // T�m koridorlar� tutan liste
    public int mevcutPara;
    public int mevcutCan;
    [SerializeField] private int maksimumCan = 100;
    public int dusmanHasar = 20;

    [SerializeField] private Image kaybetmeEkrani; // Oyuncunun kaybetti�i ekran

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
            Debug.Log("Yeterli paran�z yok!");
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
