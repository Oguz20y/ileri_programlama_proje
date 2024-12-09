using UnityEngine;
using UnityEngine.UI;

public class CanBari : MonoBehaviour
{

    public Slider slider;

    public void MaksimumCaniAyarla(int can) 
    {
        slider.maxValue = can;
        slider.value = can;
    }

    public void CaniGuncelle(int can) 
    {
        slider.value = can;
    }
}
