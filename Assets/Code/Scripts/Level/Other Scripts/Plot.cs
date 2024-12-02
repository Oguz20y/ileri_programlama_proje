using UnityEngine;

public class Plot : MonoBehaviour
{
    [Header("Referanslar")]
    [SerializeField] private SpriteRenderer spriteRenderer; // Plot'un görselini kontrol eden SpriteRenderer
    [SerializeField] private Color hoverRengi;             // Fare üzerine geldiðinde deðiþecek renk

    private GameObject kule;       // Üzerine yerleþtirilen kule
    private Color baslangicRengi;  // Plot'un baþlangýç rengi

    private void Start()
    {
        baslangicRengi = spriteRenderer.color; // Ýlk rengi kaydet
    }

    private void OnMouseEnter()
    {
        // Fare üzerine geldiðinde rengi deðiþtir
        spriteRenderer.color = hoverRengi;
    }

    private void OnMouseExit()
    {
        // Fare çýkýnca baþlangýç rengine dön
        spriteRenderer.color = baslangicRengi;
    }

    private void OnMouseDown()
    {
        if (kule != null)
        {
            Debug.Log("Buraya zaten bir kule yerleþtirildi!");
            return;
        }

        // Seçili kuleyi BuildManager'dan al
        Tower secilenKule = BuildManager.main.SeciliKuleyiAl();

        if (secilenKule == null)
        {
            Debug.LogWarning("Seçili bir kule yok!");
            return;
        }

        // Kule bedelini kontrol et
        if (secilenKule.bedel > LevelManager.main.para)
        {
            Debug.Log("Bu kuleyi inþa etmek için yeterli paranýz yok.");
            return;
        }

        // Para düþür ve kuleyi inþa et
        LevelManager.main.ParaHarca(secilenKule.bedel);
        kule = Instantiate(secilenKule.prefab, transform.position, Quaternion.identity);
    }
}
