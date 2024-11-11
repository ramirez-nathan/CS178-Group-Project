using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameOverScreen : MonoBehaviour
{
    public GameObject gameOverUI;  // Reference to the Game Over UI element
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowGameOver()
    {
        gameOverUI.SetActive(true);  // Display the Game Over screen
        // Optionally, pause the game or stop player inputs
        Time.timeScale = 0f;  // Freeze the game by setting time scale to 0
    }

}
