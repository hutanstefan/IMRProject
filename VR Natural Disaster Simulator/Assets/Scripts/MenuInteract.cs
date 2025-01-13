using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuInteract : MonoBehaviour
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

    public void enterLevelSelect()
    {
        startMenuCanvas.enabled = false;
        levelSelectCanvas.enabled = true;
    }

    public void startLevel(string level)
    {
        SceneManager.LoadScene(level, LoadSceneMode.Single);
    }
    
    public void closeGame()
    {
        Application.Quit();
    }

}
