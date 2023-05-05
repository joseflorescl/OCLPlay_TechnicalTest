using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public enum SceneName { Intro = 0, Minigame, Outro }

    private void OnEnable()
    {
        GameManager.Instance.OnGameEnd += GameEndHandler;
    }    

    private void OnDisable()
    {
        GameManager.Instance.OnGameEnd -= GameEndHandler;
    }

    private void GameEndHandler()
    {
        LoadOutro();
    }

    public void LoadMinigame()
    {
        // The scene must be in the scene list in the Build Settings window
        // This function is public because it is called from a Timeline signal
        SceneManager.LoadScene((int)SceneName.Minigame);
    }


    void LoadOutro()
    {
        SceneManager.LoadScene((int)SceneName.Outro);
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            int currentIndexScene = SceneManager.GetActiveScene().buildIndex;

            if (currentIndexScene == (int)SceneName.Intro)
                LoadMinigame();
            else 
                Quit();
        }
    }

    void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
