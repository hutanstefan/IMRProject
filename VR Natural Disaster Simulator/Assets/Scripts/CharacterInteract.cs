using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInteract : MonoBehaviour
{
    public GameObject startMenu;
    public GameObject levelSelect;
    private Canvas startMenuCanvas;
    private Canvas levelSelectCanvas;

    void Start()
    {
        startMenuCanvas = startMenu.GetComponent<Canvas>();
        levelSelectCanvas = levelSelect.GetComponent<Canvas>();

        startMenuCanvas.enabled = true;
        levelSelectCanvas.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
         if (Input.GetKeyDown(KeyCode.Return))
        {
            startMenuCanvas.enabled = !startMenuCanvas.enabled;
            levelSelectCanvas.enabled = !levelSelectCanvas.enabled;
        }
    }
}
