using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : BaseAudioManager
{
    [SerializeField] private AudioManagerData data;

    private void OnEnable()
    {
        GameManager.Instance.OnGameStart += GameStartHandler;
        GameManager.Instance.OnGrabButtonPressed += GrabButtonPressedHandler;
    }

   

    private void OnDisable()
    {
        GameManager.Instance.OnGameStart -= GameStartHandler;
        GameManager.Instance.OnGrabButtonPressed -= GrabButtonPressedHandler;
    }

    private void GameStartHandler(int currentLevel)
    {
        PlayRandomMusic(data.mainMusic, true);
    }

    private void GrabButtonPressedHandler()
    {
        PlayRandomSound(data.grabButtonPressed, SFXAudioSource);
    }

}
