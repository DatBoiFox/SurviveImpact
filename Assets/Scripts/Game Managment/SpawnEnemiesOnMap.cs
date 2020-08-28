using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class SpawnEnemiesOnMap : MonoBehaviour
{
    // The number of enemies that will be spawned
    public int enemiesCount;
    // List of enemies prefabs
    public List<Enemy> enemies;
    public GameObject spawner;
    // Points that enemies may be spawned on
    public List<Transform> spawnPoints;

    public AudioSource hordeSaund;

    public bool isCampArea;

    // if true, enemies immediately chase player
    public bool aggroEnemies;
    public bool enemiesSpawned;
    public GameObject spawnSmoke;

    // List of currently active enemies
    private List<Enemy> spawnedEnemies = new List<Enemy>();

    private void SpawnZombies()
    {
        if(hordeSaund != null)
            hordeSaund.Play();
        for (int i = 0; i < enemiesCount; i++)
        {
            Enemy e = Instantiate(enemies[Random.Range(0, enemies.Count)], spawnPoints[Random.Range(0, spawnPoints.Count)].position, Quaternion.identity);
            if (aggroEnemies)
            {
                e.setChaseTarget(FindObjectOfType<PlayerObject>().gameObject);
                e.Chase();
                e.maxChaseTimer = 99999;
            }
            spawnedEnemies.Add(e);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isCampArea)
        {
            if (FindObjectOfType<GameManager>().currentDay != FindObjectOfType<GameManager>().gameTime.Day)
            {
                enemiesSpawned = false;
            }
            if (!enemiesSpawned && FindObjectOfType<GameManager>().gameTime.DayTime >= 280 && Vector3.Distance(FindObjectOfType<PlayerObject>().transform.position, this.transform.position) > 10)
            {
                Destroy(Instantiate(spawnSmoke, spawner.transform.position, Quaternion.identity), 20f);
                enemiesSpawned = true;
                SpawnZombies();
            }

            if (spawnedEnemies.Count > 0)
            {
                for (int i = 0; i < spawnedEnemies.Count; i++)
                {
                    if (spawnedEnemies[i] != null)
                    {
                        if (spawnedEnemies[i].prevTarget == null)
                        {
                            spawnedEnemies[i].setChaseTarget(FindObjectOfType<PlayerObject>().gameObject);
                        }
                    }
                    else
                    {
                        spawnedEnemies.RemoveAt(i);
                    }
                }
            }

        }
        else if (!isCampArea && !enemiesSpawned)
        {
            enemiesSpawned = true;
            SpawnZombies();
        }
    }
}
