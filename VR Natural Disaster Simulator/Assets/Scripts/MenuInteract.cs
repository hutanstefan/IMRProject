using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuInteract : MonoBehaviour
{
    public GameObject startMenu;
    public GameObject levelSelect;
    public GameObject arrowPointer;
    public GameObject playButton;
    private Canvas startMenuCanvas;
    private Canvas levelSelectCanvas;
    private bool levelWasSelected;
    private string currentDifficulty;
    private string currentLevel;
    private Transform buttonTransform;
    private AudioSource audioSource;

    void Start()
    {
        startMenuCanvas = startMenu.GetComponent<Canvas>();
        levelSelectCanvas = levelSelect.GetComponent<Canvas>();

        audioSource = GetComponent<AudioSource>();

        startMenuCanvas.enabled = true;
        levelSelectCanvas.enabled = false;

        arrowPointer.SetActive(false);
        playButton.SetActive(false);
        levelWasSelected = false;
    }

    public void enterLevelSelect()
    {
        startMenuCanvas.enabled = false;
        levelSelectCanvas.enabled = true;
    }

    public void getButtonPosition(Transform buttonPos)
    {
        buttonTransform = buttonPos;
    }

    public void setPickedScenario(string scenario)
    {
        currentLevel = scenario;
    }

    public void setPickedDifficulty(string difficulty)
    {
        currentDifficulty = difficulty;
    }

    public void placeArrowMarker()
    {
        arrowPointer.transform.position = buttonTransform.position;

        if(levelWasSelected == false)
        {
            arrowPointer.SetActive(true);
            playButton.SetActive(true);
            levelWasSelected = true;
        }

        audioSource.Play();
    }

    public void startLevel()
    {
        PlayerPrefs.SetString("difficulty", currentDifficulty);
        PlayerPrefs.Save();
        SceneManager.LoadScene(currentLevel, LoadSceneMode.Single);
    }
    
    public void closeGame()
    {
        Application.Quit();
    }

}
