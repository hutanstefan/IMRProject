using UnityEngine;

public class FlickerAndGrow : MonoBehaviour
{
    public Light fireLight;                    
    public float minIntensity = 1.5f;           
    public float maxIntensity = 2f;            
    public float sizeGrowthRate = 0.1f;        
    public float maxParticleSize = 5f;          
    public float areaGrowthRate = 0.05f;   
    public float maxX, maxY;

    private ParticleSystem fireParticles;       
    private ParticleSystem.MainModule mainModule;
    private ParticleSystem.ShapeModule shapeModule;

    void Start()
    {
        fireParticles = GetComponentInParent<ParticleSystem>();
        if (fireParticles != null)
        {
            mainModule = fireParticles.main;
            shapeModule = fireParticles.shape;
            maxX=shapeModule.scale.x*2;
            maxY=shapeModule.scale.y*2;
        }
        else
        {
            Debug.LogError("Sistemul de particule nu a fost gasit in parinti!");
        }
    }

    void Update()
    {
        if (fireLight != null)
        {
            fireLight.intensity = Random.Range(minIntensity, maxIntensity);
        }

        if (fireParticles != null && mainModule.startSize.constant < maxParticleSize)
        {
            mainModule.startSize = new ParticleSystem.MinMaxCurve(mainModule.startSize.constant + sizeGrowthRate * Time.deltaTime);
        }

        if (fireParticles != null && (shapeModule.scale.x < maxX || shapeModule.scale.y< maxY))
        {
           float newScaleX = Mathf.Min(shapeModule.scale.x + areaGrowthRate * Time.deltaTime, maxX);
           float newScaleY = Mathf.Min(shapeModule.scale.y + areaGrowthRate * Time.deltaTime, maxY);
           shapeModule.scale = new Vector3(newScaleX, newScaleY, shapeModule.scale.z);
        }
    }
}
