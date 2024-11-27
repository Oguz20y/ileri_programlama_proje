using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    private EnemyMovement enemyMovement;

    [Header("References")]
    [SerializeField] private Animator enemyAnimator;

    [Header("Attributes")]
    [SerializeField] private int hitPoints = 2;
    [SerializeField] private int currencyWorth = 50;

    private bool isDestroyed = false;

    private void Start()
    {
        enemyAnimator = GetComponent<Animator>();
        enemyMovement = GetComponent<EnemyMovement>();
    }

    public void Die()
    {
        enemyMovement.StopMovement();
        enemyAnimator.SetBool("Alive", false);
        float animationDuration = enemyAnimator.GetCurrentAnimatorStateInfo(0).length;
        StartCoroutine(DestroyAfterAnimation(animationDuration));
    }
    private IEnumerator DestroyAfterAnimation(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
    public void TakeDamage(int dmg)
    {
        hitPoints -= dmg;
        if (hitPoints <= 0 && !isDestroyed)
        {
            EnemySpawner.onEnemyDestroy.Invoke();
            LevelManager.main.IncreaseCurrency(currencyWorth);
            isDestroyed = true;
            Die();
        }
    }
}
