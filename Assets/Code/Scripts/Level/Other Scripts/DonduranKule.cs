using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DonduranKule : MonoBehaviour
{
    [Header("Referanslar")]
    [SerializeField] private LayerMask dusmanMaskesi;

    [Header("�zellikler")]
    [SerializeField] private float hedeflemeMenzili = 5f;
    [SerializeField] private float saniyeBasiSaldiri = 1f; // Sald�r� h�z�
    [SerializeField] private float dondurmaSuresi = 1f;

    private float saldiriSayaci;
    private HashSet<EnemyMovement> dondurulanDusmanlar = new HashSet<EnemyMovement>();

    private void Update()
    {
        // Sald�r� zaman� kontrol�
        saldiriSayaci += Time.deltaTime;
        if (saldiriSayaci >= 1f / saniyeBasiSaldiri)
        {
            DondurDusmanlar();
            saldiriSayaci = 0f;
        }
    }

    private void DondurDusmanlar()
    {
        // Menzil i�erisindeki d��manlar� tarar
        RaycastHit2D[] vurulanlar = Physics2D.CircleCastAll(transform.position, hedeflemeMenzili, Vector2.zero, 0f, dusmanMaskesi);

        foreach (RaycastHit2D vurulan in vurulanlar)
        {
            // D��man bile�enini al
            EnemyMovement dusman = vurulan.transform.GetComponent<EnemyMovement>();

            // E�er d��man ge�erli ve daha �nce dondurulmam��sa
            if (dusman != null && !dondurulanDusmanlar.Contains(dusman))
            {
                dusman.HizGuncelle((0.5f)); // H�z� yar�ya indir
                dusman.RenkAyarla(Color.blue); // Rengi mavi yap
                dondurulanDusmanlar.Add(dusman);
                StartCoroutine(DondurmeSuresiBitir(dusman));
            }
        }
    }

    private IEnumerator DondurmeSuresiBitir(EnemyMovement dusman)
    {
        // Belirli bir s�re bekler, ard�ndan d��man�n h�z�n� ve rengini s�f�rlar
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
        // Menzili g�rselle�tirir
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, hedeflemeMenzili);
    }
}
