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
    private float numberOfStartedFire=1;
    public AudioSource fireAlarmAudioSource;
    public bool firstFireStarted=false;

    private List<InflamableObject> burningObjects = new List<InflamableObject>(); 

    void Start()
    {
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
                    fireAlarmAudioSource.Play();
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

                         // Raycast pentru a verifica obstacolele
                        RaycastHit hit;
                          if (Physics.Raycast(burningObject.transform.position, direction, out hit, distance))
                         {
                     // Dacă obiectul intersectat conține "wall" în numele său, nu dăm foc acestui obiect
                            if (hit.collider.gameObject.name.ToLower().Contains("wall"))
                            {
                                 Debug.Log($"Obstacol detectat: {hit.collider.gameObject.name}. Nu pot da foc la {nearbyInflamable.gameObject.name}.");
                                continue;
                             }
                             ////partea asta nu sunt sigur ca merge bine mai trebuie testat
                              Vector3 edgePosition = burningObject.transform.position + direction.normalized * distance;
                              if (!hit.collider.bounds.Contains(edgePosition))
                             {
                                   Debug.Log("Marginea particulelor depășește obiectul/peretele.");
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

    // Adjust the start size and lifetime based on the intensity and object size
    main.startSize = Mathf.Max(0.2f, Mathf.Min(objectSize.y, 2 * objectSize.x));
    main.startLifetime = Mathf.Lerp(1f, 2f, intensity / 10f);

    // Ensure the fire effect always faces upwards
    fireEffect.transform.rotation = Quaternion.Euler(-90, 0, 0);

    fireParticleSystem.Play();

    return fireEffect;
}
}
