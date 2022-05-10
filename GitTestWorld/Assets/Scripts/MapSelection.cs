using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapSelection : MonoBehaviour
{
    //public PauseMenu pauseMenu;
    
    public void SelectCity()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        Time.timeScale = 1f;
        //pauseMenu.gameIsPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        WhatMapIsSelected.isIndoorMap = false;
    }

    public void SelectIndoor()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
        Time.timeScale = 1f;
        //pauseMenu.gameIsPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        WhatMapIsSelected.isIndoorMap = true;
    }
}
