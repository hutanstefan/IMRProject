using UnityEngine;
using System.Collections.Generic;
using System.Collections;

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
    private float numberOfStartedFire=1;
    public AudioSource fireAlarmAudioSource;
    public bool firstFireStarted=false;

    private List<InflamableObject> burningObjects = new List<InflamableObject>(); 
    private Coroutine alarmCoroutine;
    void Start()
    {
        if (fireAlarmAudioSource == null)
        {
            Debug.LogError("Fire alarm audio source is not assigned.");
        }
        else
        {
            fireAlarmAudioSource.loop = false;
        }
        Invoke(nameof(StartRandomFire), initialIgnitionDelay);
    }

    void Update()
    {
        SpreadFire();
        RemoveExtinguishedFires();
    }

    public void setSpreadCoolDown(float val)
    {
        spreadCooldown=val;
    }

    public void  setFireNumber(float nr)
    {
        numberOfStartedFire=nr;
    }

    void RemoveExtinguishedFires()
    {
        burningObjects.RemoveAll(obj => obj == null || !obj.isBurning);

        if (burningObjects.Count == 0 && fireAlarmAudioSource.isPlaying)
        {
            if (alarmCoroutine != null)
            {
                StopCoroutine(alarmCoroutine);
            }
            Debug.Log("All fires extinguished. Stopping alarm.");
            fireAlarmAudioSource.Stop();
        }
    }

    void StartRandomFire()
    {
        GameObject[] flammableObjects = GameObject.FindGameObjectsWithTag(flammableTag);

        if (flammableObjects.Length == 0)
        {
            Debug.LogError("No objects found with the Inflamable tag.");
            return;
        }

        List<GameObject> availableObjects = new List<GameObject>(flammableObjects);

        int firesToStart = Mathf.Min(Mathf.FloorToInt(numberOfStartedFire), availableObjects.Count);

        for (int i = 0; i < firesToStart; i++)
        {
            GameObject randomObject = availableObjects[Random.Range(0, availableObjects.Count)];
            InflamableObject inflamableComponent = randomObject.GetComponent<InflamableObject>();

            if (inflamableComponent != null)
            {
                
                GameObject fireEffect = CreateFireEffect(inflamableComponent.transform.position, baseIntensity, inflamableComponent.GetComponent<Renderer>().bounds.size);
                inflamableComponent.Ignite(baseIntensity, 3f, fireEffect);
                burningObjects.Add(inflamableComponent);

                availableObjects.Remove(randomObject);

                if (!firstFireStarted)
                {
                    firstFireStarted = true;
                    if (alarmCoroutine != null)
                    {
                        StopCoroutine(alarmCoroutine);
                    }
                    alarmCoroutine = StartCoroutine(StartAlarmWithDelay(3f));
                }
            }
            else
            {
                Debug.LogError($"Object {randomObject.name} does not have an InflamableObject component!");
                availableObjects.Remove(randomObject);
            }
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
                        Vector3 direction = nearbyInflamable.transform.position - burningObject.transform.position;
                        float distance = direction.magnitude;
    
                        RaycastHit hit;
                        if (Physics.Raycast(burningObject.transform.position, direction, out hit, distance))
                        {
                            if (hit.collider.gameObject.name.ToLower().Contains("wall"))
                            {
                               // Debug.Log($"Obstacle detected: {hit.collider.gameObject.name}. Cannot ignite {nearbyInflamable.gameObject.name}.");
                                continue;
                            }
    
                            Vector3 edgePosition = burningObject.transform.position + direction.normalized * distance;
                            if (!hit.collider.bounds.Contains(edgePosition))
                            {
                               // Debug.Log("Edge of particles exceeds the object/wall.");
                                continue;
                            }
                        }
    
                        float intensity = baseIntensity;
                        float burnRate = 1f;
    
                        if (nearbyInflamable.CompareTag(flammableTag))
                        {
                            intensity += extraIntensity;
                            burnRate = 1f;
                        }
                        else
                        {
                            intensity *= 0.2f;
                            burnRate = 0.5f;
                        }
    
                        GameObject fireEffect = CreateFireEffect(nearbyInflamable.transform.position, intensity, nearbyInflamable.GetComponent<Renderer>().bounds.size);
                        nearbyInflamable.Ignite(intensity, burnRate, fireEffect);
                        newBurningObjects.Add(nearbyInflamable.gameObject);
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


    private GameObject CreateFireEffect(Vector3 position, float intensity, Vector3 objectSize)
    {
        GameObject fireEffect = Instantiate(fireEffectPrefab, position, Quaternion.identity);

        ParticleSystem fireParticleSystem = fireEffect.GetComponent<ParticleSystem>();
        ParticleSystem.MainModule main = fireParticleSystem.main;
        ParticleSystem.ShapeModule shape = fireParticleSystem.shape;

        main.startSize = Mathf.Max(0.2f, Mathf.Min(objectSize.y, 2 * objectSize.x));
        main.startLifetime = Mathf.Lerp(1f, 2f, intensity / 10f);

        fireEffect.transform.rotation = Quaternion.Euler(-90, 0, 0);

        fireParticleSystem.Play();

        return fireEffect;
    }
    private IEnumerator StartAlarmWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        fireAlarmAudioSource.Play();
    }

    
}
