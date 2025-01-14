using UnityEngine;

public class GameLogic : MonoBehaviour
{
    public Transform playerTransform;  
    public Transform targetObject;
    public GameObject scoreBoard;
    private float stopDistance = 6f;  
    private float startTime; 
    private bool timerRunning = true; 
    private float elapsedTime = 0f;
    private float timeInFire = 0f; 
    private bool onFire = false;
    private float inFireStartTime;
    private int finalScore = 0;
    public string fireTag = "Fire"; 
    public float targetTime=0f;

    void Start()
    {
        startTime = Time.time;
        timerRunning = true;
        //Debug.Log("Cronometru pornit");
        scoreBoard.SetActive(false);
    }

    void Update()
    {
        if (timerRunning && targetObject != null && playerTransform != null)
        {
            float distance = Vector3.Distance(playerTransform.position, targetObject.position);
            //Debug.Log($"Distance is {distance} in gameLogic");
            if (distance <= stopDistance)
            {
                Debug.Log("Opresc timmerul");
                StopTimer();
            }

            if (InFire() && onFire == false)
            {
                onFire = true;
                inFireStartTime = Time.time;
            }
            else if (!InFire() && onFire == true)
            {
                onFire = false;
                timeInFire += Time.time - inFireStartTime;
            }
        }
    }

    void StopTimer()
    {
        timerRunning = false;
        elapsedTime = Time.time - startTime; 
        //Debug.Log("Timpul scurs: " + elapsedTime + " secunde");
        float timePenalty = Mathf.Max(0f, (elapsedTime - targetTime) / targetTime) * 5f;
        float firePenalty = timeInFire * 0.5f;
        float score = Mathf.Max(0f, 100f - timePenalty - firePenalty);
        finalScore = Mathf.FloorToInt(score);
         //scor maxim 100, am penalizare de timp daca se depaseste target time si penalizare pentru timpul petrecut in foc
        ShowScoreboard();
    }

    private void ShowScoreboard()
    {
        float distanceInFront = 2f;
        scoreBoard.SetActive(true);
        scoreBoard.transform.position = playerTransform.position + playerTransform.forward * distanceInFront;
        scoreBoard.GetComponent<ShowStats>().UpdateText(elapsedTime, timeInFire, finalScore);
    }

    private bool InFire()
    {
        GameObject[] fires = GameObject.FindGameObjectsWithTag(fireTag);
        bool nearFire = false;
        float minDistance = 3f;

        foreach (GameObject fire in fires)
        {
            float distance = Vector3.Distance(fire.transform.position, playerTransform.position);
            if (distance <= minDistance)
            {
                nearFire = true;
                break;
            }
        }
        return nearFire;
    }
}
