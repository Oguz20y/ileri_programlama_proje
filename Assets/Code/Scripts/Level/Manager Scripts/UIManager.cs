using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager main;

    [SerializeField] private Animator uiAnimator; // Animator referansý
    [SerializeField] private TMP_Text dalgaSayisiText; // Dalga numarasýný göstermek için Text bileþeni (veya UnityEngine.UI.Text)

    private void Awake()
    {
        main = this; // Singleton
    }

    public void DalgaDurumunuGuncelle(bool dalgaBasliyorMu, int dalgaNumarasi)
    {
        if (uiAnimator != null)
        {
            uiAnimator.SetBool("AsamaBaslangici", dalgaBasliyorMu);
        }

        // Dalga numarasýný yalnýzca dalga baþlarken güncelle
        if (dalgaSayisiText != null)
        {
            if (dalgaBasliyorMu)
            {
                dalgaSayisiText.text = $"Dalga {dalgaNumarasi}";
            }
        }
    }
}
