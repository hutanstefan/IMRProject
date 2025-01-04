using UnityEngine;

public class GameLogic : MonoBehaviour
{
    public Transform playerTransform; 
    private float startTime; 
    private bool timerRunning = true; 

    void Start()
    {
        startTime = Time.time;
        timerRunning = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform == playerTransform && timerRunning)
        {
            StopTimer();
        }
    }

    void StopTimer()
    {
        timerRunning = false;
        float elapsedTime = Time.time - startTime; 
        Debug.Log("Timpul scurs: " + elapsedTime + " secunde"); 
    }
}
