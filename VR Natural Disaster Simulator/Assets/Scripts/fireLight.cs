using UnityEngine;

public class FlickerLight : MonoBehaviour
{
    public Light fireLight;
    public float minIntensity = 1.5f;
    public float maxIntensity = 2f;
    void Update()
    {
        if (fireLight != null)
        {
            fireLight.intensity = Random.Range(minIntensity, maxIntensity);
        }
    }
}
