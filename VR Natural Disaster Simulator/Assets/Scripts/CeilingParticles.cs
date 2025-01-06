using UnityEngine;

public class CeilingParticles : MonoBehaviour
{
    private ParticleSystem particleSystem;
    private ParticleSystem.MainModule mainModule;
    private Collider colliderArea;
    private Vector3 globalPosition;

    public float speed = 1f;
    public float lifetime = 3f;
    public int particlesPerSecond = 10; 

    void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();
        mainModule = particleSystem.main;
        colliderArea = GetComponent<Collider>();  

        Vector3 globalPosition = colliderArea.transform.position;

        particleSystem.Stop();
    }

    public void StartParticles()
    {
        particleSystem.Play();

        InvokeRepeating("GenerateRandomParticle", 0f, 1f / particlesPerSecond);
    }

    void GenerateRandomParticle()
    {
        Bounds bounds = colliderArea.bounds;

        Vector3 randomLocalPosition = new Vector3(
            Random.Range(-bounds.extents.x, bounds.extents.x),
            globalPosition.y,
            Random.Range(-bounds.extents.z, bounds.extents.z)
        );

        Vector3 finalPosition = globalPosition + randomLocalPosition;
    

        ParticleSystem.EmitParams emitParams = new ParticleSystem.EmitParams
        {
            position = finalPosition
        };

        mainModule.startSpeed = speed;
        mainModule.startLifetime = lifetime;

        particleSystem.Emit(emitParams, 1);
    }


    public void StopParticles()
    {
        CancelInvoke("GenerateRandomParticle");
        particleSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
    }
}
