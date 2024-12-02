using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager main;

    [Header("Referanslar")]
    [SerializeField] private Tower[] kuleler; // Kule t�rlerini i�eren dizi

    private int secilenKule = 0; // Se�ili kule indeksi

    private void Awake()
    {
        // Singleton yap�s�
        if (main == null)
        {
            main = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Se�ili kuleyi d�nd�r�r.
    public Tower SeciliKuleyiAl()
    {
        if (kuleler != null && secilenKule >= 0 && secilenKule < kuleler.Length)
        {
            return kuleler[secilenKule];
        }
        else
        {
            Debug.LogWarning("Se�ili kule ge�ersiz veya kule listesi bo�!");
            return null;
        }
    }

    // Se�ili kuleyi ayarlar.
    public void SeciliKuleyiAyarla(int yeniSecilenKule)
    {
        if (yeniSecilenKule >= 0 && yeniSecilenKule < kuleler.Length)
        {
            secilenKule = yeniSecilenKule;
        }
        else
        {
            Debug.LogWarning("Ge�ersiz kule indeksi!");
        }
    }
}