using System.Collections;
using UnityEngine;

public class Health : MonoBehaviour
{
    private EnemyMovement dusmanHareketi;

    [Header("Referanslar")]
    [SerializeField] private Animator dusmanAnimatoru;
    [SerializeField] private SpriteRenderer spriteRenderer; // SpriteRenderer referans�

    [Header("�zellikler")]
    [SerializeField] private int canPuanlari = 2;
    [SerializeField] private int paraDegeri = 50;

    private bool yokEdildi = false;
    private Color orijinalRenk; // Orijinal renk

    private void Start()
    {
        // Gerekli bile�enleri al
        dusmanAnimatoru = GetComponent<Animator>();
        dusmanHareketi = GetComponent<EnemyMovement>();

        // SpriteRenderer varsa orijinal rengini al
        if (spriteRenderer != null)
        {
            orijinalRenk = spriteRenderer.color;
        }
    }

    public void HasarAl(int hasar)
    {
        // Can puanlar�n� azalt
        canPuanlari -= hasar;

        // K�rm�z�ya d�nme efektini ba�lat
        StartCoroutine(KirmiziYanipSonme());

        // Can puan� s�f�r�n alt�na d��erse
        if (canPuanlari <= 0 && !yokEdildi)
        {
            yokEdildi = true;

            // D��man yok edildi�ini bildir
            EnemySpawner.dusmanYokEdildi.Invoke();

            // Oyuncuya para ekle
            LevelManager.main.ParaArtir(paraDegeri);

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
        yield return new WaitForSeconds(0.1f);

        // Eski rengine geri d�n
        if (spriteRenderer != null)
        {
            spriteRenderer.color = orijinalRenk;
        }
    }
}


