using UnityEngine;

public class DusmanHareketi : MonoBehaviour
{
    [Header("Referanslar")]
    [SerializeField] private Rigidbody2D rigidBody;
    [SerializeField] private Animator dusmanAnimatoru;

    [Header("Özellikler")]
    [SerializeField] private float hareketHizi = 2f;

    private Transform[] mevcutYol; // Bu düþmana ait yol
    private int yolIndex = 0;

    private float temelHiz;
    private SpriteRenderer spriteRenderer;
    private Color orijinalRenk;

    private float siraGuncellemeZamani = 0.2f; // Sýralama güncelleme aralýðý
    private float sonGuncellemeZamani = 0f;

    private void Start()
    {
        // Rastgele bir koridor seç ve bu düþmana ait yolu ayarla
        int koridorIndex = Random.Range(0, LevelYoneticisi.main.koridorlar.Length);
        mevcutYol = LevelYoneticisi.main.koridorlar[koridorIndex].yolNoktalari;

        // Düþman seçilen koridorun baþlangýç noktasýna taþýnýr
        transform.position = mevcutYol[0].position;

        // Baþlangýç ayarlarý
        temelHiz = hareketHizi;
        spriteRenderer = GetComponent<SpriteRenderer>();
        orijinalRenk = spriteRenderer.color;

        dusmanAnimatoru = GetComponent<Animator>();

        // Baþlangýçta sýralama güncelle
        SiraGuncelle();
    }

    private void Update()
    {
        // Eðer hedef noktaya ulaþýldýysa bir sonraki hedefe geç
        if (Vector2.Distance(mevcutYol[yolIndex].position, transform.position) <= 0.1f)
        {
            yolIndex++;
            if (yolIndex == mevcutYol.Length)
            {
                // Yolun sonuna ulaþýldýðýnda oyuncuya hasar ver ve düþmaný yok et
                LevelYoneticisi.main.suankiOyuncuCani -= LevelYoneticisi.main.dusmanHasar;
                DusmanUreticisi.dusmanYokEdildi.Invoke();
                Destroy(gameObject);
                return;
            }
        }

        // Belirli aralýklarla sýralama güncelle
        if (Time.time - sonGuncellemeZamani >= siraGuncellemeZamani)
        {
            SiraGuncelle();
            sonGuncellemeZamani = Time.time;
        }
    }

    private void FixedUpdate()
    {
        // Düþmaný hedefe doðru hareket ettir
        Vector2 yon = (mevcutYol[yolIndex].position - transform.position).normalized;
        rigidBody.velocity = yon * hareketHizi;
    }

    private void SiraGuncelle()
    {
        // Y pozisyonuna göre sýralama düzenle
        spriteRenderer.sortingOrder = Mathf.RoundToInt(-transform.position.y * 100);
    }

    public void HizGuncelle(float yeniHiz)
    {
        hareketHizi = yeniHiz;
    }

    public void HizSifirla()
    {
        hareketHizi = temelHiz;
    }

    public void RenkAyarla(Color yeniRenk)
    {
        spriteRenderer.color = yeniRenk;
    }

    public void RenkSifirla()
    {
        spriteRenderer.color = orijinalRenk;
    }

    public void HareketiDurdur()
    {
        hareketHizi = 0f;
    }
}
