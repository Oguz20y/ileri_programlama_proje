using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemySpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject[] enemyPrefabs;


    [Header("Attributes")]
    [SerializeField] private int baseEnemies = 8; // �lk ba�ta spawn olacak d��man say�s�
    [SerializeField] private float enemiesPerSecond = 0.5f; // D��manlar�n spawn olma s�resi aras�ndaki fark (Artt�kd�k�a spawn h�z� artar bekleme s�resi azal�r)
    [SerializeField] private float timeBetweenWaves = 5f; // Dalgalar aras�ndaki bekleme s�resi
    [SerializeField] private float difficultScalingFactor = 0.75f; // Zorluk ayar�

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
        if (!isSpawning) return; // E�er spawn olmas� false ise hi�bir �ey d�nd�rme.

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
        return Mathf.RoundToInt(baseEnemies * Mathf.Pow(currentWave, difficultScalingFactor)); // Wave say�s� artt�k�a spawn olacak d��man say�s� artar.
                                                                                               // Zorluk ayar�na g�re art�� h�z� belirlenir.
                                                                                               // �uan zorluk ayar� = 0.75f;
    }
}
