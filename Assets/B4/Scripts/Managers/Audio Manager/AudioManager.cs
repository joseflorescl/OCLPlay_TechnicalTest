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
        GameManager.Instance.OnTreasureCatched += TreasureCatchedHandler;
        GameManager.Instance.OnTreasureUncatched += TreasureUncatchedHandler;
        GameManager.Instance.OnGrabbing += GrabbingHandler;
    }
    

    private void OnDisable()
    {
        GameManager.Instance.OnGameStart -= GameStartHandler;
        GameManager.Instance.OnGrabButtonPressed -= GrabButtonPressedHandler;
        GameManager.Instance.OnTreasureCatched -= TreasureCatchedHandler;
        GameManager.Instance.OnTreasureUncatched -= TreasureUncatchedHandler;
        GameManager.Instance.OnGrabbing -= GrabbingHandler;
    }

    private void GameStartHandler(int currentLevel)
    {
        PlayRandomMusic(data.mainMusic, true);
    }

    private void GrabButtonPressedHandler()
    {
        PlayRandomSound(data.grabButtonPressed, SFXAudioSource);
    }

    private void TreasureCatchedHandler(TreasureController obj)
    {
        PlayRandomSound(data.treasurePickup, SFXAudioSource, data.volumeScaleTreasurePickup);
        PlayRandomSound(data.voiceArloTreasureCatched, SFXAudioSource, data.volumeScaleArloVoice);
    }

    private void TreasureUncatchedHandler()
    {
        PlayRandomSound(data.voiceArloTreasureUncatched, SFXAudioSource);
    }

    private void GrabbingHandler()
    {
        PlayRandomSound(data.arloGrabbing, SFXAudioSource);
    }

}
