using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header("Referanslar")]
    [SerializeField] private Rigidbody2D rigidBody;
    [SerializeField] private Animator dusmanAnimatoru;

    [Header("�zellikler")]
    [SerializeField] private float hareketHizi = 2f;

    private Transform[] mevcutYol; // Bu d��mana ait yol
    private int yolIndex = 0;

    private float temelHiz;
    private SpriteRenderer spriteRenderer;
    private Color orijinalRenk;

    private void Start()
    {
        // Rastgele bir koridor se� ve bu d��mana ait yolu ayarla
        int koridorIndex = Random.Range(0, LevelManager.main.koridorlar.Length);
        mevcutYol = LevelManager.main.koridorlar[koridorIndex].yolNoktalari;

        // D��man se�ilen koridorun ba�lang�� noktas�na ta��n�r
        transform.position = mevcutYol[0].position;

        // Ba�lang�� ayarlar�
        temelHiz = hareketHizi;
        spriteRenderer = GetComponent<SpriteRenderer>();
        orijinalRenk = spriteRenderer.color;

        dusmanAnimatoru = GetComponent<Animator>();
    }

    private void Update()
    {
        // E�er hedef noktaya ula��ld�ysa bir sonraki hedefe ge�
        if (Vector2.Distance(mevcutYol[yolIndex].position, transform.position) <= 0.1f)
        {
            yolIndex++;
            if (yolIndex == mevcutYol.Length)
            {
                // Yolun sonuna ula��ld���nda oyuncuya hasar ver ve d��man� yok et
                LevelManager.main.suankiOyuncuCani -= LevelManager.main.dusmanHasar;
                EnemySpawner.dusmanYokEdildi.Invoke();
                Destroy(gameObject);
                return;
            }
        }
    }

    private void FixedUpdate()
    {
        // D��man� hedefe do�ru hareket ettir
        Vector2 yon = (mevcutYol[yolIndex].position - transform.position).normalized;
        rigidBody.velocity = yon * hareketHizi;
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
