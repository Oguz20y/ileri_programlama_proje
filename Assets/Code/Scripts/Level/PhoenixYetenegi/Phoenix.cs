using UnityEngine;

public class Phoenix : MonoBehaviour
{
    [Header("Referanslar")]
    [SerializeField] private LayerMask dusmanMaskesi; // Hedeflenecek düþman katmaný
    [SerializeField] private Animator animasyon; // Phoenix animasyonu
    [SerializeField] private GameObject etkiAlaniGorseliPrefab; // Etki alanýný görselleþtiren prefab
    [SerializeField] private AudioSource atesSFX;

    [Header("Özellikler")]
    [SerializeField] private float etkiAlani = 2f; // Etki alaný yarýçapý
    [SerializeField] private int hasar = 50; // Verilecek hasar

    private GameObject aktifEtkiGorseli;

    // Etki alanýný göstermek için animasyonun baþýnda çaðrýlacak event.
    public void EtkiAlaniniGoster()
    {
        if (etkiAlaniGorseliPrefab != null)
        {
            aktifEtkiGorseli = Instantiate(etkiAlaniGorseliPrefab, transform.position, Quaternion.identity);
            aktifEtkiGorseli.transform.localScale = new Vector3(etkiAlani * 2, etkiAlani * 2, 1); // Çapý yarýçapla ölçekle
        }
    }

    // Hasar vermek için animasyonun sonunda çaðrýlacak event.
    public void HasarVer()
    {
        // Etki alanýný yok et (görseli temizle)
        if (aktifEtkiGorseli != null)
        {
            Destroy(aktifEtkiGorseli);
        }

        // Etki alanýndaki düþmanlara hasar ver
        Collider2D[] dusmanlar = Physics2D.OverlapCircleAll(transform.position, etkiAlani, dusmanMaskesi);

        foreach (Collider2D dusman in dusmanlar)
        {
            var dusmanSaglik = dusman.GetComponent<DusmanCani>();
            if (dusmanSaglik != null)
            {
                dusmanSaglik.HasarAl(hasar);
            }
        }

        // Phoenix objesini yok et
        Destroy(gameObject);
    }

    public void AtesSesiOynat()
    {
        if (atesSFX != null)
        {
            // Ses kaydýný hýzlandýrarak 3 saniyeden 1 saniyeye düþür.
            float originalDuration = atesSFX.clip.length; // Ses kaydýnýn orijinal uzunluðu
            float targetDuration = 1f; // Hedef uzunluk (1 saniye)

            // pitch deðerini hesapla
            atesSFX.pitch = originalDuration / targetDuration;

            // Ses kaydýný çal
            atesSFX.Play();
        }
        else
        {
            Debug.LogError("AudioSource atanmadý!");
        }
        atesSFX.Play();
    }
}
