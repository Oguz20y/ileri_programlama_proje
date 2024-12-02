using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager main;

    [SerializeField] private Animator uiAnimator; // Animator referans�
    [SerializeField] private TMP_Text dalgaSayisiText; // Dalga numaras�n� g�stermek i�in Text bile�eni (veya UnityEngine.UI.Text)

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

        // Dalga numaras�n� yaln�zca dalga ba�larken g�ncelle
        if (dalgaSayisiText != null)
        {
            if (dalgaBasliyorMu)
            {
                dalgaSayisiText.text = $"Dalga {dalgaNumarasi}";
            }
        }
    }
}
