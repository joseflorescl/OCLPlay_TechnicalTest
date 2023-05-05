using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Singleton class
    private static GameManager instance = null;
    public static GameManager Instance => instance;

    enum GameState { Intro, MainGame, Outro }

    GameState currentState;

    GameState CurrentState
    {
        get => currentState;
        set
        {
            // Aquí pueden ir los métodos Exit de cada estado

            currentState = value;

            switch (currentState)
            {
                case GameState.Intro:
                    IntroEnter();
                    break;
                case GameState.MainGame:
                    MainGameEnter();
                    break;
                case GameState.Outro:
                    OutroEnter();
                    break;
            }
        }
    }


    // Events - Naming Convention
    //   OnClosing: a close event that is raised before a window is closed
    //   OnClosed: one that is raised after the window is closed 
    public event Action<int> OnGameStart;
    public event Action<int> OnSpawningTreasures;
    public event Action<TreasureController> OnTreasureCreated;
    public event Action OnGrabButtonPressed;
    public event Action<TreasureController> OnTreasureCatched;
    public event Action OnTreasureUncatched;
    public event Action OnGrabbing;
    public event Action OnGameEnd;

    [Range(1, 3)]
    [SerializeField] private int currentLevel = 1;
    [SerializeField] private int[] treasuresPerLevel;
    [Range(0, 1)]
    [SerializeField] private float moreDifficultyPerLevel = 0.2f;
    [SerializeField] private float delayBeforeOutro = 1f;

    int treasuresCatched;
    List<TreasureController> treasures = new();
    bool validateEndMinigame;
    bool treasureWasCatched;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }
    }


    private void Start()
    {
        CurrentState = GameState.Intro;
    }



    void IntroEnter()
    {
        CurrentState = GameState.MainGame;
    }

    void MainGameEnter()
    {
        treasuresCatched = 0;
        validateEndMinigame = false;
        OnGameStart?.Invoke(currentLevel);
    }


    void OutroEnter()
    {
        StartCoroutine(OutroRoutine());
    }

    IEnumerator OutroRoutine()
    {        
        yield return new WaitForSeconds(delayBeforeOutro);
        OnGameEnd?.Invoke();
    }

    public void BackgroundCreated()
    {
        
    }

    public void TreasureCreated(TreasureController treasure)
    {
        treasures.Add(treasure);
        OnTreasureCreated?.Invoke(treasure);
    }

    public void TreasureDisappears(TreasureController treasure)
    {
        DestroyTreasure(treasure);
    }

    void DestroyTreasure(TreasureController treasure)
    {
        treasures.Remove(treasure);
        Destroy(treasure.gameObject);

        if (validateEndMinigame && treasures.Count == 0)
        {
            CurrentState = GameState.Outro;
        }
    }

    public void TreasureCatched(TreasureController treasure)
    {
        treasureWasCatched = true;
        treasuresCatched++;
        DestroyTreasure(treasure);        
        OnTreasureCatched?.Invoke(treasure);
    }            

    public void UIShown()
    {
        // Only now is when the treasures should start appearing
        OnSpawningTreasures?.Invoke(treasuresPerLevel[currentLevel - 1]);
    }

    public void GrabButtonPressed()
    {
        OnGrabButtonPressed?.Invoke();
    }

    public void TreasuresSpawned()
    {
        validateEndMinigame = true;
    }

    public void GrabStart()
    {
        treasureWasCatched = false;
        OnGrabbing?.Invoke();
    }

    public void GrabEnd()
    {
        if (!treasureWasCatched)
        {
            OnTreasureUncatched?.Invoke();
        }
    }

    public float GetSpeedFactorByLevel(int level)
    {
        // For example: speed == 1 for level 1, speed == 1.2 for level 2
        return 1f + (level - 1) * moreDifficultyPerLevel;
    }

}
