using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public float spawnRangeX = 10;
    private float spawnZMin = 15; // set min spawn Z
    private float spawnZMax = 25;
    private float startDelay = 10;
    private float spawnInterval = 20;

    public GameObject asteroidPrefab;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating(nameof(SpawnAsteroid), startDelay, spawnInterval);
    }
    Vector3 GenerateSpawnPosition()
    {
        float xPos = Random.Range(-spawnRangeX, spawnRangeX);
        float zPos = Random.Range(spawnZMin, spawnZMax);
        return new Vector3(xPos, 0, zPos);
    }

    // Update is called once per frame
    void Update()
    {
       
    }
    void SpawnAsteroid()
    {
        //Vector3 spawnPos = new Vector3(Random.Range(-spawnRangeX, spawnRangeX), 0, spawnPosZ);
        Instantiate(asteroidPrefab, GenerateSpawnPosition(), asteroidPrefab.transform.rotation);
       // Instantiate(asteroidPrefab, spawnPos, asteroidPrefab.transform.rotation);
    }
}
