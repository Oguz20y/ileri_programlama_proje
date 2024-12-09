using UnityEngine;

public class HavanTopuKulesi : MonoBehaviour
{
    [Header("Referanslar")]
    [SerializeField] private LayerMask dusmanMaskesi; // D��manlar� alg�lamak i�in katman maskesi
    [SerializeField] private GameObject mermiPrefab;  // Ate�lenecek mermi prefab�
    [SerializeField] private Transform atisNoktasi;  // Merminin ��k�� noktas�
    [SerializeField] private Animator kuleAnimatoru;


    [Header("�zellikler")]
    [SerializeField] private float hedeflemeMenzili = 4f;    // Kule hedefleme menzili
    [SerializeField] private float saniyeBasiMermi = 1f;    // Saniyede at�lan mermi say�s�

    private Transform hedef;    // Kule taraf�ndan hedef al�nan d��man
    private float atisSayaci;   // Ate� etme zaman�n� kontrol eden saya�

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
        
        Debug.Log("Patlay�c� Ate� Edildi");
        GameObject mermiObjesi = Instantiate(mermiPrefab, atisNoktasi.position, Quaternion.identity);

        // `PatlayiciMermi` bile�enini kontrol et
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
            hedef = vurulanlar[0].transform; // �lk vurulan hedef olarak se�ilir
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, hedeflemeMenzili);
    }

    
}
