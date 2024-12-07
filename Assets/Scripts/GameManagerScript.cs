using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // Retrieve the selected stage prefab
        GameObject selectedStage = StageSelectorScript.Instance.GetSelectedStage();

        if (selectedStage != null)
        {
            // Instantiate the selected stage at the origin
            Instantiate(selectedStage, Vector3.zero, Quaternion.identity);
        }
        else
        {
            Debug.LogError("No stage selected! Did you forget to select a stage?");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
