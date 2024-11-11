using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSelectorScript : MonoBehaviour
{
    public static StageSelectorScript Instance;  // Singleton instance
    public GameObject[] stagePrefabs;            // Array of stage prefabs

    private int selectedStageIndex = 0;          // Default stage index

    private void Awake()
    {
        // Singleton pattern to ensure only one instance
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  // Keep this object across scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Method to select a stage
    public void SelectStage(int stageIndex)
    {
        selectedStageIndex = stageIndex;
    }

    // Method to get the selected stage prefab
    public GameObject GetSelectedStage()
    {
        return stagePrefabs[selectedStageIndex];
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
