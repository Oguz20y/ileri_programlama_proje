using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody2D rb;

    /*
    [SerializeField] private string muhimmatAnimationIsmi;
    [SerializeField] Animator muhimmatAnimator;
    */

    [Header("Attributes")]
    [SerializeField] private float bulletSpeed = 5f;
    [SerializeField] private int bulletDamage = 1;

    private Transform target;

    public void SetTarget(Transform _target) 
    {
        target = _target;
    }

    private void FixedUpdate()
    {
        if (!target) return;

        Vector2 direction = (target.position - transform.position).normalized;

        // Merminin yönünü hedefe doðru çevir
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));

        rb.velocity = direction * bulletSpeed;
    }

    
    private void OnCollisionEnter2D(Collision2D other)
    {
        // Düþmandan saðlýk puaný çýkar
        other.gameObject.GetComponent<Health>().TakeDamage(bulletDamage);

        // muhimmatAnimator.SetBool(muhimmatAnimationIsmi, true);
        Destroy(gameObject);
    }
    

}
