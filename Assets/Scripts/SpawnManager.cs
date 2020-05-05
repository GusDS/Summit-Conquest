using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject enemySmallPrefab;
    public float enemySmallSpawn = 3f;

    public GameObject enemyBigPrefab;
    public float enemyBigSpawn = 15f;

    public GameObject powerupPushPrefab;
    public float powerupPushMinSpawn = 10f;
    public float powerupPushMaxSpawn = 20f;

    public GameObject powerupExplodePrefab;
    public float powerupExplodeMinSpawn = 15f;
    public float powerupExplodeMaxSpawn = 30f;

    private Control control;
    private GameObject enemiesGroup;
    private float spawnRange = 9.0f;

    void Start()
    {
        control = GameObject.Find("Control").GetComponent<Control>();
        enemiesGroup = GameObject.Find("EnemiesGroup");

        StartCoroutine(SpawnRoutine(powerupPushPrefab, powerupPushMinSpawn, powerupPushMaxSpawn));
        StartCoroutine(SpawnRoutine(powerupExplodePrefab, powerupExplodeMinSpawn, powerupExplodeMaxSpawn));
        StartCoroutine(SpawnRoutine(enemySmallPrefab, enemySmallSpawn, enemySmallSpawn));
        StartCoroutine(SpawnRoutine(enemyBigPrefab, enemyBigSpawn, enemyBigSpawn));
    }

    private void Update()
    {
//        if (Input.GetKeyDown(KeyCode.Q)) SpawnSmallEnemy();
    }
    Vector3 GenerateRandomPos()
    {
        float spawnPosX = Random.Range(-spawnRange, spawnRange);
        float spawnPosZ = Random.Range(-spawnRange, spawnRange);
        Vector3 randomPos = new Vector3(spawnPosX, 0, spawnPosZ);
        return randomPos;
    }

    IEnumerator SpawnRoutine(GameObject spawnPrefab, float minTime, float maxTime)
    {
        while (true)
        {
            if (control.isActionOn) {
                float spawnTime = Random.Range(minTime, maxTime);
                yield return new WaitForSeconds(spawnTime);
                GameObject newSpawn = Instantiate(spawnPrefab, GenerateRandomPos(), spawnPrefab.transform.rotation);

                if (spawnPrefab.gameObject.CompareTag("Enemy"))
                {
                    newSpawn.transform.position += Vector3.up * 10;
                    newSpawn.transform.SetParent(enemiesGroup.transform);
                    newSpawn.GetComponent<Rigidbody>().AddForce(Vector3.down * 50, ForceMode.VelocityChange);
                }
            } else {
                yield return new WaitForSeconds(1f);
            }
        }
    }

    void SpawnSmallEnemy()
    {
        if (control.isActionOn)
        {
            GameObject newEnemy = Instantiate(enemySmallPrefab, GenerateRandomPos() + (Vector3.up * 10), enemySmallPrefab.transform.rotation);
            newEnemy.transform.SetParent(enemiesGroup.transform);
            newEnemy.GetComponent<Rigidbody>().AddForce(Vector3.down * 50, ForceMode.VelocityChange);
        }
    }

    /* Keep those for individual debug:

    IEnumerator SpawnEnemy(GameObject enemyPrefab, float spawnTime)
    {
        while (control.isActionOn)
        {
            yield return new WaitForSeconds(spawnTime);
            GameObject newEnemy = Instantiate(enemyPrefab, GenerateRandomPos() + (Vector3.up * 10), enemyPrefab.transform.rotation);
            newEnemy.transform.SetParent(enemiesGroup.transform);
            newEnemy.GetComponent<Rigidbody>().AddForce(Vector3.down * 50, ForceMode.VelocityChange);
        }
    }

    void SpawnSmallEnemy()
    {
        if (control.isActionOn) {
            GameObject newEnemy = Instantiate(enemySmallPrefab, GenerateRandomPos() + (Vector3.up * 10), enemySmallPrefab.transform.rotation);
            newEnemy.transform.SetParent(enemiesGroup.transform);
            newEnemy.GetComponent<Rigidbody>().AddForce(Vector3.down * 50, ForceMode.VelocityChange);
        }
    }
    void SpawnBigEnemy()
    {
        GameObject newEnemy = Instantiate(enemyBigPrefab, GenerateRandomPos() + (Vector3.up * 10), enemyBigPrefab.transform.rotation);
        newEnemy.transform.SetParent(enemiesGroup.transform);
        newEnemy.GetComponent<Rigidbody>().AddForce(Vector3.down * 50, ForceMode.VelocityChange);
    }

    void SpawnPowerupPush()
    {
        Instantiate(powerupPushPrefab, GenerateRandomPos(), powerupPushPrefab.transform.rotation);
    }
    void SpawnPowerupExplode()
    {
        Instantiate(powerupExplodePrefab, GenerateRandomPos(), powerupExplodePrefab.transform.rotation);
    }
    */
}
