using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShowStats : MonoBehaviour
{
    public TextMeshProUGUI totalTimeText;
    public TextMeshProUGUI timeSpentInFireText;
    public TextMeshProUGUI finalScoreText;

    public void UpdateText(float totalTime, float timeSpentInFire, int score)
    {
        totalTimeText.text = $"Total time: {Mathf.FloorToInt(totalTime/60):D2}:{Mathf.FloorToInt(totalTime%60):D2}";
        timeSpentInFireText.text = $"Time close to fire: {Mathf.FloorToInt(timeSpentInFire/60):D2}:{Mathf.FloorToInt(timeSpentInFire%60):D2}";
        finalScoreText.text = $"Final score: {score}";
    }
}
