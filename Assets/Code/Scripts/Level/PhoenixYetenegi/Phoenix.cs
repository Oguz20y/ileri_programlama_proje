using UnityEngine;

public class Phoenix : MonoBehaviour
{
    [Header("Referanslar")]
    [SerializeField] private LayerMask dusmanMaskesi; // Hedeflenecek d��man katman�
    [SerializeField] private Animator animasyon; // Phoenix animasyonu
    [SerializeField] private GameObject etkiAlaniGorseliPrefab; // Etki alan�n� g�rselle�tiren prefab
    [SerializeField] private AudioSource atesSFX;

    [Header("�zellikler")]
    [SerializeField] private float etkiAlani = 2f; // Etki alan� yar��ap�
    [SerializeField] private int hasar = 50; // Verilecek hasar

    private GameObject aktifEtkiGorseli;

    // Etki alan�n� g�stermek i�in animasyonun ba��nda �a�r�lacak event.
    public void EtkiAlaniniGoster()
    {
        if (etkiAlaniGorseliPrefab != null)
        {
            aktifEtkiGorseli = Instantiate(etkiAlaniGorseliPrefab, transform.position, Quaternion.identity);
            aktifEtkiGorseli.transform.localScale = new Vector3(etkiAlani * 2, etkiAlani * 2, 1); // �ap� yar��apla �l�ekle
        }
    }

    // Hasar vermek i�in animasyonun sonunda �a�r�lacak event.
    public void HasarVer()
    {
        // Etki alan�n� yok et (g�rseli temizle)
        if (aktifEtkiGorseli != null)
        {
            Destroy(aktifEtkiGorseli);
        }

        // Etki alan�ndaki d��manlara hasar ver
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
            // Ses kayd�n� h�zland�rarak 3 saniyeden 1 saniyeye d���r.
            float originalDuration = atesSFX.clip.length; // Ses kayd�n�n orijinal uzunlu�u
            float targetDuration = 1f; // Hedef uzunluk (1 saniye)

            // pitch de�erini hesapla
            atesSFX.pitch = originalDuration / targetDuration;

            // Ses kayd�n� �al
            atesSFX.Play();
        }
        else
        {
            Debug.LogError("AudioSource atanmad�!");
        }
        atesSFX.Play();
    }
}
