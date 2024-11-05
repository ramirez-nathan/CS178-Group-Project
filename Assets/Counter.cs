using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class Counter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI counterText;
    public bool countDownOver = false;

    private void Start()
    {
        Time.timeScale = 0f;
        StartCoroutine(StartCountdown());  // Start the countdown coroutine when the scene starts
    }

    private IEnumerator StartCountdown()
    {
        // Countdown values, unaffected by Time.timeScale
        counterText.text = "3";
        yield return new WaitForSecondsRealtime(1f);

        counterText.text = "2";
        yield return new WaitForSecondsRealtime(1f);

        counterText.text = "1";
        yield return new WaitForSecondsRealtime(1f);

        counterText.text = "Start";
        yield return new WaitForSecondsRealtime(1f);

        // Clear the text after the countdown is complete
        counterText.text = "";

        Time.timeScale = 1f;
    }

    // Public method to check if countdown is over
    public bool IsCountDownOver()
    {
        return countDownOver;
    }
}

