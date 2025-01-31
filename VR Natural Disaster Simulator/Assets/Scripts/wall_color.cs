using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallDarkening : MonoBehaviour
{
    public float darkeningRate = 0.00000005f; 
    public float detectionRadius = 0.1f;
    private float timeSinceLastChange = 0f; // Timer pentru intervalul de 2 secunde

    void Update()
    {
        // Incrementăm timpul
        timeSinceLastChange += Time.deltaTime;

        // Dacă au trecut 2 secunde, schimbăm culoarea
        if (timeSinceLastChange >= 5f)
        {
            Debug.Log("Innegresc");
            timeSinceLastChange = 0f;

            // Căutăm coliziuni în raza de detecție
            Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius);
            foreach (Collider col in colliders)
            {
                if (col.gameObject.name.Contains("Wall") && col.gameObject.name.Contains("Int"))
                {
                    Renderer wallRenderer = col.GetComponent<Renderer>();
                    if (wallRenderer != null)
                    {
                        Color currentColor = wallRenderer.material.color;

                        if (currentColor != Color.black)
                        {
                               wallRenderer.material.color = Color.Lerp(currentColor, Color.black, darkeningRate);
                        }
                    }
                }
            }
        }
    }
}
