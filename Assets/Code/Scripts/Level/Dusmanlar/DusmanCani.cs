using System.Collections;
using UnityEngine;

public class DusmanCani: MonoBehaviour
{
    private DusmanHareketi dusmanHareketi;

    [Header("Referanslar")]
    [SerializeField] private Animator dusmanAnimatoru;
    [SerializeField] private SpriteRenderer spriteRenderer; // SpriteRenderer referans�

    [Header("�zellikler")]
    [SerializeField] private float canPuanlari = 2;
    [SerializeField] private int paraDegeri = 50;

    private bool yokEdildi = false;
    private Color orijinalRenk; // Orijinal renk

    private void Start()
    {
        // Gerekli bile�enleri al
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
        // Can puanlar�n� azalt
        canPuanlari -= hasar;

        // K�rm�z�ya d�nme efektini ba�lat
        StartCoroutine(KirmiziYanipSonme());

        // Can puan� s�f�r�n alt�na d��erse
        if (canPuanlari <= 0f && !yokEdildi)
        {
            yokEdildi = true;

            // D��man yok edildi�ini bildir
            DusmanUreticisi.dusmanYokEdildi.Invoke();

            // Oyuncuya para ekle
            LevelYoneticisi.main.ParaArtir(paraDegeri);

            // Yok edilme animasyonunu ba�lat
            dusmanAnimatoru.SetBool("Alive", false);

            // Hareketi durdur
            dusmanHareketi.HareketiDurdur();

            // Yok etme i�lemini ba�lat
            //YokOl();
        }
    }

    private void YokOl()
    {
        // Yok edilme animasyon s�resi kadar bekleyip objeyi yok et
        //Destroy(gameObject, 0.19f); // 0.19 saniye sonra yok et
        Destroy(gameObject); // 0.19 saniye sonra yok et
    }

    private IEnumerator KirmiziYanipSonme()
    {
        // Sprite'� k�rm�z� yap
        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.red;
        }

        // K�rm�z� olarak kalma s�resi
        yield return new WaitForSeconds(0.2f);

        // Eski rengine geri d�n
        if (spriteRenderer != null)
        {
            spriteRenderer.color = orijinalRenk;
        }
    }
}


