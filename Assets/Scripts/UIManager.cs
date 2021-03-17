using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject settingMenu;
    public GameObject pauseMenu;
    public GameObject replaceChessMenu;
    public GameObject winMenu;
    public Text textWin;

    public static bool isPause;

    private void Awake()
    {
        isPause = false;
    }


    public void StartGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void SettingMenu()
    {
        mainMenu.SetActive(!mainMenu.activeInHierarchy);
        settingMenu.SetActive(!settingMenu.activeInHierarchy);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void PauseMenu()
    {
        isPause = !isPause;
        mainMenu.SetActive(!mainMenu.activeInHierarchy);
        pauseMenu.SetActive(!pauseMenu.activeInHierarchy);
    }

    public void SettingMenuOnPause()
    {
        pauseMenu.SetActive(!pauseMenu.activeInHierarchy);
        settingMenu.SetActive(!settingMenu.activeInHierarchy);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void BackMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void ReplaceChessMenu()
    {        
        replaceChessMenu.SetActive(!replaceChessMenu.activeInHierarchy);
        mainMenu.SetActive(!mainMenu.activeInHierarchy);
        isPause = !isPause;
    }

    public void WinMenu(bool isWhiteWin)
    {
        if (!isWhiteWin)
        {
            textWin.text = "BLACK WIN!";
        }
        mainMenu.SetActive(false);
        winMenu.SetActive(true);
    }
}
