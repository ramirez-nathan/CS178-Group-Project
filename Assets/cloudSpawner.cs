using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class cloudSpawner : MonoBehaviour
{
    public GameObject cloud;
    public float spawnRate = 30;
    private float timer = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        spawnCloud();
    }

    // Update is called once per frame
    void Update()
    {
        if (timer < spawnRate)
        {
            timer += Time.deltaTime;
        }
        else
        {
            spawnCloud();
            timer = 0;
        }

        
    }

    void spawnCloud()
    {
        float lowestPoint = 3.3f;  
        float highestPoint = 4.4f;  

        float randomY = Random.Range(lowestPoint, highestPoint);

        Vector3 spawnPosition = new Vector3(transform.position.x, randomY, transform.position.z);

        Instantiate(cloud, spawnPosition, transform.rotation);
    }

}
