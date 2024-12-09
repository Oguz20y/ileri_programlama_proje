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
    [SerializeField] private float MermiSpeed = 5f;
    [SerializeField] private int MermiDamage = 1;
    [SerializeField] private float freezeTime = 1f;

    private Transform target;

    //private const float rotationOffset = -316f; // Ba�lang�� d�n���ne g�re ayarlama

    private void Start()
    {
        // Ana kameray� al
        mainCamera = Camera.main;
    }
    public void SetTarget(Transform _target)
    {
        target = _target;
    }

    private void FixedUpdate()
    {
        if (!target) return;

        // Hedefe do�ru hareket et
        Vector2 direction = (target.position - transform.position).normalized;

        // Rigidbody'yi hedefe do�ru hareket ettir
        rb.velocity = direction * MermiSpeed;

        /*
        // Hareket y�n�ne g�re oku d�nd�r (offset ekleniyor)
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
        // Merminin d�nya uzay�ndaki pozisyonunu al
        Vector3 MermiPosition = transform.position;

        // Merminin pozisyonunu ekran koordinatlar�na d�n��t�r
        Vector3 screenPosition = mainCamera.WorldToScreenPoint(MermiPosition);

        // Mermi ekran s�n�rlar�n�n d���na ��kt�ysa yok et
        if (screenPosition.x < 0 || screenPosition.x > Screen.width ||
            screenPosition.y < 0 || screenPosition.y > Screen.height)
        {
            Destroy(gameObject);
        }
    }

    private IEnumerator ResetEnemySpeed(DusmanHareketi em)
    {
        yield return new WaitForSeconds(freezeTime);
        em.HizSifirla();
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        DusmanHareketi em = other.gameObject.GetComponent<DusmanHareketi>();
        // D��mandan sa�l�k puan� ��kar
        other.gameObject.GetComponent<DusmanCani>().HasarAl(MermiDamage);
        em.HizGuncelle(0f);
        StartCoroutine(ResetEnemySpeed(em));
        // muhimmatAnimator.SetBool(muhimmatAnimationIsmi, true);
        Destroy(gameObject);
        
    }
}
