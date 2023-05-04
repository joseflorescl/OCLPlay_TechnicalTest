using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New AudioManager Data", menuName = "B4/Audio Manager Data")]
public class AudioManagerData : ScriptableObject
{
    [Header("BGM Sounds")]
    public AudioClip[] mainMusic;    

    [Space(10)]
    [Header("Player SFX Sounds")]
    public AudioClip[] arloGrabbing;
    
    [Space(10)]
    [Header("Gameplay SFX Sounds")]
    public AudioClip[] treasurePickup;
    public AudioClip[] grabButtonPressed;

    [Space(10)]
    [Header("Voice Sounds")]
    public AudioClip[] voiceArloTreasureCatched;
    public AudioClip[] voiceArloTreasureUncatched;

    [Space(10)]
    [Header("Settings")]
    public float volumeScaleTreasurePickup = 0.5f;
    public float volumeScaleArloVoice = 2f;
}