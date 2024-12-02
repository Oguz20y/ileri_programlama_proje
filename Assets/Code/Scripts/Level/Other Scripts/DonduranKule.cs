using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DonduranKule : MonoBehaviour
{
    [Header("Referanslar")]
    [SerializeField] private LayerMask dusmanMaskesi;

    [Header("Özellikler")]
    [SerializeField] private float hedeflemeMenzili = 5f;
    [SerializeField] private float saniyeBasiSaldiri = 1f; // Saldýrý hýzý
    [SerializeField] private float dondurmaSuresi = 1f;

    private float saldiriSayaci;
    private HashSet<EnemyMovement> dondurulanDusmanlar = new HashSet<EnemyMovement>();

    private void Update()
    {
        // Saldýrý zamaný kontrolü
        saldiriSayaci += Time.deltaTime;
        if (saldiriSayaci >= 1f / saniyeBasiSaldiri)
        {
            DondurDusmanlar();
            saldiriSayaci = 0f;
        }
    }

    private void DondurDusmanlar()
    {
        // Menzil içerisindeki düþmanlarý tarar
        RaycastHit2D[] vurulanlar = Physics2D.CircleCastAll(transform.position, hedeflemeMenzili, Vector2.zero, 0f, dusmanMaskesi);

        foreach (RaycastHit2D vurulan in vurulanlar)
        {
            // Düþman bileþenini al
            EnemyMovement dusman = vurulan.transform.GetComponent<EnemyMovement>();

            // Eðer düþman geçerli ve daha önce dondurulmamýþsa
            if (dusman != null && !dondurulanDusmanlar.Contains(dusman))
            {
                dusman.HizGuncelle((0.5f)); // Hýzý yarýya indir
                dusman.RenkAyarla(Color.blue); // Rengi mavi yap
                dondurulanDusmanlar.Add(dusman);
                StartCoroutine(DondurmeSuresiBitir(dusman));
            }
        }
    }

    private IEnumerator DondurmeSuresiBitir(EnemyMovement dusman)
    {
        // Belirli bir süre bekler, ardýndan düþmanýn hýzýný ve rengini sýfýrlar
        yield return new WaitForSeconds(dondurmaSuresi);
        if (dusman != null)
        {
            dusman.HizSifirla();
            dusman.RenkSifirla();
            dondurulanDusmanlar.Remove(dusman);
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Menzili görselleþtirir
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, hedeflemeMenzili);
    }
}
