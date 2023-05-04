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

    [Serializable]
    struct LevelConfiguration
    {
        public int treasuresCount;
        public float speed; // TODO: this value is not used yet: the background calculates the speed based on the currentLevel
    }

    [Range(1, 3)]
    [SerializeField] private int currentLevel = 1;
    [SerializeField] private LevelConfiguration[] levelConfigurations;

    int treasuresCatched;
    List<TreasureController> treasures = new();
    bool validateEndMinigame;

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
        //TODO: create timeline intro        
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
        //TODO: create timeline outro
        // TODO: esperar un delay antes de pasar a la sgte escena
        print("Outro Scene");
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
        treasuresCatched++;
        DestroyTreasure(treasure);
        // TODO: gatillar evento OnTreasureCatched que debería ser usado por el AudioMgr y VfxMgr
    }

    // TODO: identificar cuando el player NO agarra el tesoro para gatillar evento
    // TODO: el collider pickup del Player SOLO se deberia activar durante la animación de Grab en algunos frames
    // TODO: agregar la condición de término del juego para pasar a la escena del Outro

    public void UIShown()
    {
        // Only now is when the treasures should start appearing
        OnSpawningTreasures?.Invoke(levelConfigurations[currentLevel - 1].treasuresCount);
    }

    public void GrabButtonPressed()
    {
        OnGrabButtonPressed?.Invoke();
    }

    public void TreasuresSpawned()
    {
        validateEndMinigame = true;
    }

}
