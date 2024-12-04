using UnityEngine;

public class AddInflamableToAllObjects : MonoBehaviour
{
    public string flammableTag = "Inflamable"; // Tag-ul specific pentru obiectele inflamabile

    void Start()
    {
        // Obține toate obiectele din scenă
        GameObject[] allObjects = FindObjectsOfType<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            // Sari peste camere și obiectele care conțin componente de tip MonoBehaviour (scripturi)
            if (obj.GetComponent<Camera>() != null || obj.GetComponent<MonoBehaviour>() != null)
            {
                continue;
            }

            // Adaugă componenta InflamableObject dacă nu există deja
            if (obj.GetComponent<InflamableObject>() == null)
            {
                obj.AddComponent<InflamableObject>();

                // Dacă obiectul are tag-ul Inflamable, afișează mesaj în consolă
                if (obj.CompareTag(flammableTag))
                {
                    Debug.Log($"InflamableObject a fost adăugat la obiectul cu tag Inflamable: {obj.name}");
                }
            }
        }
    }
}
