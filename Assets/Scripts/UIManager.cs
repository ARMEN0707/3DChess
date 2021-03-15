using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject settingMenu;

    public void StartGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void SettingMenu()
    {
        mainMenu.SetActive(false);
        settingMenu.SetActive(true);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void BackMainMenu()
    {
        settingMenu.SetActive(false);
        mainMenu.SetActive(true);
    }
}
