using System.Collections;
using UnityEngine;

public class DonduranMermi : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody2D rb;

    private Camera mainCamera;

    /*
    [SerializeField] private string muhimmatAnimationIsmi;
    [SerializeField] Animator muhimmatAnimator;
    */

    [Header("Attributes")]
    [SerializeField] private float bulletSpeed = 5f;
    [SerializeField] private int bulletDamage = 1;
    [SerializeField] private float freezeTime = 1f;

    private Transform target;

    //private const float rotationOffset = -316f; // Baþlangýç dönüþüne göre ayarlama

    private void Start()
    {
        // Ana kamerayý al
        mainCamera = Camera.main;
    }
    public void SetTarget(Transform _target)
    {
        target = _target;
    }

    private void FixedUpdate()
    {
        if (!target) return;

        // Hedefe doðru hareket et
        Vector2 direction = (target.position - transform.position).normalized;

        // Rigidbody'yi hedefe doðru hareket ettir
        rb.velocity = direction * bulletSpeed;

        /*
        // Hareket yönüne göre oku döndür (offset ekleniyor)
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle + rotationOffset - 180);
        */

    }
    private void Update()
    {
        SinirlariBelirle();
    }

    private void SinirlariBelirle()
    {
        // Merminin dünya uzayýndaki pozisyonunu al
        Vector3 bulletPosition = transform.position;

        // Merminin pozisyonunu ekran koordinatlarýna dönüþtür
        Vector3 screenPosition = mainCamera.WorldToScreenPoint(bulletPosition);

        // Mermi ekran sýnýrlarýnýn dýþýna çýktýysa yok et
        if (screenPosition.x < 0 || screenPosition.x > Screen.width ||
            screenPosition.y < 0 || screenPosition.y > Screen.height)
        {
            Destroy(gameObject);
        }
    }

    private IEnumerator ResetEnemySpeed(EnemyMovement em)
    {
        yield return new WaitForSeconds(freezeTime);
        em.HizSifirla();
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        EnemyMovement em = other.gameObject.GetComponent<EnemyMovement>();
        // Düþmandan saðlýk puaný çýkar
        other.gameObject.GetComponent<Health>().HasarAl(bulletDamage);
        em.HizGuncelle(0f);
        StartCoroutine(ResetEnemySpeed(em));
        // muhimmatAnimator.SetBool(muhimmatAnimationIsmi, true);
        Destroy(gameObject);
        
    }
}
