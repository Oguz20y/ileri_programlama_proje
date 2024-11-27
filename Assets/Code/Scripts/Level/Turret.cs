using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Turret : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firingPoint;


    [Header("Attributes")]
    [SerializeField] private float targetingRange = 4f;
    [SerializeField] private float bulletsPerSecond = 1f; // bps

    private Transform target;
    private float timeUntilFire;

    private void Update() 
    {
        if(target == null) 
        {
            FindTarget();
            return;
        }
        if (!CheckTargetIsInRange())
        {
            target = null;
        }
        else
        {
            timeUntilFire += Time.deltaTime;
            if (timeUntilFire >= 1f/bulletsPerSecond)
            {
                Shoot();
                timeUntilFire = 0f;
            }
        }
    }

    private void Shoot() 
    {
        Debug.Log("Shoot");
        GameObject bulletObj = Instantiate(bulletPrefab, firingPoint.position, Quaternion.identity);
        Bullet bulletScript = bulletObj.GetComponent<Bullet>();
        bulletScript.SetTarget(target);
    }

    private bool CheckTargetIsInRange() 
    {
        return Vector2.Distance(target.position, transform.position) <= targetingRange;
    }

    private void FindTarget() 
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, targetingRange, (Vector2)transform.position, 0f, enemyMask);
        if (hits.Length > 0)
        {
            target = hits[0].transform;
        }
    }
    private void OnDrawGizmosSelected() 
    {
        Handles.color = Color.blue;
        Handles.DrawWireDisc(transform.position, transform.forward ,targetingRange);
    }
    
}
