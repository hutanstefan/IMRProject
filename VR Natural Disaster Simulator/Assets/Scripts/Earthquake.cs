using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Earthquake : MonoBehaviour
{
    public float intensity = 5f; // Intensitatea cutremurului
    public float duration = 2f; // Durata cutremurului în secunde

    private bool isShaking = false;
    private float shakeTimer;

    void Update()
    {
        // Detectăm dacă utilizatorul apasă Enter
        if (Input.GetKeyDown(KeyCode.Return) && !isShaking)
        {
            StartCoroutine(Shake());
        }
    }

    IEnumerator Shake()
    {
        isShaking = true;
        shakeTimer = duration;

        // Luăm toate obiectele din scenă cu Rigidbody
        Rigidbody[] rigidbodies = FindObjectsOfType<Rigidbody>();

        while (shakeTimer > 0)
        {
            foreach (var rb in rigidbodies)
            {
                // Aplicăm o forță aleatoare doar pe axele X și Z
                Vector3 randomForce = new Vector3(
                    Random.Range(-intensity, intensity), // Mișcare pe X
                    0,                                  // Fără mișcare pe Y
                    Random.Range(-intensity, intensity) // Mișcare pe Z
                );

                rb.AddForce(randomForce, ForceMode.Impulse);
            }

            shakeTimer -= Time.deltaTime;
            yield return null; // Așteptăm până la următorul frame
        }

        isShaking = false;
    }
}
