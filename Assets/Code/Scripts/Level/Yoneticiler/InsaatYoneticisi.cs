using UnityEngine;

public class InsaatYoneticisi : MonoBehaviour
{
    public static InsaatYoneticisi main;

    [Header("Referanslar")]
    public Kule[] kuleler; // Kule türlerini içeren dizi

    private int secilenKule = 0; // Seçili kule indeksi

    private void Awake()
    {
        // Singleton yapýsý
        if (main == null)
        {
            main = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Seçili kuleyi döndürür.
    public Kule SeciliKuleyiAl()
    {
        if (kuleler != null && secilenKule >= 0 && secilenKule < kuleler.Length)
        {
            return kuleler[secilenKule];
        }
        else
        {
            Debug.LogWarning("Seçili kule geçersiz veya kule listesi boþ!");
            return null;
        }
    }

    // Seçili kuleyi ayarlar.
    public void SeciliKuleyiAyarla(int yeniSecilenKule)
    {
        if (yeniSecilenKule >= 0 && yeniSecilenKule < kuleler.Length)
        {
            secilenKule = yeniSecilenKule;
        }
        else
        {
            Debug.LogWarning("Geçersiz kule indeksi!");
        }
    }
}
