using System.Collections;
using UnityEngine;

public class ScoreEarthquake : MonoBehaviour
{
    public Transform player; // Referință la player
    public float checkRadius = 4.0f; // Raza de verificare
    public float duration = 30f; // Durata cutremurului
    public float delay = 5f; // Timpul de așteptare înainte de a începe verificarea
    private int score = 0; // Punctajul

    void Start()
    {
        // Începe verificarea după perioada de timp setată
        StartCoroutine(StartEarthquakeCheck());
    }

    // Coroutine pentru a începe verificarea după un anumit timp
    IEnumerator StartEarthquakeCheck()
    {
        yield return new WaitForSeconds(delay); // Așteaptă 5 secunde înainte de a începe verificarea
    
        // Verificăm cât timp durează cutremurul
        float timeSpentNearDoorFrame = 0f; // Timpul petrecut aproape de DoorFrame
        bool isNearDoorFrame = false;

        while (timeSpentNearDoorFrame < duration)
        {
            // Căutăm obiectele care au "DoorFrame" în nume
            GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();

            foreach (GameObject obj in allObjects)
            {
                if (obj.name.ToLower().Contains("doorframe"))
                {
                    Vector3 playerPos = player.position;
                    Vector3 doorPos = obj.transform.position;

                    // Verificăm dacă player-ul este în raza permisă pe fiecare axă
                    bool isWithinX = Mathf.Abs(playerPos.x - doorPos.x) <= checkRadius;
                    bool isWithinY = Mathf.Abs(playerPos.y - doorPos.y) <= checkRadius + 3f;
                    bool isWithinZ = Mathf.Abs(playerPos.z - doorPos.z) <= checkRadius;

                    if (isWithinX && isWithinY && isWithinZ)
                    {
                        Debug.Log("Player-ul este aproape de un DoorFrame!");
                        isNearDoorFrame = true;
                        break;
                    }
                }
            }

            // Dacă player-ul a stat lângă DoorFrame, crește timpul petrecut
            if (isNearDoorFrame)
            {
                timeSpentNearDoorFrame += Time.deltaTime; // Crește timpul cu timpul trecut în acest frame  
            }

            // Dacă player-ul nu este aproape de DoorFrame, resetăm timpul
            else
            {
                timeSpentNearDoorFrame = 0f;
            }

            yield return null; // Așteaptă până la următorul frame
        }

        // La final, calculăm scorul (îți ofer un exemplu simplu de scor)
        CalculateScore(timeSpentNearDoorFrame);
    }

    // Calcularea scorului pe baza timpului petrecut lângă DoorFrame
    // Calcularea scorului pe baza timpului petrecut lângă DoorFrame
    private void CalculateScore(float timeSpent)
    {
        // Evităm împărțirea la zero (în caz că durata e 0 dintr-o eroare)
        if (duration <= 0)
        {
            Debug.LogError("Durata cutremurului este 0! Nu se poate calcula scorul.");
            return;
        }

        // Scorul este procentajul de timp petrecut sub cadrul ușii
        float scorePercentage = (timeSpent / duration) * 100f;

        // Rotunjim scorul la un număr întreg
        score = Mathf.FloorToInt(scorePercentage);

        Debug.Log($"Scorul obținut: {score}");
    }

}
