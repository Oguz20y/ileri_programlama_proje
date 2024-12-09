using UnityEngine;

public class Mermi : MonoBehaviour
{
    [Header("Referanslar")]
    [SerializeField] private Rigidbody2D rigidBody;

    private Camera anaKamera;

    [Header("Özellikler")]
    [SerializeField] private float mermiHizi = 5f;  // Merminin hýzý
    [SerializeField] private float mermiHasari = 1f;   // Merminin verdiði hasar
    [SerializeField] private int hareketsizFrame = 120; // Hareket etmeyen mermiyi yok etme süresi (frame sayýsý)

    private Transform hedef;  // Merminin hedef aldýðý düþman
    private int hareketsizlikSayaci = 0; // Hareket kontrolü için frame sayacý
    private Vector3 oncekiPozisyon;

    private const float donusOffset = -316f; // Dönüþ açýsýna göre düzeltme

    private void Start()
    {
        // Ana kamerayý al 
        anaKamera = Camera.main;
        oncekiPozisyon = transform.position;
    }

    public void HedefBelirle(Transform yeniHedef)
    {
        hedef = yeniHedef;
    }

    private void FixedUpdate()
    {
        if (!hedef) return;

        // Hedefe doðru hareket et
        Vector2 yon = (hedef.position - transform.position).normalized;

        // Rigidbody'yi hedefe doðru hareket ettir
        rigidBody.velocity = yon * mermiHizi;

        // Hareket yönüne göre mermiyi döndür
        float aci = Mathf.Atan2(yon.y, yon.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, aci + donusOffset - 180);
    }

    private void Update()
    {
        SinirKontrolu();
        HareketKontrolu();
    }

    private void SinirKontrolu()
    {
        // Merminin dünya uzayýndaki pozisyonunu al
        Vector3 mermiPozisyonu = transform.position;

        // Merminin pozisyonunu ekran koordinatlarýna dönüþtür
        Vector3 ekranPozisyonu = anaKamera.WorldToScreenPoint(mermiPozisyonu);

        // Ekran sýnýrlarýnýn dýþýna çýktýysa mermiyi yok et
        if (ekranPozisyonu.x < 0 || ekranPozisyonu.x > Screen.width ||
            ekranPozisyonu.y < 0 || ekranPozisyonu.y > Screen.height)
        {
            Destroy(gameObject);
        }
    }

    private void HareketKontrolu()
    {
        // Hareket kontrolü için pozisyon deðiþimini kontrol et
        if (Vector3.Distance(oncekiPozisyon, transform.position) < 0.01f)
        {
            hareketsizlikSayaci++; // Hareket yoksa frame sayacýný artýr
            if (hareketsizlikSayaci >= hareketsizFrame)
            {
                Destroy(gameObject); // Hareket etmeyen mermiyi yok et
            }
        }
        else
        {
            hareketsizlikSayaci = 0; // Hareket varsa sayacý sýfýrla
        }

        oncekiPozisyon = transform.position; // Bir sonraki kontrol için pozisyonu güncelle
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        // Saðlýk bileþeni var mý kontrol et
        DusmanCani dusmanSaglik = other.gameObject.GetComponent<DusmanCani>();
        if (dusmanSaglik != null)
        {
            // Düþmana hasar ver
            dusmanSaglik.HasarAl(mermiHasari);
        }

        // Mermiyi yok et
        Destroy(gameObject);
    }
}
