using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Turret : MonoBehaviour
{
    [Header("Referanslar")]
    [SerializeField] private LayerMask dusmanMaskesi; // D��manlar� alg�lamak i�in katman maskesi
    [SerializeField] private GameObject mermiPrefab;  // Ate�lenecek mermi prefab�
    [SerializeField] private Transform atisNoktasi;  // Merminin ��k�� noktas�

    [Header("�zellikler")]
    [SerializeField] private float hedeflemeMenzili = 4f;    // Taretin hedefleme menzili
    [SerializeField] private float saniyeBasiMermi = 1f;    // Saniyede at�lan mermi say�s�

    private Transform hedef;    // Taretin hedef ald��� d��man
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
            if (atisSayaci >= 1f / saniyeBasiMermi)
            {
                AtesEt();
                atisSayaci = 0f;
            }
        }
    }

    private void AtesEt()
    {
        Debug.Log("Ate� Edildi");
        GameObject mermiObjesi = Instantiate(mermiPrefab, atisNoktasi.position, Quaternion.identity);
        Bullet mermiScripti = mermiObjesi.GetComponent<Bullet>();
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
            hedef = vurulanlar[0].transform; // �lk vurulan hedef olarak se�ilir
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, hedeflemeMenzili); // Menzil g�stergesi
    }
}
