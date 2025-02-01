using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuInteract : MonoBehaviour
{
    public GameObject startMenu;
    public GameObject levelSelect;
    public GameObject earthquakeTutorial;
    public GameObject[] earthquakeTutorialPages;
    public GameObject fireTutorial;
    public GameObject[] fireTutorialPages;
    public GameObject arrowPointer;
    public GameObject playButton;
    public GameObject leftFireButton;
    public GameObject rightFireButton;
    public GameObject leftEarthquakeButton;
    public GameObject rightEarthquakeButton;
    private Canvas startMenuCanvas;
    private Canvas levelSelectCanvas;
    private Canvas earthquakeTutorialCanvas;
    private Canvas fireTutorialCanvas;
    private bool levelWasSelected;
    private string currentDifficulty;
    private string currentLevel;
    private Transform buttonTransform;
    private AudioSource audioSource;
    private int currentPage = 0;

    void Start()
    {
        startMenuCanvas = startMenu.GetComponent<Canvas>();
        levelSelectCanvas = levelSelect.GetComponent<Canvas>();
        earthquakeTutorialCanvas = earthquakeTutorial.GetComponent<Canvas>();
        fireTutorialCanvas = fireTutorial.GetComponent<Canvas>();

        audioSource = GetComponent<AudioSource>();

        startMenuCanvas.enabled = true;
        levelSelectCanvas.enabled = false;
        earthquakeTutorialCanvas.enabled = false;
        fireTutorialCanvas.enabled = false;

        arrowPointer.SetActive(false);
        playButton.SetActive(false);
        levelWasSelected = false;
    }

    public void enterLevelSelect()
    {
        startMenuCanvas.enabled = false;
        levelSelectCanvas.enabled = true;
    }

    public void showEarthquakeTutorial()
    {
        levelSelectCanvas.enabled = false;
        earthquakeTutorialCanvas.enabled = true;
        earthquakeTutorialPages[0].SetActive(true);
        for(int i = 1; i < earthquakeTutorialPages.Length; i++)
        {
            earthquakeTutorialPages[i].SetActive(false);
        }
        leftEarthquakeButton.SetActive(false);
        rightEarthquakeButton.SetActive(true);
    }

    public void turnLeftEarthquake()
    {
        earthquakeTutorialPages[currentPage].SetActive(false);
        currentPage--;
        earthquakeTutorialPages[currentPage].SetActive(true);
        if(currentPage == 0)
        {
            leftEarthquakeButton.SetActive(false);
        }
        if(currentPage != earthquakeTutorialPages.Length - 1 && !rightEarthquakeButton.activeSelf)
        {
            rightEarthquakeButton.SetActive(true);
        }
    }
    
    public void turnRightEarthquake()
    {
        earthquakeTutorialPages[currentPage].SetActive(false);
        currentPage++;
        earthquakeTutorialPages[currentPage].SetActive(true);
        if(currentPage == earthquakeTutorialPages.Length - 1)
        {
            rightEarthquakeButton.SetActive(false);
        }
        if(currentPage != 0 && !leftEarthquakeButton.activeSelf)
        {
            leftEarthquakeButton.SetActive(true);
        }
    }

    public void showFireTutorial()
    {
        levelSelectCanvas.enabled = false;
        fireTutorialCanvas.enabled = true;
        fireTutorialPages[0].SetActive(true);
        for(int i = 1; i < fireTutorialPages.Length; i++)
        {
            fireTutorialPages[i].SetActive(false);
        }
        leftFireButton.SetActive(false);
        rightFireButton.SetActive(true);
    }

    public void turnLeftFire()
    {
        fireTutorialPages[currentPage].SetActive(false);
        currentPage--;
        fireTutorialPages[currentPage].SetActive(true);
        if(currentPage == 0)
        {
            leftFireButton.SetActive(false);
        }
        if(currentPage != fireTutorialPages.Length - 1 && !rightFireButton.activeSelf)
        {
            rightFireButton.SetActive(true);
        }
    }
    
    public void turnRightFire()
    {
        fireTutorialPages[currentPage].SetActive(false);
        currentPage++;
        fireTutorialPages[currentPage].SetActive(true);
        if(currentPage == fireTutorialPages.Length - 1)
        {
            rightFireButton.SetActive(false);
        }
        if(currentPage != 0 && !leftFireButton.activeSelf)
        {
            leftFireButton.SetActive(true);
        }
    }

    public void closeTutorial()
    {
        levelSelectCanvas.enabled = true;
        earthquakeTutorialCanvas.enabled = false;
        fireTutorialCanvas.enabled = false;
        currentPage = 0;
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
