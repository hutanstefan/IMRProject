using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddRigidbodiesInCollider : MonoBehaviour
{
    public Collider areaCollider; // Colliderul care definește zona
    public float densityFactor = 1f; // Factorul de densitate pentru calculul masei
    public float staticVolumeThreshold = 10f; // Pragul de volum pentru a marca obiectele ca statice
    public float minimumMass = 1f;
    private bool hasBeenApplied = false;
    public float startDelay = 5f; // Întârzierea de pornire în secunde
    // Lista cuvintele cheie pentru obiectele statice
    private string[] staticKeywords = { "counter", "cabinet", "drawer", "building", "wall", "floor", "ceiling", "pillar", "door", "window", "roof" ,"xr", "manager", "glassmodel", "colliderapartament", "ceilingparticles"};
    private string[] separators = {"_", ",", ".", "-", " "};

    void Start()
    {
        StartCoroutine(ApplyRigidbodiesAfterDelay(startDelay));
    }
    
    private IEnumerator ApplyRigidbodiesAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        ApplyRigidbodies();
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
                if(mass < 1)
                    mass = minimumMass;
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

        // Separăm numele în cuvinte folosind separatorii
        string[] words = lowerName.Split(separators, System.StringSplitOptions.RemoveEmptyEntries);

        foreach (string word in words)
        {
            foreach (string keyword in staticKeywords)
            {
                // Verificăm dacă cuvântul conține sau este exact egal cu un cuvânt cheie
                if (word.Contains(keyword) || word == keyword)
                {
                    return true;
                }
            }
        }

        return false;
    }

}
