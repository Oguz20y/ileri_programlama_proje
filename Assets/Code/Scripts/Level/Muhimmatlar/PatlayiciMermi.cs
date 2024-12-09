using UnityEngine;

public class PatlayiciMermi : MonoBehaviour
{
    [Header("Referanslar")]
    [SerializeField] private Animator mermiAnimatoru;
    [SerializeField] private AudioSource patlamaSFX;

    [Header("�zellikler")]
    [SerializeField] private float yukseklik = 2f;  // Maksimum y�kseklik
    [SerializeField] private float patlamaYaricapi = 2f; // Patlaman�n etkili yar��ap�
    [SerializeField] private int mermiHasari = 1;        // Merminin verdi�i hasar

    private Vector2 hedefPozisyon;
    private Vector2 baslangicPozisyon;
    private float yolculukSuresi;  // Hedefe ula�mak i�in gereken s�re
    private float gecenSure;
    private bool hasarVerildi = false; // Hasar�n sadece bir kez verilmesini kontrol eder

    public void HedefBelirle(Vector2 hedef)
    {
        hedefPozisyon = hedef;
        baslangicPozisyon = transform.position;

        // Mesafeyi hesapla
        float mesafe = Vector2.Distance(baslangicPozisyon, hedefPozisyon);

        // Uygun s�reyi hesapla (�rne�in, belirli bir h�z yerine mesafeye g�re ayarlama)
        yolculukSuresi = Mathf.Sqrt(2 * yukseklik / Mathf.Abs(Physics2D.gravity.y)) * 2;

        // E�er y�kseklik kullanm�yorsak veya yer�ekimi s�f�rsa, s�reyi mesafeye g�re belirle
        if (Physics2D.gravity.y == 0)
        {
            yolculukSuresi = Mathf.Clamp(mesafe / 10f, 0.5f, 5f); // S�reyi mant�kl� bir aral�kta tut
        }

        gecenSure = 0f; // Hareketi ba�latmak i�in s�reyi s�f�rla
    }

    private void Update()
    {
        if (gecenSure >= yolculukSuresi)
        {
            // Hedefe ula�t���nda patlama etkisi olu�tur
            PatlamaEtkiAlan�();
            return;
        }

        // Hareketi g�ncelle
        gecenSure += Time.deltaTime;
        float orantiliZaman = gecenSure / yolculukSuresi;

        // Yeni pozisyonu hesapla
        Vector2 yeniPozisyon = Vector2.Lerp(baslangicPozisyon, hedefPozisyon, orantiliZaman);

        // Y�kseklik efekti ekle (parabolik bir y�r�nge i�in)
        float yukseklikEklentisi = yukseklik * Mathf.Sin(orantiliZaman * Mathf.PI); // Sin�s, parabol olu�turur
        yeniPozisyon.y += yukseklikEklentisi;

        // Yeni pozisyonu uygula
        transform.position = yeniPozisyon;

        // D�n�� a��s�n� ayarla
        Vector2 yon = (yeniPozisyon - (Vector2)transform.position).normalized;
        float angle = Mathf.Atan2(yon.y, yon.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void PatlamaEtkiAlan�()
    {
        if (hasarVerildi) return; // Hasar�n sadece bir kez verilmesini sa�la
        hasarVerildi = true;

        // Patlama yar��ap� i�inde bulunan nesneleri bul
        Collider2D[] carpilanlar = Physics2D.OverlapCircleAll(transform.position, patlamaYaricapi);

        foreach (Collider2D carpilan in carpilanlar)
        {
            // Sa�l�k bile�eni var m� kontrol et
            DusmanCani dusmanSaglik = carpilan.GetComponent<DusmanCani>();
            if (dusmanSaglik != null)
            {
                // D��mana hasar ver
                dusmanSaglik.HasarAl(mermiHasari);
            }
        }

        // Patlama animasyonu oynat
        if (mermiAnimatoru != null)
        {
            mermiAnimatoru.SetBool("Patlama", true);
        }

        // Mermiyi yok et
        //Destroy(gameObject); // Animasyonun oynayabilmesi i�in yok etmeden �nce biraz bekle
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // �arp��ma ger�ekle�ti�inde patlama olu�tur
        PatlamaEtkiAlan�();
    }

    private void OnDrawGizmosSelected()
    {
        // Patlama yar��ap�n� g�rselle�tir
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, patlamaYaricapi);
    }

    private void mermiYokEt() 
    {
        Destroy(gameObject);
    }

    public void SesiOynat()
    {
        // Ses oynat
        if (patlamaSFX != null)
        {
            patlamaSFX.Play();
        }
        else
        {
            Debug.LogError("patlamaSFX bile�eni bulunamad�!");
        }
    }

}
