using UnityEngine;
using TMPro;

public class Menu : MonoBehaviour
{
    [Header("Referanslar")]
    [SerializeField] private TextMeshProUGUI paraUI; // Para göstergesi
    [SerializeField] private Animator animasyon;    // Menü animasyonu

    private bool menuAcik = true;

    // Menüyü açýp kapatýr.
    public void MenuDegistir()
    {
        menuAcik = !menuAcik;
        animasyon.SetBool("MenuOpen", menuAcik);
    }

    private void Update()
    {
        // Para deðerini sürekli günceller
        if (LevelManager.main != null)
        {
            paraUI.text = LevelManager.main.para.ToString();
        }
        else
        {
            Debug.LogWarning("LevelManager bulunamadý!");
        }
    }

    // Seçim yapýldýðýnda çalýþtýrýlacak metot.
    // Þu anda iþlevsel deðil, gelecekte kullanýlabilir.
    public void SecimYapildi()
    {
        Debug.Log("Bir seçim yapýldý.");
        // Gelecekte seçim iþlevi için kullanýlabilir.
    }
}
