using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddRigidbodiesInCollider : MonoBehaviour
{
    public Collider areaCollider; // Colliderul care definește zona
    public float densityFactor = 1f; // Factorul de densitate pentru calculul masei
    public float staticVolumeThreshold = 10f; // Pragul de volum pentru a marca obiectele ca statice
    private bool hasBeenApplied = false;

    // Lista cuvintele cheie pentru obiectele statice
    public string[] staticKeywords = { "counter", "cabinet", "drawer", "building", "wall", "floor", "ceiling", "pillar", "door", "window", "roof" };

    void Update()
    {
        // Detectează apăsarea tastei Space
        if (Input.GetKeyDown(KeyCode.Space) && !hasBeenApplied)
        {
            ApplyRigidbodies();
            hasBeenApplied = true;
        }
    }

    private void ApplyRigidbodies()
    {
        // Găsește toate obiectele din zonă
        Collider[] objectsInArea = Physics.OverlapBox(
            areaCollider.bounds.center,
            areaCollider.bounds.extents,
            areaCollider.transform.rotation
        );

        foreach (Collider col in objectsInArea)
        {
            GameObject obj = col.gameObject;

            // Adaugă Rigidbody doar dacă obiectul nu are deja unul
            if (!obj.GetComponent<Rigidbody>() && !IsStaticObject(obj))
            {
                Rigidbody rb = obj.AddComponent<Rigidbody>();

                // Calculează masa
                float mass = CalculateMass(obj);
                rb.mass = mass;

                // Verifică dacă obiectul are un MeshCollider non-convex
                MeshCollider meshCollider = obj.GetComponent<MeshCollider>();
                if (meshCollider != null && !meshCollider.convex)
                {
                    // Dacă este non-convex, setează Rigidbody ca kinematic
                    rb.isKinematic = true;
                }
                else
                {
                    // Dacă este convex sau alt tip de collider, lasă Rigidbody să fie dinamic
                    rb.isKinematic = false;
                }

                
            }
        }
    }

    private float CalculateMass(GameObject obj)
    {
        // Obține dimensiunile bounding box-ului
        Renderer renderer = obj.GetComponent<Renderer>();
        if (renderer != null)
        {
            Vector3 size = renderer.bounds.size;
            float volume = size.x * size.y * size.z; // Aproximăm volumul ca produsul dimensiunilor
            return volume * densityFactor; // Masa este proporțională cu volumul
        }

        return 1f; // Masa implicită pentru obiectele fără Renderer
    }

    private bool IsStaticObject(GameObject obj)
    {
        // Convertim numele obiectului la litere mici pentru a face verificarea case-insensitive
        string lowerName = obj.name.ToLower();

        foreach (string keyword in staticKeywords)
        {
            // Verificăm dacă numele obiectului începe sau se potrivește exact cu un cuvânt cheie
            if (lowerName.Contains(keyword) || lowerName.StartsWith(keyword.ToLower()))
            {
                return true;
            }
        }

        return false;
    }

}
