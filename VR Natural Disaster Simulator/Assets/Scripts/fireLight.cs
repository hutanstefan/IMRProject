using UnityEngine;

public class FlickerAndGrow : MonoBehaviour
{
    public Light fireLight;                     
    public float minIntensity = 1.5f;            
    public float maxIntensity = 2f;             
    public float sizeGrowthRate = 0.01f;        
    public float maxParticleSizeMultiplier = 1.01f; 
    public float areaGrowthRate = 0.05f;        

    private ParticleSystem fireParticles;       
    private ParticleSystem.MainModule mainModule;
    private ParticleSystem.ShapeModule shapeModule;
    private Vector3 initialShapeScale;

    void Start()
    {
        fireParticles = GetComponentInParent<ParticleSystem>();
        if (fireParticles != null)
        {
            mainModule = fireParticles.main;
            shapeModule = fireParticles.shape;
            initialShapeScale = shapeModule.scale;
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

        if (fireParticles != null)
        {
            Vector3 maxScale = initialShapeScale * maxParticleSizeMultiplier;
            float newScaleX = Mathf.Min(shapeModule.scale.x + areaGrowthRate * Time.deltaTime, maxScale.x);
            float newScaleY = Mathf.Min(shapeModule.scale.y + areaGrowthRate * Time.deltaTime, maxScale.y);
            float newScaleZ = Mathf.Min(shapeModule.scale.z + areaGrowthRate * Time.deltaTime, maxScale.z);
            shapeModule.scale = new Vector3(newScaleX, newScaleY, newScaleZ);
        }
    }
}
