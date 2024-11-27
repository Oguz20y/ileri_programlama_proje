using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator enemyAnimator;

    [Header("Attributes")]
    [SerializeField] private float moveSpeed = 2f;

    private Transform target;
    private int pathIndex = 0;

    // Start is called before the first frame update
    private void Start()
    {
        target = LevelManager.main.path[pathIndex];
        enemyAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (Vector2.Distance(target.position, transform.position) <= 0.1f)
        {
            pathIndex++;
            if (pathIndex == LevelManager.main.path.Length)
            {
                EnemySpawner.onEnemyDestroy.Invoke();
                Destroy(gameObject);
                return;
            }
            else
            {
                target = LevelManager.main.path[pathIndex];
            }
        }   
    }

    public void StopMovement()
    {
        moveSpeed = 0f;
    }

    private void FixedUpdate()
    {
        Vector2 direction = (target.position - transform.position).normalized; // �ki pozisyon aras�ndaki mesafeyi 0-1 aras� bir say� yapar.

        rb.velocity = direction * moveSpeed;
    }
}
