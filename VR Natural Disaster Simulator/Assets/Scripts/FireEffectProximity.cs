using UnityEngine;
using UnityEngine.Rendering;

public class FireEffectProximity : MonoBehaviour
{
    public GameObject postProcessingVolume; // Reference to the post-process volume
    public float activationDistance = 2f; // Distance within which the effect activates
    public Transform player; 
    public string fireTag = "Fire"; 
    private float maxHeightDifference=2f;

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
        GameObject closestFire = FindClosestFire();
        if (closestFire == null)
        {
            //Debug.Log("No fire detected.");
            return;
        }
           RaycastHit hit;
        Vector3 directionToFire1 = closestFire.transform.position - player.position;
        if (Physics.Raycast(player.position, directionToFire1, out hit, directionToFire1.magnitude))
         {
            if (hit.collider != null && hit.collider.gameObject.name.Contains("Wall"))
            {
            //Debug.Log("Wall detected between player and fire. Disabling effect.");
            DisableEffect(); 
            return; 
            }
         }
        float heightDifference = Mathf.Abs(player.position.y - closestFire.transform.position.y);
        if (heightDifference <= maxHeightDifference)
        {
            float distance = Vector3.Distance(new Vector3(player.position.x, closestFire.transform.position.y, player.position.z), 
                                              new Vector3(closestFire.transform.position.x, closestFire.transform.position.y, closestFire.transform.position.z));

            Vector3 directionToFire = (closestFire.transform.position - player.position).normalized;
            float dotProduct = Vector3.Dot(player.forward, directionToFire);
            if (((distance <= activationDistance && dotProduct > 0.5f) || distance<=1f) && !isEffectActive)
            {
                EnableEffect();
            }
            else if ((distance > activationDistance || dotProduct <= 0.5f) && isEffectActive)
            {
                DisableEffect();
            }
        }
        else if(isEffectActive)
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
