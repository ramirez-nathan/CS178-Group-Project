using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Analytics;

public class Countdown : MonoBehaviour
{
    public GameObject countDownUI;  // Reference to the countdown UI GameObject
    public Counter counter;         // Reference to the Counter script

    private void Start()
    {
        ShowCountDown();
    }

    private void Update()
    {
        // Continuously check if the countdown is over
        if (counter.IsCountDownOver())
        {
            CountDownOver();
        }
    }

    // Show countdown UI
    public void ShowCountDown()
    {
        countDownUI.SetActive(true);  // Display the countdown UI
    }

    // Hide countdown UI and start the game
    public void CountDownOver()
    {
        countDownUI.SetActive(false);   // Hide the countdown UI
    }

}
