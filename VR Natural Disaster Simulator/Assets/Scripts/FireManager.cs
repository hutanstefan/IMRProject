using UnityEngine;
using System.Collections.Generic;

public class FireManager : MonoBehaviour
{
    public string flammableTag = "Inflamable";
    public GameObject fireEffectPrefab;       
    public float spreadRadius = 1f;           
    public float baseIntensity = 1f;         
    public float extraIntensity = 1f;        
    public float initialIgnitionDelay = 5f;   
    private float spreadCooldown = 40f; 
    private float lastSpreadTime = 0f;

    private List<InflamableObject> burningObjects = new List<InflamableObject>(); 

    void Start()
    {
        Invoke(nameof(StartRandomFire), initialIgnitionDelay);
    }

    void Update()
    {
        SpreadFire();
    }

    void StartRandomFire()
    {
        GameObject[] flammableObjects = GameObject.FindGameObjectsWithTag(flammableTag);

        if (flammableObjects.Length > 0)
        {
            GameObject randomObject = flammableObjects[Random.Range(0, flammableObjects.Length)];
            InflamableObject inflamableComponent = randomObject.GetComponent<InflamableObject>();

            if (inflamableComponent != null)
            {
                inflamableComponent.Ignite(baseIntensity,3f);
                burningObjects.Add(inflamableComponent);
                CreateFireEffect(inflamableComponent.transform.position, baseIntensity, inflamableComponent.GetComponent<Renderer>().bounds.size);
                Debug.Log($"{randomObject.name} a început să ardă! Are dimensiunea:{inflamableComponent.GetComponent<Renderer>().bounds.size} Intensitatea focului: {baseIntensity}");
            }
            else
            {
                Debug.LogError($"Obiectul {randomObject.name} nu are componenta InflamableObject!");
            }
        }
        else
        {
            Debug.LogError("Nu au fost găsite obiecte cu tag-ul Inflamable.");
        }
    }


void SpreadFire()
{
    if (Time.time - lastSpreadTime < spreadCooldown) 
        return; 
    lastSpreadTime = Time.time;
    List<GameObject> newBurningObjects = new List<GameObject>();

    foreach (var burningObject in burningObjects)
    {
        if (burningObject != null)  
        {
            Collider[] nearbyObjects = Physics.OverlapSphere(burningObject.transform.position, spreadRadius);

            foreach (var collider in nearbyObjects)
            {
                InflamableObject nearbyInflamable = collider.GetComponent<InflamableObject>();

                if (nearbyInflamable != null && !nearbyInflamable.isBurning && !burningObjects.Contains(nearbyInflamable))
                {
                    float intensity = baseIntensity;
                    float burnRate = 1f;

                    if (nearbyInflamable.CompareTag(flammableTag))
                    {
                        intensity += extraIntensity;
                        burnRate = 1.5f;            
                    }
                    else
                    {
                        intensity *= 0.5f; 
                        burnRate = 0.5f;   
                    }

                    nearbyInflamable.Ignite(intensity, burnRate);
                    newBurningObjects.Add(nearbyInflamable.gameObject);  

                    CreateFireEffect(nearbyInflamable.transform.position, intensity, nearbyInflamable.GetComponent<Renderer>().bounds.size);

                    //Debug.Log($"Obiect {nearbyInflamable.name} arde cu burnRate {burnRate} și intensitate {intensity}");
                }
            }
        }
    }

   foreach (var newObject in newBurningObjects)
{
    var inflamableComponent = newObject.GetComponent<InflamableObject>();
    if (inflamableComponent != null)
    {
        burningObjects.Add(inflamableComponent);
    }
}
}


void CreateFireEffect(Vector3 position, float intensity, Vector3 objectSize)
    {
        GameObject fireEffect = Instantiate(fireEffectPrefab, position-objectSize/2, Quaternion.identity);
        fireEffect.transform.localScale = objectSize;


        ParticleSystem fireParticleSystem = fireEffect.GetComponent<ParticleSystem>();
        ParticleSystem.ShapeModule shape = fireParticleSystem.shape;
        shape.radius = intensity * 2f;  

        var main = fireParticleSystem.main;
        main.startLifetime = Mathf.Lerp(1f, 2f, intensity / 10f); 
        main.startSize = Mathf.Max(0.2f, Mathf.Min(objectSize.y, 2 * objectSize.x));
    
        fireParticleSystem.Play();
    }
}
