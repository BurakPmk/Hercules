using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class MainMenu : MonoBehaviour
{
    public Button level2;
    private void Update()
    {
        if(PlayerPrefs.GetInt("level2")>0)
        {
            level2.interactable = true;
        }
    }
    public void reStart()
    {

        Application.LoadLevel(Application.loadedLevel);
    }
    public void PlayGame()
    {
        SceneManager.LoadScene("level1");

    }

    public void QuitGame()
    {

        Application.Quit();
    }

    public void PlayLevel2()
    {
        SceneManager.LoadScene("level2");
    }
    public void backMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    

}
