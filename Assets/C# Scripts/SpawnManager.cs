using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] enemyPrefabs;
    public GameObject[] powerupPrefabs;
    private TextMeshProUGUI waveText;
    
    private float spawnRange = 9.0f;
    public bool isGameActive;
    public int enemyCount;
    public int waveNumber = 1;

    // Spawns waves of enemy spheres
    void SpawnEnemyWave(int enemiesToSpawn)
    {
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            int randomEnemy = Random.Range(0, enemyPrefabs.Length);
            Instantiate(enemyPrefabs[randomEnemy], GenerateSpawnPosition(), enemyPrefabs[randomEnemy].transform.rotation);
        }
    }

    // Generates random spawn positions for the enemy/powerups
    private Vector3 GenerateSpawnPosition() 
    {
        float spawnPisnX = Random.Range(-spawnRange, spawnRange);
        float spawnPisnY = Random.Range(-spawnRange, spawnRange);
        Vector3 randomPosition = new Vector3(spawnPisnX, 0, spawnPisnY);

        return randomPosition;
    }

    // Start is called before the first frame update
    void Start()
    {
        isGameActive = true;
        int randomPowerup = Random.Range(0, powerupPrefabs.Length);
        Instantiate(powerupPrefabs[randomPowerup], GenerateSpawnPosition(), powerupPrefabs[randomPowerup].transform.rotation);
        SpawnEnemyWave(waveNumber);
    }

    // Update is called once per frame
    void Update()
    {
        if (isGameActive) {
            enemyCount = FindObjectsOfType<EnemyController>().Length;
            if (enemyCount == 0)
            {
                waveNumber++;
                Debug.Log("This is wave number " + waveNumber);
                SpawnEnemyWave(waveNumber); 
                int randomPowerup = Random.Range(0, powerupPrefabs.Length);
                Instantiate(powerupPrefabs[randomPowerup], GenerateSpawnPosition(), powerupPrefabs[randomPowerup].transform.rotation);
            }
        }
    }
}