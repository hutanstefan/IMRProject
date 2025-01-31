using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class FireExtinguisher : MonoBehaviour
{
    [SerializeField] private ParticleSystem particleSystem; // Reference to the Particle System
    [SerializeField] private InputActionReference activateParticlesAction; // Input Action reference
    [SerializeField] private float rayDistance = 100f; // Distance of the ray
    [SerializeField] private float extinguishRate = 10f; // Rate at which the fire is extinguished

    private XRGrabInteractable grabInteractable;

    private bool isHeld = false; // To track if the object is currently grabbed

    private void Awake()
    {
        // Get the XRGrabInteractable component
        grabInteractable = GetComponent<XRGrabInteractable>();

        if (grabInteractable == null)
        {
            Debug.LogError("XRGrabInteractable component is missing!");
        }
        
    }

    private void OnEnable()
    {
        // Subscribe to XRGrabInteractable events
        grabInteractable.selectEntered.AddListener(OnGrab);
        grabInteractable.selectExited.AddListener(OnRelease);

        // Enable the input action
        activateParticlesAction.action.Enable();
    }

    private void OnDisable()
    {
        // Unsubscribe from XRGrabInteractable events
        grabInteractable.selectEntered.RemoveListener(OnGrab);
        grabInteractable.selectExited.RemoveListener(OnRelease);

        // Disable the input action
        activateParticlesAction.action.Disable();
    }

    private void Update()
    {
        if (isHeld && activateParticlesAction.action.IsPressed())
        {
            if (!particleSystem.isPlaying)
                particleSystem.Play();

            Vector3 origin = particleSystem.transform.position;
            Vector3 forward = particleSystem.transform.forward;
            float coneAngle = 30f; // Angle of the cone in degrees
            float radius = rayDistance * Mathf.Tan(coneAngle * Mathf.Deg2Rad);

            Collider[] hits = Physics.OverlapSphere(origin, radius);

            foreach (Collider hit in hits)
            {
                Vector3 directionToHit = hit.transform.position - origin;
                float angleToHit = Vector3.Angle(forward, directionToHit);

                if (angleToHit <= coneAngle)
                {
                    Ray ray = new Ray(origin, directionToHit);
                    RaycastHit[] raycastHits = Physics.RaycastAll(ray, rayDistance);

                    foreach (RaycastHit raycastHit in raycastHits)
                    {
                        InflamableObject inflamableObject = raycastHit.collider.GetComponent<InflamableObject>();

                        if (inflamableObject != null && inflamableObject.isBurning)
                        {
                            inflamableObject.ReduceFireIntensity(extinguishRate * 0.1f * Time.deltaTime); // Reduce intensity slower
                        }
                    }
                }
            }
        }
        else
        {
            if (particleSystem.isPlaying)
                particleSystem.Stop();
        }
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        isHeld = true;
    }

    private void OnRelease(SelectExitEventArgs args)
    {
        isHeld = false;
        particleSystem.Stop(); // Ensure particles stop when released
    }
    
}