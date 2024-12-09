using UnityEngine;

public class PatlayiciMermi : MonoBehaviour
{
    [Header("Referanslar")]
    [SerializeField] private Animator mermiAnimatoru;
    [SerializeField] private AudioSource patlamaSFX;

    [Header("Özellikler")]
    [SerializeField] private float yukseklik = 2f;  // Maksimum yükseklik
    [SerializeField] private float patlamaYaricapi = 2f; // Patlamanýn etkili yarýçapý
    [SerializeField] private int mermiHasari = 1;        // Merminin verdiði hasar

    private Vector2 hedefPozisyon;
    private Vector2 baslangicPozisyon;
    private float yolculukSuresi;  // Hedefe ulaþmak için gereken süre
    private float gecenSure;
    private bool hasarVerildi = false; // Hasarýn sadece bir kez verilmesini kontrol eder

    public void HedefBelirle(Vector2 hedef)
    {
        hedefPozisyon = hedef;
        baslangicPozisyon = transform.position;

        // Mesafeyi hesapla
        float mesafe = Vector2.Distance(baslangicPozisyon, hedefPozisyon);

        // Uygun süreyi hesapla (örneðin, belirli bir hýz yerine mesafeye göre ayarlama)
        yolculukSuresi = Mathf.Sqrt(2 * yukseklik / Mathf.Abs(Physics2D.gravity.y)) * 2;

        // Eðer yükseklik kullanmýyorsak veya yerçekimi sýfýrsa, süreyi mesafeye göre belirle
        if (Physics2D.gravity.y == 0)
        {
            yolculukSuresi = Mathf.Clamp(mesafe / 10f, 0.5f, 5f); // Süreyi mantýklý bir aralýkta tut
        }

        gecenSure = 0f; // Hareketi baþlatmak için süreyi sýfýrla
    }

    private void Update()
    {
        if (gecenSure >= yolculukSuresi)
        {
            // Hedefe ulaþtýðýnda patlama etkisi oluþtur
            PatlamaEtkiAlaný();
            return;
        }

        // Hareketi güncelle
        gecenSure += Time.deltaTime;
        float orantiliZaman = gecenSure / yolculukSuresi;

        // Yeni pozisyonu hesapla
        Vector2 yeniPozisyon = Vector2.Lerp(baslangicPozisyon, hedefPozisyon, orantiliZaman);

        // Yükseklik efekti ekle (parabolik bir yörünge için)
        float yukseklikEklentisi = yukseklik * Mathf.Sin(orantiliZaman * Mathf.PI); // Sinüs, parabol oluþturur
        yeniPozisyon.y += yukseklikEklentisi;

        // Yeni pozisyonu uygula
        transform.position = yeniPozisyon;

        // Dönüþ açýsýný ayarla
        Vector2 yon = (yeniPozisyon - (Vector2)transform.position).normalized;
        float angle = Mathf.Atan2(yon.y, yon.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void PatlamaEtkiAlaný()
    {
        if (hasarVerildi) return; // Hasarýn sadece bir kez verilmesini saðla
        hasarVerildi = true;

        // Patlama yarýçapý içinde bulunan nesneleri bul
        Collider2D[] carpilanlar = Physics2D.OverlapCircleAll(transform.position, patlamaYaricapi);

        foreach (Collider2D carpilan in carpilanlar)
        {
            // Saðlýk bileþeni var mý kontrol et
            DusmanCani dusmanSaglik = carpilan.GetComponent<DusmanCani>();
            if (dusmanSaglik != null)
            {
                // Düþmana hasar ver
                dusmanSaglik.HasarAl(mermiHasari);
            }
        }

        // Patlama animasyonu oynat
        if (mermiAnimatoru != null)
        {
            mermiAnimatoru.SetBool("Patlama", true);
        }

        // Mermiyi yok et
        //Destroy(gameObject); // Animasyonun oynayabilmesi için yok etmeden önce biraz bekle
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Çarpýþma gerçekleþtiðinde patlama oluþtur
        PatlamaEtkiAlaný();
    }

    private void OnDrawGizmosSelected()
    {
        // Patlama yarýçapýný görselleþtir
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
            Debug.LogError("patlamaSFX bileþeni bulunamadý!");
        }
    }

}
