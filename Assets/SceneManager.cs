using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");  // Replace "MainMenu" with the exact name of your main menu scene
    }

    public void LoadSelectModeMenu()
    {
        SceneManager.LoadScene("SelectMode");  // Replace "SelectMode" with the exact name of your select mode scene
    }

    public void LoadGameplayScene()
    {
        SceneManager.LoadScene("Gameplay");  // Replace "Gameplay" with the name of your gameplay scene
    }


}
