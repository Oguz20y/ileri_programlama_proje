using System.Collections;
using UnityEngine;

public class DusmanCani: MonoBehaviour
{
    private DusmanHareketi dusmanHareketi;

    [Header("Referanslar")]
    [SerializeField] private Animator dusmanAnimatoru;
    [SerializeField] private SpriteRenderer spriteRenderer; // SpriteRenderer referansý

    [Header("Özellikler")]
    [SerializeField] private float canPuanlari = 2;
    [SerializeField] private int paraDegeri = 50;

    private bool yokEdildi = false;
    private Color orijinalRenk; // Orijinal renk

    private void Start()
    {
        // Gerekli bileþenleri al
        dusmanAnimatoru = GetComponent<Animator>();
        dusmanHareketi = GetComponent<DusmanHareketi>();

        // SpriteRenderer varsa orijinal rengini al
        if (spriteRenderer != null)
        {
            orijinalRenk = spriteRenderer.color;
        }
    }

    public void HasarAl(float hasar)
    {
        // Can puanlarýný azalt
        canPuanlari -= hasar;

        // Kýrmýzýya dönme efektini baþlat
        StartCoroutine(KirmiziYanipSonme());

        // Can puaný sýfýrýn altýna düþerse
        if (canPuanlari <= 0f && !yokEdildi)
        {
            yokEdildi = true;

            // Düþman yok edildiðini bildir
            DusmanUreticisi.dusmanYokEdildi.Invoke();

            // Oyuncuya para ekle
            LevelYoneticisi.main.ParaArtir(paraDegeri);

            // Yok edilme animasyonunu baþlat
            dusmanAnimatoru.SetBool("Alive", false);

            // Hareketi durdur
            dusmanHareketi.HareketiDurdur();

            // Yok etme iþlemini baþlat
            //YokOl();
        }
    }

    private void YokOl()
    {
        // Yok edilme animasyon süresi kadar bekleyip objeyi yok et
        //Destroy(gameObject, 0.19f); // 0.19 saniye sonra yok et
        Destroy(gameObject); // 0.19 saniye sonra yok et
    }

    private IEnumerator KirmiziYanipSonme()
    {
        // Sprite'ý kýrmýzý yap
        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.red;
        }

        // Kýrmýzý olarak kalma süresi
        yield return new WaitForSeconds(0.2f);

        // Eski rengine geri dön
        if (spriteRenderer != null)
        {
            spriteRenderer.color = orijinalRenk;
        }
    }
}


