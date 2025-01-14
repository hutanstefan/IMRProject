using UnityEngine;

public class DifficultySelector : MonoBehaviour
{
    public string difficulty = "Medium"; 

    public GameLogic gameLogic;
    public FireManager fireManager;

    void Start()
    {
        ApplyDifficultySettings();
    }


    private void ApplyDifficultySettings()
    {
        if (difficulty.Equals("Easy", System.StringComparison.OrdinalIgnoreCase))
        {
            SetEasySettings();
        }
        else if (difficulty.Equals("Medium", System.StringComparison.OrdinalIgnoreCase))
        {
            SetMediumSettings();
        }
        else if (difficulty.Equals("Hard", System.StringComparison.OrdinalIgnoreCase))
        {
            SetHardSettings();
        }
        else
        {
            Debug.LogWarning($"Dificultatea specificată '{difficulty}' nu este validă! Alege între 'Easy', 'Medium' sau 'Hard'.");
        }
    }

    private void SetEasySettings()
    {
        fireManager.spreadRadius = 0.5f; 
        fireManager.baseIntensity = 0.8f;
        fireManager.extraIntensity = 0.5f;
        fireManager.initialIgnitionDelay = 8f;
        fireManager.setSpreadCoolDown(50f);
        gameLogic.targetTime=120;
    }

    private void SetMediumSettings()
    {
        fireManager.spreadRadius = 1f;
        fireManager.baseIntensity = 1f;
        fireManager.extraIntensity = 1f;
        fireManager.initialIgnitionDelay = 6f;
        fireManager.setSpreadCoolDown(30f);
        gameLogic.targetTime=105;
    }

    private void SetHardSettings()
    {
        fireManager.spreadRadius = 2f; 
        fireManager.baseIntensity = 1.5f;
        fireManager.extraIntensity = 2f;
        fireManager.initialIgnitionDelay = 3f;
        fireManager.setFireNumber(2); 
        fireManager.setSpreadCoolDown(10f);
         gameLogic.targetTime=90; 
    }
}
