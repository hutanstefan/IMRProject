using UnityEngine;
using System.Collections.Generic;

public class DifficultySelector : MonoBehaviour
{
    public string difficulty = "Medium"; 

    public GameLogic gameLogic;
    public FireManager fireManager;
    public GameObject xrRig;
    private List<Transform> startLocations = new List<Transform>();

    /*void Awake()
    {
        difficulty = PlayerPrefs.GetString("difficulty", "Medium");
    }*/

    void Start()
    {
        ApplyDifficultySettings();
        Debug.Log($"Am selectat dificultatea {difficulty}");
    }


    private void ApplyDifficultySettings()
    {
        if (difficulty.Equals("Easy", System.StringComparison.OrdinalIgnoreCase))
        {
            AddLocationsByTag("Easy");
            SetEasySettings();
        }
        else if (difficulty.Equals("Medium", System.StringComparison.OrdinalIgnoreCase))
        {
            AddLocationsByTag("Medium");
            SetMediumSettings();
        }
        else if (difficulty.Equals("Hard", System.StringComparison.OrdinalIgnoreCase))
        {
            AddLocationsByTag("Hard");
            SetHardSettings();
        }
        else
        {
            Debug.LogWarning($"Dificultatea specificată '{difficulty}' nu este validă! Alege între 'Easy', 'Medium' sau 'Hard'.");
        }
          MovePlayerToStartLocation();
    }

 private void MovePlayerToStartLocation()
{
    if (startLocations.Count > 0)
    {
        Transform randomStartLocation = startLocations[Random.Range(0, startLocations.Count)];
        Debug.LogWarning($"Am selectat {randomStartLocation}");
 
        if (xrRig != null)
        {
            xrRig.transform.position = randomStartLocation.position;
            xrRig.transform.rotation = randomStartLocation.rotation;
        }
        else
        {
            Debug.LogWarning("XR Rig nu a fost asignat! Doar camera a fost mutată.");
        }
    }
    else
    {
        Debug.LogError("Nu există locații de start definite pentru dificultatea aleasă!");
    }
}


     private void AddLocationsByTag(string tag)
        {
        GameObject[] locationObjects = GameObject.FindGameObjectsWithTag(tag);

        foreach (var obj in locationObjects)
        {
            startLocations.Add(obj.transform);
        }
        }

    private void SetEasySettings()
    {
        fireManager.spreadRadius = 0.5f; 
        fireManager.baseIntensity = 0.001f;
        fireManager.extraIntensity = 0.5f;
        fireManager.initialIgnitionDelay = 8f;
        fireManager.setSpreadCoolDown(20f);
        gameLogic.targetTime=120;
    }

    private void SetMediumSettings()
    {
        fireManager.spreadRadius = 1f;
        fireManager.baseIntensity = 0.005f;
        fireManager.extraIntensity = 0.6f;
        fireManager.initialIgnitionDelay = 6f;
        fireManager.setSpreadCoolDown(18f);
        gameLogic.targetTime=105;
    }

    private void SetHardSettings()
    {
        fireManager.spreadRadius = 2f; 
        fireManager.baseIntensity = 1f;
        fireManager.extraIntensity = 1f;
        fireManager.initialIgnitionDelay = 3f;
        fireManager.setFireNumber(2); 
        fireManager.setSpreadCoolDown(13f);
         gameLogic.targetTime=90; 
    }
}
