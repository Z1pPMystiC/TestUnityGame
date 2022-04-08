using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenuUI;
    public GameObject mapSelectionUI;
    // public GameObject settingsUI;
    public void PlayGame()
    {
        mainMenuUI.SetActive(false);
        mapSelectionUI.SetActive(true);
        // settingsUI.SetActive(false);

        /*SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        Time.timeScale = 1f;
        pauseMenu.gameIsPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;*/
    }

    public void QuitGame()
    {
        Debug.Log("Quit the game");
        Application.Quit();
    }
}
