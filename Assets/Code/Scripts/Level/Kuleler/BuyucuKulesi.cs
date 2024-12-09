using System.Collections;
using UnityEngine;

public class BuyucuKulesi : MonoBehaviour
{
    [Header("Referanslar")]
    [SerializeField] private LayerMask dusmanMaskesi;
    [SerializeField] private AudioSource buyucuKuleMuhimmatSFX;
    [SerializeField] private GameObject buz;

    [Header("�zellikler")]
    [SerializeField] private float hedeflemeMenzili = 5f;
    [SerializeField] private float saniyeBasiSaldiri = 1f; // Sald�r� h�z�
    [SerializeField] private float dondurmaSuresi = 1f;
    [SerializeField] private float yavasHiz = 0.5f;

    private float saldiriSayaci;

    private void Update()
    {
        // Menzilde d��man var m� kontrol et
        RaycastHit2D[] vurulanlar = Physics2D.CircleCastAll(transform.position, hedeflemeMenzili, Vector2.zero, 0f, dusmanMaskesi);

        if (vurulanlar.Length > 0) // E�er menzilde d��man varsa
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
            // D��man bile�enini al
            DusmanHareketi dusman = vurulan.transform.GetComponent<DusmanHareketi>();

            if (dusman != null)
            {
                // Buz efekti aktif hale getir
                if (buz != null)
                {
                    buz.SetActive(true);
                }

                // D��man her sald�r�da tekrar dondurulur
                dusman.HizGuncelle(yavasHiz); // H�z� yar�ya indir
                dusman.RenkAyarla(Color.blue); // Rengi mavi yap
                StartCoroutine(DondurmaSuresiBitir(dusman));
            }
        }
    }

    private IEnumerator DondurmaSuresiBitir(DusmanHareketi dusman)
    {
        // Belirli bir s�re bekler, ard�ndan d��man�n h�z�n� ve rengini s�f�rlar
        yield return new WaitForSeconds(dondurmaSuresi);

        if (dusman != null)
        {
            dusman.HizSifirla();
            dusman.RenkSifirla();
        }

        // Buz efekti devre d��� b�rak
        if (buz != null)
        {
            buz.SetActive(false);
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Menzili g�rselle�tirir
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
            Debug.LogError("buyucuKuleMuhimmatSFX bile�eni bulunamad�!");
        }
    }

}
