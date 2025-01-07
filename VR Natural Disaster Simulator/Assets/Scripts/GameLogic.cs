using UnityEngine;

public class GameLogic : MonoBehaviour
{
    public Transform playerTransform;  
    public Transform targetObject;    
    private float stopDistance = 5f;  
    private float startTime; 
    private bool timerRunning = true; 
    private float timeInFire=0; //timpul petrecut in apropierea focului
    public string fireTag = "Fire"; 

    void Start()
    {
        startTime = Time.time;
        timerRunning = true;
        Debug.Log("Cronometru pornit");
    }

    void Update()
    {
        if (timerRunning && targetObject != null && playerTransform != null)
        {
            float distance = Vector3.Distance(playerTransform.position, targetObject.position);
            Debug.Log($"Distance is {distance} in gameLogic");
            if (distance <= stopDistance)
            {
                Debug.Log("Opresc timmerul");
                StopTimer();
            }
        }
    }

    void StopTimer()
    {
        timerRunning = false;
        float elapsedTime = Time.time - startTime; 
        Debug.Log("Timpul scurs: " + elapsedTime + " secunde"); 
    }
}
