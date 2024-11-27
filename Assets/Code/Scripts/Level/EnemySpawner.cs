using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemySpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject[] enemyPrefabs;


    [Header("Attributes")]
    [SerializeField] private int baseEnemies = 8; // Ýlk baþta spawn olacak düþman sayýsý
    [SerializeField] private float enemiesPerSecond = 0.5f; // Düþmanlarýn spawn olma süresi arasýndaki fark (Arttýkdýkça spawn hýzý artar bekleme süresi azalýr)
    [SerializeField] private float timeBetweenWaves = 5f; // Dalgalar arasýndaki bekleme süresi
    [SerializeField] private float difficultScalingFactor = 0.75f; // Zorluk ayarý

    [Header("Events")]
    public static UnityEvent onEnemyDestroy = new UnityEvent();

    private int currentWave = 1;
    private float timeSinceLastSpawn;
    private int enemiesAlive;
    private int enemiesLeftToSpawn;
    private bool isSpawning = false;

    private void Awake()
    {
        onEnemyDestroy.AddListener(EnemyDestroyed);
    }

    private void Start()
    {
        StartCoroutine(StartWave());
    }
    private void Update()
    {
        if (!isSpawning) return; // Eðer spawn olmasý false ise hiçbir þey döndürme.

        timeSinceLastSpawn += Time.deltaTime;
         
        if (timeSinceLastSpawn >= (1f / enemiesPerSecond) && enemiesLeftToSpawn > 0)
        {
            SpawnEnemy();
            enemiesLeftToSpawn--;
            enemiesAlive++;
            timeSinceLastSpawn = 0f;
        }

        if (enemiesAlive == 0 && enemiesLeftToSpawn == 0)
        {
            EndWave();
        }
    }
    private IEnumerator StartWave()
    {
        yield return new WaitForSeconds(timeBetweenWaves);
        isSpawning = true;
        enemiesLeftToSpawn = EnemiesPerWave();
    }
    private void EndWave() 
    {
        isSpawning = false;
        timeSinceLastSpawn = 0f;
        currentWave++;
        StartCoroutine(StartWave());
    }
    private void SpawnEnemy()
    {
        //Debug.Log("Spawn Enemy");
        GameObject prefabToSpawn = enemyPrefabs[0];
        Instantiate(prefabToSpawn, LevelManager.main.startPoint.position, Quaternion.identity);
    }
    private void EnemyDestroyed() 
    {
        enemiesAlive--;
    }

    private int EnemiesPerWave() 
    {
        return Mathf.RoundToInt(baseEnemies * Mathf.Pow(currentWave, difficultScalingFactor)); // Wave sayýsý arttýkça spawn olacak düþman sayýsý artar.
                                                                                               // Zorluk ayarýna göre artýþ hýzý belirlenir.
                                                                                               // Þuan zorluk ayarý = 0.75f;
    }
}
