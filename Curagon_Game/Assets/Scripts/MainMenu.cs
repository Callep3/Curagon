using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Button play;
    public Button quit;

    private void Start()
    {
        play.onClick.AddListener(OnPlayClick);
        quit.onClick.AddListener(OnQuitClick);
    }

    void OnPlayClick()
    {
        SceneManager.LoadScene("GameScene");
        Debug.Log("Clicking");
    }

    void OnQuitClick()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif
        //Application.Quit();
    }
}
