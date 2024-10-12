using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class enemyHealthScript : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI healthText;
    public enemyScript enemy;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        healthText.text = "Player 2 HP: " + enemy.currentHealth.ToString();
    }
}
