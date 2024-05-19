using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string newGameSceneName = "BaseScene";
    
    public AudioClip backgroundMusic;
    public AudioSource mainChannel;
    
    private void Start()
        {
            mainChannel.PlayOneShot(backgroundMusic);
        }

    public void StartNewGame()
    {
        SceneManager.LoadScene(newGameSceneName);
        mainChannel.Stop();
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
      UnityEditor.EditorApplication.isPlaying = false;  
#else
    Application.Quit();
#endif
    }
}
