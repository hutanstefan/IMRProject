using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public float shakeDuration = 5.0f;  // Durata cutremurului la apăsarea Enter (5 secunde)
    public float shakeMagnitude = 0.1f; // Puterea cutremurului

    public Camera cameraToShake; // Camera pe care să o miște

    private Vector3 initialPosition;
    private float shakeTimeRemaining;

    private void Start()
    {
        if (cameraToShake == null)
        {
            cameraToShake = GetComponent<Camera>();
        }
        
        if (cameraToShake != null)
        {
            initialPosition = cameraToShake.transform.localPosition;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            StartShake(shakeDuration, shakeMagnitude); // Pornește cutremurul pentru 5 secunde
        }

        if (shakeTimeRemaining > 0)
        {
            cameraToShake.transform.localPosition = initialPosition + Random.insideUnitSphere * shakeMagnitude;
            shakeTimeRemaining -= Time.deltaTime;
        }
        else if (shakeTimeRemaining <= 0)
        {
            cameraToShake.transform.localPosition = initialPosition;
        }
    }

    public void StartShake(float duration, float magnitude)
    {
        shakeDuration = duration;
        shakeMagnitude = magnitude;
        shakeTimeRemaining = shakeDuration;
    }
}
