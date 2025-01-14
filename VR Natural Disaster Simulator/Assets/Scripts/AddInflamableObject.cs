using UnityEngine;

public class AddInflamableToAllObjects : MonoBehaviour
{
    public string flammableTag = "Inflamable"; 

    void Start()
    {
        // Obține toate obiectele din scenă
        GameObject[] allObjects = FindObjectsOfType<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            if (obj.GetComponent<Camera>() != null || obj.GetComponent<MonoBehaviour>() != null)
            {
                continue;
            }

            if (obj.GetComponent<InflamableObject>() == null)
            {
                obj.AddComponent<InflamableObject>();

                if (obj.CompareTag(flammableTag))
                {
                   // Debug.Log($"InflamableObject a fost adăugat la obiectul cu tag Inflamable: {obj.name}");
                }
            }
        }
    }
}
