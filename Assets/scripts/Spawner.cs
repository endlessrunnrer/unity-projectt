using UnityEngine;

public class Spawner : MonoBehaviour
{

    [Header("Settings")]
    public float minSpawnDelay; 
    public float maxSpawnDelay; 

    [Header("References")]
    public GameObject[] gameObjects;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {
        Invoke("Spawn", Random.Range(minSpawnDelay, maxSpawnDelay)); 
    }


    void OnDisable(){
        CancelInvoke();
    }


    void Spawn(){

        var randomObject = gameObjects[Random.Range(0, gameObjects.Length)]; 
        Vector3 spawnPosition = new Vector3(transform.position.x, transform.position.y, 10);

        Instantiate(randomObject, transform.position, Quaternion.identity); 
        Invoke("Spawn", Random.Range(minSpawnDelay, maxSpawnDelay)); 
        }   
    }
