using UnityEngine;

public class FlickerAndGrow : MonoBehaviour
{
    public Light fireLight;                    
    public float minIntensity = 1.5f;           
    public float maxIntensity = 2f;            
    public float sizeGrowthRate = 0.1f;        
    public float maxParticleSize = 5f;          
    public float areaGrowthRate = 0.05f;       
    public float maxAreaSize = 3f;            

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

        if (fireParticles != null && shapeModule.scale.x < maxAreaSize)
        {
            float newScale = shapeModule.scale.x + areaGrowthRate * Time.deltaTime;
            float newScaleY = shapeModule.scale.y + areaGrowthRate * Time.deltaTime;
            shapeModule.scale = new Vector3(newScale, newScaleY, newScale);
        }
    }
}
