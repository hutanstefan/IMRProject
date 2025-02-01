using System.Collections;
using UnityEngine;

public class ScoreEarthquake : MonoBehaviour
{
    public Transform player; // Reference to player
    public GameObject scoreBoard; // Reference to scoreboard
    public float checkRadius = 4.0f; // Check radius
    public float duration = 30f; // Duration of the earthquake
    public float delay = 5f; // Delay before starting the check
    private int score = 0; // Overall score

    void Start()
    {
        // Start checking after the specified delay
        scoreBoard.SetActive(false);
        StartCoroutine(StartEarthquakeCheck());
    }

    // Coroutine to start checking after a delay
    IEnumerator StartEarthquakeCheck()
    {
        yield return new WaitForSeconds(delay); // Wait for the delay

        float safeTimeSpent = 0f; // Time spent near safe spots
        float unsafeTimeSpent = 0f; // Time spent near unsafe spots
        float normalTimeSpent = 0f;

        while (safeTimeSpent + unsafeTimeSpent + normalTimeSpent < duration)
        {
            GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();
            bool isNearSafeSpot = false;
            bool isNearUnsafeSpot = false;
            bool isNearNothing = false;

            foreach (GameObject obj in allObjects)
            {
                Vector3 playerPos = player.position;
                Vector3 objPos = obj.transform.position;

                // Check if the player is near the object
                bool isWithinX = Mathf.Abs(playerPos.x - objPos.x) <= checkRadius;
                bool isWithinY = Mathf.Abs(playerPos.y - objPos.y) <= checkRadius + 3f;
                bool isWithinZ = Mathf.Abs(playerPos.z - objPos.z) <= checkRadius;

                if (isWithinX && isWithinY && isWithinZ)
                {
                    if (obj.name.ToLower().Contains("doorframe") || obj.name.ToLower().Contains("table_dining"))
                    {
                        Debug.Log("Player is near a safe spot!");
                        isNearSafeSpot = true;
                    }
                    else if (obj.name.ToLower().Contains("window_frame") || obj.name.ToLower().Contains("staircase"))
                    {
                        Debug.Log("Player is near an unsafe spot!");
                        isNearUnsafeSpot = true;
                    }
                    else isNearNothing = true;
                }
            }

            if (isNearSafeSpot)
            {
                safeTimeSpent += Time.deltaTime;
            }

            if (isNearUnsafeSpot)
            {
                unsafeTimeSpent += Time.deltaTime;
            }

            if (isNearNothing)
            {
                normalTimeSpent += Time.deltaTime;
            }

            yield return null; // Wait for the next frame
        }

        CalculateScore(safeTimeSpent, unsafeTimeSpent);
    }

    // Calculate the overall score based on time spent near safe and unsafe spots
    private void CalculateScore(float safeTime, float unsafeTime)
    {
        if (duration <= 0)
        {
            Debug.LogError("Earthquake duration is 0! Cannot calculate score.");
            return;
        }

        // Score based on safe and unsafe times
        float safeScore = (safeTime / duration) * 100f;
        float unsafeScore = (unsafeTime / duration) * 100f;

        // Adjust overall score
        score = Mathf.FloorToInt(safeScore - unsafeScore);

        Debug.Log($"Final Score: {score}\nSafe Spot Time: {safeTime}s\nUnsafe Spot Time: {unsafeTime}s");

        ShowScoreboard(safeTime, unsafeTime);
    }

    private void ShowScoreboard(float safeTime, float unsafeTime)
    {
        scoreBoard.SetActive(true);
        scoreBoard.GetComponent<EarthquakeStats>().UpdateText(safeTime, unsafeTime, score);
    }
}
