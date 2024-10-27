// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class PlayerTwoHealthBar : MonoBehaviour
// {
//     public enemyScript enemy;       // reference to our enemy
//     // Start is called before the first frame update
//     void Start()
//     {
        
//     }

//     // Update is called once per frame
//     void Update()
//     {
//         // Check if the player reference is still valid and that maxHealth is not zero
//         if (enemy != null && enemy.health > 0)
//         {
//             // Calculate the health percentage (between 0 and 1)
//             float healthPercentage = enemy.currentHealth / enemy.health;

//             // Ensure healthPercentage is between 0 and 1
//             healthPercentage = Mathf.Clamp(healthPercentage, 0, 1);

//             // Update the scale of the health bar based on the health percentage
//             Vector3 scale = transform.localScale;
//             scale.x = healthPercentage; // Adjust only the x scale to change the width of the health bar
//             transform.localScale = scale;
//         }
//         else
//         {
//             // Optionally disable or destroy the health bar when the enemy is destroyed
//             gameObject.SetActive(false); // Disables the health bar when the enemy is null or maxHealth is 0
//         }
//     }
// }
