using UnityEngine;

public class HavanTopuKulesi : MonoBehaviour
{
    [Header("Referanslar")]
    [SerializeField] private LayerMask dusmanMaskesi; // Düþmanlarý algýlamak için katman maskesi
    [SerializeField] private GameObject mermiPrefab;  // Ateþlenecek mermi prefabý
    [SerializeField] private Transform atisNoktasi;  // Merminin çýkýþ noktasý
    [SerializeField] private Animator kuleAnimatoru;


    [Header("Özellikler")]
    [SerializeField] private float hedeflemeMenzili = 4f;    // Kule hedefleme menzili
    [SerializeField] private float saniyeBasiMermi = 1f;    // Saniyede atýlan mermi sayýsý

    private Transform hedef;    // Kule tarafýndan hedef alýnan düþman
    private float atisSayaci;   // Ateþ etme zamanýný kontrol eden sayaç

    private void Update()
    {
        if (hedef == null)
        {
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
            if (atisSayaci >= 1.2f / saniyeBasiMermi)
            {
                if (kuleAnimatoru != null)
                {
                    kuleAnimatoru.SetBool("Hareket", true);
                }
                //AtesEt();
                atisSayaci = 0f;
            }
            else
            {
                kuleAnimatoru.SetBool("Hareket", false);
            }
        }
    }

    private void AtesEt()
    {
        
        Debug.Log("Patlayýcý Ateþ Edildi");
        GameObject mermiObjesi = Instantiate(mermiPrefab, atisNoktasi.position, Quaternion.identity);

        // `PatlayiciMermi` bileþenini kontrol et
        PatlayiciMermi mermiScripti = mermiObjesi.GetComponent<PatlayiciMermi>();
        if (mermiScripti != null && hedef != null)
        {
            mermiScripti.HedefBelirle(hedef.position);
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
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, hedeflemeMenzili);
    }

    
}
