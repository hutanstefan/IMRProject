using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseMenu;
    public InputActionProperty input;
    public Transform playerPosition;
    public float spawnDistance;
    
    void Update()
    {
        if(input.action.WasPressedThisFrame())
        {
            pauseMenu.SetActive(!pauseMenu.activeSelf);
            pauseMenu.transform.position = playerPosition.position + new Vector3(playerPosition.forward.x, 0, playerPosition.forward.z).normalized * spawnDistance;
        }
    }

    public void ExitLevel()
    {
        SceneManager.LoadScene("StartScene", LoadSceneMode.Single);
    }
}
