using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class Breakable : MonoBehaviour
{
    [SerializeField] GameObject intactObj;
    [SerializeField] GameObject brokenObj;
    [SerializeField] GameObject onBreakCollider;

    BoxCollider bc;

    private bool isBroken = false; 

    private void Awake()
    {
        intactObj.SetActive(true);
        brokenObj.SetActive(false);

        bc = GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Breaker") && !isBroken)
        {
            Break();
        }
    }

    private void Break()
    {
        isBroken = true; 

        brokenObj.transform.position = intactObj.transform.position;
        brokenObj.transform.rotation = intactObj.transform.rotation;

        intactObj.SetActive(false);
        brokenObj.SetActive(true);

        // bc.enabled = false;
    }
}
