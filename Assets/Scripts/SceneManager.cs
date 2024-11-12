using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public Button selectModeButton;
    public Button startGameButton;

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
        SceneManager.LoadScene("MainMenu");  
    }

    public void LoadSelectModeMenu()
    {
            SceneManager.LoadScene("SelectMode");  
    }

    public void LoadGameplayScene()
    {
        SceneManager.LoadScene("Game");  
    }

    public void LoadCredits()
    {
        SceneManager.LoadScene("Credits");
    }


}
