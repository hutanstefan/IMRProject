using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Earthquake : MonoBehaviour
{
    public CeilingParticles ceilingParticles;
    public float intensity = 20f; // Intensitatea cutremurului
    public float duration = 30f; // Durata cutremurului în secunde
    public float startDelay = 5f; // Întârzierea de pornire în secunde
    public float shakeInterval = 0.1f; // Cât de des se schimbă direcția cutremurului
    public float directionRange = 3f;

    private bool isShaking = false;
    private float shakeTimer;

    void Start()
    {
        StartCoroutine(StartEarthquakeAfterDelay(startDelay));
    }

    void Update()
    {
        if (isShaking)
        {
            ceilingParticles.StartParticles();
        }
        else
        {
            ceilingParticles.StopParticles();
        }
    }

    IEnumerator StartEarthquakeAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        StartCoroutine(Shake());
    }

    IEnumerator Shake()
    {
        isShaking = true;
        shakeTimer = duration;
        Rigidbody[] rigidbodies = FindObjectsOfType<Rigidbody>();

        while (shakeTimer > 0)
        {
            // Generăm o direcție globală aleatoare o singură dată per iterație
            Vector3 shakeDirection = new Vector3(
                Random.Range(-directionRange, directionRange), // Direcție pe X
                0,                     // Fără mișcare pe Y
                Random.Range(-directionRange, directionRange)  // Direcție pe Z
            ).normalized * intensity;

            // Aplicăm forța tuturor obiectelor în aceeași direcție
            foreach (var rb in rigidbodies)
            {
                rb.AddForce(shakeDirection, ForceMode.Impulse);
            }

            shakeTimer -= shakeInterval;
            yield return new WaitForSeconds(shakeInterval); // Așteptăm înainte de a schimba direcția
        }

        isShaking = false;
    }
}
