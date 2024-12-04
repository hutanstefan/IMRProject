using UnityEngine;

public class InflamableObject : MonoBehaviour
{
    public bool isBurning = false;        // Stare actuală (dacă obiectul arde)
    public float fireIntensity = 0f;     // Intensitatea focului
    public GameObject fireEffectPrefab;  // Prefab-ul efectului de foc
    private GameObject fireEffectInstance;
     public float health = 100f;
     private Rigidbody rb; 

    void Start()
    {
        rb = GetComponent<Rigidbody>(); 
    }

    public void Ignite(float intensity, float burnRate)
    {
        if (!isBurning)
        {
            isBurning = true;
            fireIntensity = intensity;

            if (fireEffectPrefab != null)
            {
                fireEffectInstance = Instantiate(fireEffectPrefab, transform.position, Quaternion.identity, transform);
            }

            Debug.Log($"{gameObject.name} a început să ardă cu intensitatea {fireIntensity}.");
        }
        else{
             health -= burnRate * Time.deltaTime;

            if (health <= 0 && rb == null) 
            {
                rb = gameObject.AddComponent<Rigidbody>();
                
                if (fireEffectPrefab != null)
                {
                    ParticleSystem fire = Instantiate(fireEffectPrefab, transform.position, Quaternion.identity).GetComponent<ParticleSystem>();
                    var main = fire.main;
                    main.startSize = 0.5f; 
                }

                Destroy(this);
            }
        }
    }

    public void Extinguish()
    {
        if (isBurning)
        {
            isBurning = false;
            fireIntensity = 0f;

            if (fireEffectInstance != null)
            {
                Destroy(fireEffectInstance);
            }

            Debug.Log($"{gameObject.name} a fost stins.");
        }
    }
}
