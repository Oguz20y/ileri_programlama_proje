using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Referanslar")]
    [SerializeField] private Rigidbody2D rigidBody;

    private Camera anaKamera;

    [Header("Özellikler")]
    [SerializeField] private float mermiHizi = 5f;  // Merminin hýzý
    [SerializeField] private int mermiHasari = 1;   // Merminin verdiði hasar

    private Transform hedef;  // Merminin hedef aldýðý düþman

    private const float donusOffset = -316f; // Dönüþ açýsýna göre düzeltme

    private void Start()
    {
        // Ana kamerayý al
        anaKamera = Camera.main;
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

    private void OnCollisionEnter2D(Collision2D other)
    {
        // Saðlýk bileþeni var mý kontrol et
        Health dusmanSaglik = other.gameObject.GetComponent<Health>();
        if (dusmanSaglik != null)
        {
            // Düþmana hasar ver
            dusmanSaglik.HasarAl(mermiHasari);
        }

        // Mermiyi yok et
        Destroy(gameObject);
    }

}
