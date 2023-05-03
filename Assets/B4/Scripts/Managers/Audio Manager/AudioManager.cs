using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : BaseAudioManager
{
    [SerializeField] private AudioManagerData data;

    private void OnEnable()
    {
        GameManager.Instance.OnGameStart += GameStartHandler;
    }

    

    private void OnDisable()
    {
        GameManager.Instance.OnGameStart -= GameStartHandler;
    }

    private void GameStartHandler()
    {
        PlayRandomMusic(data.mainMusic, true);
    }

}
