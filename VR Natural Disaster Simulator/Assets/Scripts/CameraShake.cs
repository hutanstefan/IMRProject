using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    public float delay = 5.0f; // Delay-ul de pornire a cutremurului (5 secunde)
    public float shakeDuration = 5.0f;  // Durata cutremurului la apăsarea Enter (5 secunde)
    public float shakeMagnitude = 0.1f; // Puterea cutremurului
    public Transform cameraToShake; // Camera pe care să o miște
    public AudioClip shakeSound; // Sunetul cutremurului

    private Vector3 initialPosition;
    private float shakeTimeRemaining;
    private bool hasStarted = false;
    public AudioSource audioSource;

    private void Start()
    {
        if (cameraToShake == null)
        {
            cameraToShake = GetComponent<Transform>();
        }

        if (cameraToShake != null)
        {
            initialPosition = cameraToShake.transform.localPosition;
        }

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = shakeSound;
        audioSource.playOnAwake = false; // Prevent the sound from playing automatically

        StartCoroutine(StartShakeAfterDelay(delay));
    }

    private void Update()
    {
        if (shakeTimeRemaining > 0)
        {
            cameraToShake.transform.localPosition = initialPosition + Random.insideUnitSphere * shakeMagnitude;
            shakeTimeRemaining -= Time.deltaTime;
        }
        else if (shakeTimeRemaining <= 0 && hasStarted)
        {
            audioSource.Stop();
            cameraToShake.transform.localPosition = initialPosition;
        }
    }

    private IEnumerator StartShakeAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        StartShake(shakeDuration, shakeMagnitude);
        hasStarted = true;
    }

    public void StartShake(float duration, float magnitude)
    {
        shakeDuration = duration;
        shakeMagnitude = magnitude;
        shakeTimeRemaining = shakeDuration;
        audioSource.loop = true;
        audioSource.Play(); // Play the shake sound
    }
}