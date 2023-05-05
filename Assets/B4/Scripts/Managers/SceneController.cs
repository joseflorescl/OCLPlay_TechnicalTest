using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    enum SceneName { Intro = 0, Minigame, Outro }

    [SerializeField] private float delayLoadOutro = 0.5f;

    private void OnEnable()
    {
        // Border condition: GameManager is NOT used in the Intro scene
        if (GameManager.Instance != null)
            GameManager.Instance.OnGameEnd += GameEndHandler;
    }    

    private void OnDisable()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.OnGameEnd -= GameEndHandler;
    }

    private void GameEndHandler()
    {
        StartCoroutine(LoadOutroRoutine());
    }

    IEnumerator LoadOutroRoutine()
    {
        yield return new WaitForSeconds(delayLoadOutro);
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
