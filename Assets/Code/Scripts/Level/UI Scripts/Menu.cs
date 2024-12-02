using UnityEngine;
using TMPro;

public class Menu : MonoBehaviour
{
    [Header("Referanslar")]
    [SerializeField] private TextMeshProUGUI paraUI; // Para g�stergesi
    [SerializeField] private Animator animasyon;    // Men� animasyonu

    private bool menuAcik = true;

    // Men�y� a��p kapat�r.
    public void MenuDegistir()
    {
        menuAcik = !menuAcik;
        animasyon.SetBool("MenuOpen", menuAcik);
    }

    private void Update()
    {
        // Para de�erini s�rekli g�nceller
        if (LevelManager.main != null)
        {
            paraUI.text = LevelManager.main.para.ToString();
        }
        else
        {
            Debug.LogWarning("LevelManager bulunamad�!");
        }
    }

    // Se�im yap�ld���nda �al��t�r�lacak metot.
    // �u anda i�levsel de�il, gelecekte kullan�labilir.
    public void SecimYapildi()
    {
        Debug.Log("Bir se�im yap�ld�.");
        // Gelecekte se�im i�levi i�in kullan�labilir.
    }
}
