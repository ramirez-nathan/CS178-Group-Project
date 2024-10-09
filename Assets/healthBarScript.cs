using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class healthBarScript : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI healthText;
    public stickManScript stickMan;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        healthText.text = "Player 1 HP: " + stickMan.health.ToString();
    }
}
