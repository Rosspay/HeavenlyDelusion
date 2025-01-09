using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public void Start()
    {
        Effects.FadeScreen(Color.black, 1, 0, 1);
    }

    public void onNewGameButtonClicked()
    {
        Effects.FadeScreen(Color.black, 0, 1, 1, () => SceneManager.LoadScene("Level1"));
    }

    public void onLoadGameButtonClicked()
    {
        if (PlayerPrefs.HasKey("Save"))
        {
            Effects.FadeScreen(Color.black, 0, 1, 1, () => SceneManager.LoadScene("Level" + PlayerPrefs.GetInt("Save").ToString()));
        }
        else
        {
            Effects.FadeScreen(Color.black, 0, 1, 1, () => SceneManager.LoadScene("Level1"));
        }
    }
    public void onExitGameButtonClicked()
    {
#if UNITY_STANDALONE
        Application.Quit();
#endif
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    public void onRestartGameButtonClicked()
    {
        Effects.FadeScreen(Color.black, 0, 1, 1, () => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex ));
        
    }

    public void onMenuButtonClicked()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
