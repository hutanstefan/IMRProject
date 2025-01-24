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
        // Check if the object is held and the button is pressed
        if (isHeld && activateParticlesAction.action.IsPressed())
        {
            if (!particleSystem.isPlaying)
                particleSystem.Play();

            // Launch a ray to detect burning flammable objects
            Ray ray = new Ray(particleSystem.transform.position, particleSystem.transform.forward);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, rayDistance))
            {
                InflamableObject inflamableObject = hit.collider.GetComponent<InflamableObject>();

                if (inflamableObject != null && inflamableObject.isBurning)
                {
                    inflamableObject.ReduceFireIntensity(extinguishRate * Time.deltaTime);
                    Debug.Log($"{hit.collider.gameObject.name} fire extinguished!");
                }
                else
                {
                    //Debug.Log("No burning object detected.");
                }
            }
            else
            {
                //Debug.Log("No object detected.");
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