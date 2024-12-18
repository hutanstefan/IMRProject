using UnityEngine;
using UnityEngine.Rendering;

public class FireEffectProximity : MonoBehaviour
{
    public GameObject postProcessingVolume; // Reference to the post-process volume
    public float activationDistance = 5f; // Distance within which the effect activates
    public Transform player; // Player's transform (e.g., camera rig)
    public string fireTag = "Fire"; // Tag assigned to fire GameObjects

    private bool isEffectActive = false;

    private void Start()
    {
        // Ensure the post-processing volume is initially disabled
        if (postProcessingVolume != null)
        {
            postProcessingVolume.SetActive(false); // Disable the volume initially
        }
        else
        {
            Debug.LogWarning("Post-Processing Volume is not assigned.");
        }

        if (player == null)
        {
            Debug.LogError("Player transform is not assigned.");
        }
    }

    private void Update()
    {
        if (postProcessingVolume == null || player == null)
            return;

        // Find the closest fire object
        GameObject closestFire = FindClosestFire();
        if (closestFire == null)
        {
            Debug.Log("No fire detected.");
            return;
        }

        // Calculate the distance to the closest fire
        float distance = Vector3.Distance(closestFire.transform.position, player.position);
        //Debug.Log($"Fire detected at distance: {distance}");
        // Enable or disable the effect based on distance
        if (distance <= activationDistance && !isEffectActive)
        {
            EnableEffect();
        }
        else if (distance > activationDistance && isEffectActive)
        {
            DisableEffect();
        }
    }

    private GameObject FindClosestFire()
    {
        GameObject[] fires = GameObject.FindGameObjectsWithTag(fireTag);
        GameObject closest = null;
        float minDistance = Mathf.Infinity;

        foreach (GameObject fire in fires)
        {
            float distance = Vector3.Distance(fire.transform.position, player.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closest = fire;
            }
        }

        return closest;
    }

    private void EnableEffect()
    {
        if (postProcessingVolume != null)
        {
            postProcessingVolume.SetActive(true); // Enable the entire volume
        }

        isEffectActive = true;
    }

    private void DisableEffect()
    {
        if (postProcessingVolume != null)
        {
            postProcessingVolume.SetActive(false); // Disable the entire volume
        }

        isEffectActive = false;
    }
}
