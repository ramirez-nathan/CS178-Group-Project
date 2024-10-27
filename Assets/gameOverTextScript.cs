using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class gameOverTextScript : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI winnerText;
    public PlayerScript player;
    public enemyScript enemy;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null)
        {
            winnerText.text = "Game!\nPlayer 2 Wins!";
        }

        else if (enemy == null)
        {
            winnerText.text = "Game!\nPlayer 1 Wins!";
        }
    }
}
