using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public void onNewGameButtonClicked()
    {
        SceneManager.LoadScene("Level1");
    }

    public void onLoadGameButtonClicked()
    {
        SceneManager.LoadScene("Level" + PlayerPrefs.GetInt("Save").ToString());
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
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex );
    }

    public void onMenuButtonClicked()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
