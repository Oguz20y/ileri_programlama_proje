using UnityEngine;

public class Plot : MonoBehaviour
{
    [Header("Referanslar")]
    [SerializeField] private SpriteRenderer spriteRenderer; // Plot'un g�rselini kontrol eden SpriteRenderer
    [SerializeField] private Color hoverRengi;             // Fare �zerine geldi�inde de�i�ecek renk

    private GameObject kule;       // �zerine yerle�tirilen kule
    private Color baslangicRengi;  // Plot'un ba�lang�� rengi

    private void Start()
    {
        baslangicRengi = spriteRenderer.color; // �lk rengi kaydet
    }

    private void OnMouseEnter()
    {
        // Fare �zerine geldi�inde rengi de�i�tir
        spriteRenderer.color = hoverRengi;
    }

    private void OnMouseExit()
    {
        // Fare ��k�nca ba�lang�� rengine d�n
        spriteRenderer.color = baslangicRengi;
    }

    private void OnMouseDown()
    {
        if (kule != null)
        {
            Debug.Log("Buraya zaten bir kule yerle�tirildi!");
            return;
        }

        // Se�ili kuleyi BuildManager'dan al
        Tower secilenKule = BuildManager.main.SeciliKuleyiAl();

        if (secilenKule == null)
        {
            Debug.LogWarning("Se�ili bir kule yok!");
            return;
        }

        // Kule bedelini kontrol et
        if (secilenKule.bedel > LevelManager.main.para)
        {
            Debug.Log("Bu kuleyi in�a etmek i�in yeterli paran�z yok.");
            return;
        }

        // Para d���r ve kuleyi in�a et
        LevelManager.main.ParaHarca(secilenKule.bedel);
        kule = Instantiate(secilenKule.prefab, transform.position, Quaternion.identity);
    }
}
