using UnityEngine;

public class Mermi : MonoBehaviour
{
    [Header("Referanslar")]
    [SerializeField] private Rigidbody2D rigidBody;

    private Camera anaKamera;

    [Header("�zellikler")]
    [SerializeField] private float mermiHizi = 5f;  // Merminin h�z�
    [SerializeField] private float mermiHasari = 1f;   // Merminin verdi�i hasar
    [SerializeField] private int hareketsizFrame = 120; // Hareket etmeyen mermiyi yok etme s�resi (frame say�s�)

    private Transform hedef;  // Merminin hedef ald��� d��man
    private int hareketsizlikSayaci = 0; // Hareket kontrol� i�in frame sayac�
    private Vector3 oncekiPozisyon;

    private const float donusOffset = -316f; // D�n�� a��s�na g�re d�zeltme

    private void Start()
    {
        // Ana kameray� al 
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

        // Hedefe do�ru hareket et
        Vector2 yon = (hedef.position - transform.position).normalized;

        // Rigidbody'yi hedefe do�ru hareket ettir
        rigidBody.velocity = yon * mermiHizi;

        // Hareket y�n�ne g�re mermiyi d�nd�r
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
        // Merminin d�nya uzay�ndaki pozisyonunu al
        Vector3 mermiPozisyonu = transform.position;

        // Merminin pozisyonunu ekran koordinatlar�na d�n��t�r
        Vector3 ekranPozisyonu = anaKamera.WorldToScreenPoint(mermiPozisyonu);

        // Ekran s�n�rlar�n�n d���na ��kt�ysa mermiyi yok et
        if (ekranPozisyonu.x < 0 || ekranPozisyonu.x > Screen.width ||
            ekranPozisyonu.y < 0 || ekranPozisyonu.y > Screen.height)
        {
            Destroy(gameObject);
        }
    }

    private void HareketKontrolu()
    {
        // Hareket kontrol� i�in pozisyon de�i�imini kontrol et
        if (Vector3.Distance(oncekiPozisyon, transform.position) < 0.01f)
        {
            hareketsizlikSayaci++; // Hareket yoksa frame sayac�n� art�r
            if (hareketsizlikSayaci >= hareketsizFrame)
            {
                Destroy(gameObject); // Hareket etmeyen mermiyi yok et
            }
        }
        else
        {
            hareketsizlikSayaci = 0; // Hareket varsa sayac� s�f�rla
        }

        oncekiPozisyon = transform.position; // Bir sonraki kontrol i�in pozisyonu g�ncelle
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        // Sa�l�k bile�eni var m� kontrol et
        DusmanCani dusmanSaglik = other.gameObject.GetComponent<DusmanCani>();
        if (dusmanSaglik != null)
        {
            // D��mana hasar ver
            dusmanSaglik.HasarAl(mermiHasari);
        }

        // Mermiyi yok et
        Destroy(gameObject);
    }
}
