using UnityEngine;

public class InflamableObject : MonoBehaviour
{
    public bool isBurning = false;        
    public float fireIntensity = 0f; 
    public float burnRate=0f;  
    public GameObject fireEffectPrefab; 
    private GameObject fireEffectInstance;
     public float health = 100f;
     private Rigidbody rb; 
     private Renderer objectRenderer;     
    private Material objectMaterial;
    private float lastReductionTime;
    private float regenerationDelay = 2f; // 2 seconds delay for regeneration


    void Start()
    {
        rb = GetComponent<Rigidbody>(); 
        objectRenderer = GetComponent<Renderer>();
        if (objectRenderer != null)
        {
            objectMaterial = objectRenderer.material; 
        }
    }

    public void Ignite(float intensity, float burnRateParam, GameObject fireEffect)
    {
        if (!isBurning)
        {
            isBurning = true;
            fireIntensity = intensity;
            burnRate = burnRateParam;
            fireEffectInstance = fireEffect;
            lastReductionTime = Time.time;
        }
    }

    public void ReduceFireIntensity(float amount)
    {
        if (isBurning)
        {
            fireIntensity -= amount;
            lastReductionTime = Time.time;
            if (fireIntensity <= 0f)
            {
                fireIntensity = 0f;
                Extinguish();
            }
            else
            {
                // Scale down the fire effect based on the current fireIntensity
                if (fireEffectInstance != null)
                {
                    ParticleSystem fireParticleSystem = fireEffectInstance.GetComponent<ParticleSystem>();
                    if (fireParticleSystem != null)
                    {
                        var main = fireParticleSystem.main;
                        main.startSize = Mathf.Lerp(0.1f, 1f, fireIntensity / 100f);
                    }
                }
            }
        }
    }
     void Update()
    {
       if (isBurning)
    {
        health -= Time.deltaTime * burnRate*2;
        //Debug.Log($"{gameObject.name} are {health} viata.");

        if (health <= 0f)
        {
            //Debug.Log($"{gameObject.name} a ars complet");
            health = 0f;
            Extinguish(); 
        }

        float burnFactor = 1f - (health / 100f); 
        Color burnColor = Color.Lerp(Color.white, Color.black, burnFactor);

        if (objectRenderer != null)
        {
            foreach (Material mat in objectRenderer.materials)
            {
                if (mat.shader.name == "Standard") 
                {
                    mat.SetColor("_Color", burnColor);
                }
                else if (mat.shader.name.Contains("Unlit")) 
                {
                    mat.SetColor("_Color", burnColor);
                }
                else if (mat.HasProperty("_BaseColor")) 
                {
                    mat.SetColor("_BaseColor", burnColor);
                }
                else
                {
                    Debug.LogWarning($"Shader-ul materialului {mat.name} nu este standard și nu poate fi modificat direct.");
                }
            }
        }
        // Regenerate fire if not extinguished within the delay
        if (Time.time - lastReductionTime > regenerationDelay)
        {
            fireIntensity = Mathf.Min(fireIntensity + burnRate * Time.deltaTime, 100f);
            if (fireEffectInstance != null)
            {
                ParticleSystem fireParticleSystem = fireEffectInstance.GetComponent<ParticleSystem>();
                if (fireParticleSystem != null)
                {
                    var main = fireParticleSystem.main;
                    main.startSize = Mathf.Lerp(0.1f, 1f, fireIntensity / 100f);
                }
            }
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
                ParticleSystem fireParticleSystem = fireEffectInstance.GetComponent<ParticleSystem>();
                if (fireParticleSystem != null)
                {
                    //Debug.Log("Particulele de foc au fost oprite.");
                    fireParticleSystem.Stop();
                }
                Destroy(fireEffectInstance);
                fireEffectInstance = null; // Ensure the reference is cleared
            }

            //Debug.Log($"{gameObject.name} has been extinguished.");
        }
    }
}
