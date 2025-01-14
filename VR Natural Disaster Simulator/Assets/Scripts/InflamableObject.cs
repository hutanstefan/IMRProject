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

    void Start()
    {
        rb = GetComponent<Rigidbody>(); 
        objectRenderer = GetComponent<Renderer>();
        if (objectRenderer != null)
        {
            objectMaterial = objectRenderer.material; 
        }
    }

    public void Ignite(float intensity, float burnRateParam)
    {
        if (!isBurning)
        {
            isBurning = true;
            fireIntensity = intensity;
            burnRate=burnRateParam;
           // Debug.Log($"{gameObject.name} a început să ardă cu intensitatea {fireIntensity}.");
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
