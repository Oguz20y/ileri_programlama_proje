using UnityEngine;

public class OkcuKulesi : MonoBehaviour
{
    [Header("Referanslar")]
    [SerializeField] private LayerMask dusmanMaskesi; // Düþmanlarý algýlamak için katman maskesi
    [SerializeField] private GameObject mermiPrefab;  // Ateþlenecek mermi prefabý
    [SerializeField] private Transform atisNoktasi;  // Merminin çýkýþ noktasý
    [SerializeField] private AudioSource okSFX;
    [SerializeField] private Animator okcuAnimatoru;

    [Header("Özellikler")]
    [SerializeField] private float hedeflemeMenzili = 4f;    // Taretin hedefleme menzili
    [SerializeField] private float saniyeBasiMermi = 1f;    // Saniyede atýlan mermi sayýsý

    private Transform hedef;    // Taretin hedef aldýðý düþman
    private float atisSayaci;   // Ateþ etme zamanýný kontrol eden sayaç

    private void Update()
    {
        if (hedef == null)
        {
            okcuAnimatoru.SetBool("OkAt", false);
            HedefBul();
            return;
        }

        if (!HedefMenzildeMi())
        {
            hedef = null;
        }
        else
        {
            atisSayaci += Time.deltaTime;
            if (atisSayaci >= 1f / saniyeBasiMermi)
            {
                okcuAnimatoru.SetBool("OkAt", true);
                //AtesEt();
                atisSayaci = 0f;
            }
            else
            {
                okcuAnimatoru.SetBool("OkAt", false);
            }
            
        }
    }

    private void AtesEt()
    {
        SesiOynat();
        Debug.Log("Ateþ Edildi");
        GameObject mermiObjesi = Instantiate(mermiPrefab, atisNoktasi.position, Quaternion.identity);
        Mermi mermiScripti = mermiObjesi.GetComponent<Mermi>();
        if (mermiScripti != null)
        {
            mermiScripti.HedefBelirle(hedef);
        }
    }

    private bool HedefMenzildeMi()
    {
        return hedef != null && Vector2.Distance(hedef.position, transform.position) <= hedeflemeMenzili;
    }

    private void HedefBul()
    {
        RaycastHit2D[] vurulanlar = Physics2D.CircleCastAll(transform.position, hedeflemeMenzili, Vector2.zero, 0f, dusmanMaskesi);
        if (vurulanlar.Length > 0)
        {
            hedef = vurulanlar[0].transform; // Ýlk vurulan hedef olarak seçilir
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, hedeflemeMenzili); // Menzil göstergesi
    }

    public void SesiOynat()
    {
        // Ses oynat
        if (okSFX != null)
        {
            okSFX.Play();
        }
        else
        {
            Debug.LogError("okSFX bileþeni bulunamadý!");
        }
    }
}
