using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EarthquakeStats : MonoBehaviour
{
    public TextMeshProUGUI safeTimeText;
    public TextMeshProUGUI unsafeTimeText;
    public TextMeshProUGUI finalScoreText;

    public void UpdateText(float safeTime, float unsafeTime, int score)
    {
        safeTimeText.text = $"Safe time: {Mathf.FloorToInt(safeTime/60):D2}:{Mathf.FloorToInt(safeTime%60):D2}";
        unsafeTimeText.text = $"Unsafe time: {Mathf.FloorToInt(unsafeTime/60):D2}:{Mathf.FloorToInt(unsafeTime%60):D2}";
        finalScoreText.text = $"Final score: {score}";
    }
}
