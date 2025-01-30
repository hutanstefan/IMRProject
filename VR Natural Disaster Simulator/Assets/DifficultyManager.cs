using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    public Earthquake earthquake; // Referință la scriptul Earthquake
    private string difficulty;

    void Awake()
    {
        // Preia dificultatea salvată din PlayerPrefs
        difficulty = PlayerPrefs.GetString("difficulty", "Medium");
    }

    void Start()
    {
        ApplyDifficultySettings();
        Debug.Log($"Dificultatea selectată: {difficulty}");
    }

    private void ApplyDifficultySettings()
    {
        if (earthquake == null)
        {
            Debug.LogError("Earthquake nu este asignat în DifficultyManager!");
            return;
        }

        switch (difficulty)
        {
            case "Easy":
                SetEasySettings();
                break;
            case "Medium":
                SetMediumSettings();
                break;
            case "Hard":
                SetHardSettings();
                break;
            default:
                Debug.LogWarning($"Dificultatea '{difficulty}' nu este validă! Se folosește setarea implicită (Medium).");
                SetMediumSettings();
                break;
        }
    }

    private void SetEasySettings()
    {
        earthquake.intensity = 0.5f;
        earthquake.duration = 15f;
        earthquake.shakeInterval = 0.1f;
        earthquake.directionRange = 0.5f;
    }

    private void SetMediumSettings()
    {
        earthquake.intensity = 1f;
        earthquake.duration = 30f;
        earthquake.shakeInterval = 0.05f;
        earthquake.directionRange = 1f;
    }

    private void SetHardSettings()
    {
        earthquake.intensity = 2f;
        earthquake.duration = 45f;
        earthquake.shakeInterval = 0.02f;
        earthquake.directionRange = 2f;
    }
}
