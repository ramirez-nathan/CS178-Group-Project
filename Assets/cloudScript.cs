using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cloudScript : MonoBehaviour
{

    public float moveSpeed = 2F;
    public float outOfBounds = -15f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += (Vector3.left * moveSpeed) * Time.deltaTime;

        if (transform.position.x < outOfBounds)
        {
            Debug.Log("Cloud destroyed");
            Destroy(gameObject);
        }
    }
}
