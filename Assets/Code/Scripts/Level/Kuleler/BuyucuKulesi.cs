using System.Collections;
using UnityEngine;

public class BuyucuKulesi : MonoBehaviour
{
    [Header("Referanslar")]
    [SerializeField] private LayerMask dusmanMaskesi;
    [SerializeField] private AudioSource buyucuKuleMuhimmatSFX;
    [SerializeField] private GameObject buz;

    [Header("Özellikler")]
    [SerializeField] private float hedeflemeMenzili = 5f;
    [SerializeField] private float saniyeBasiSaldiri = 1f; // Saldýrý hýzý
    [SerializeField] private float dondurmaSuresi = 1f;
    [SerializeField] private float yavasHiz = 0.5f;

    private float saldiriSayaci;

    private void Update()
    {
        // Menzilde düþman var mý kontrol et
        RaycastHit2D[] vurulanlar = Physics2D.CircleCastAll(transform.position, hedeflemeMenzili, Vector2.zero, 0f, dusmanMaskesi);

        if (vurulanlar.Length > 0) // Eðer menzilde düþman varsa
        {
            saldiriSayaci += Time.deltaTime;
            if (saldiriSayaci >= 1f / saniyeBasiSaldiri)
            {
                SesiOynat();
                DondurDusmanlar(vurulanlar);
                saldiriSayaci = 0f;
            }
        }
    }

    private void DondurDusmanlar(RaycastHit2D[] vurulanlar)
    {
        foreach (RaycastHit2D vurulan in vurulanlar)
        {
            // Düþman bileþenini al
            DusmanHareketi dusman = vurulan.transform.GetComponent<DusmanHareketi>();

            if (dusman != null)
            {
                // Buz efekti aktif hale getir
                if (buz != null)
                {
                    buz.SetActive(true);
                }

                // Düþman her saldýrýda tekrar dondurulur
                dusman.HizGuncelle(yavasHiz); // Hýzý yarýya indir
                dusman.RenkAyarla(Color.blue); // Rengi mavi yap
                StartCoroutine(DondurmaSuresiBitir(dusman));
            }
        }
    }

    private IEnumerator DondurmaSuresiBitir(DusmanHareketi dusman)
    {
        // Belirli bir süre bekler, ardýndan düþmanýn hýzýný ve rengini sýfýrlar
        yield return new WaitForSeconds(dondurmaSuresi);

        if (dusman != null)
        {
            dusman.HizSifirla();
            dusman.RenkSifirla();
        }

        // Buz efekti devre dýþý býrak
        if (buz != null)
        {
            buz.SetActive(false);
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Menzili görselleþtirir
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, hedeflemeMenzili);
    }

    public void SesiOynat()
    {
        // Ses oynat
        if (buyucuKuleMuhimmatSFX != null)
        {
            buyucuKuleMuhimmatSFX.Play();
        }
        else
        {
            Debug.LogError("buyucuKuleMuhimmatSFX bileþeni bulunamadý!");
        }
    }

}
