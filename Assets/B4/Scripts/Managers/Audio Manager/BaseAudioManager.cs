using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class BaseAudioManager : MonoBehaviour
{
    [SerializeField] protected AudioSource BGMAudioSource;
    [SerializeField] protected AudioSource SFXAudioSource;

    Coroutine audioRoutine;
    HashSet<AudioClip> clipsPlayedThisFrame;

    protected void Awake() => clipsPlayedThisFrame = new HashSet<AudioClip>();

    protected void LateUpdate() => clipsPlayedThisFrame.Clear();

    protected float PlayRandomMusic(AudioClip[] clips, bool loop)
    {
        StopAudioRoutine();
        var clip = GetRandomClip(clips);
        PlayBGMMusic(clip, loop);
        return clip.length;
    }

    protected void PlayRandomMusicWithDelay(AudioClip[] clips, bool loop, float delay, bool stopGameMusic)
    {
        if (stopGameMusic)
            StopGameMusic();
        audioRoutine = StartCoroutine(PlayRandomMusicWithDelayRoutine(clips, loop, delay));
    }

    IEnumerator PlayRandomMusicWithDelayRoutine(AudioClip[] clips, bool loop, float delay)
    {
        yield return new WaitForSeconds(delay);
        PlayRandomMusic(clips, loop);
    }


    protected void PlayRandomSoundWhitLoop(AudioClip[] clips, AudioSource audioSource)
    {
        if (clips == null || clips.Length == 0) return;
        var clip = GetRandomClip(clips);
        audioSource.clip = clip;
        audioSource.loop = true;
        audioSource.Play();
    }

    protected float PlayRandomSound(AudioClip[] clips, AudioSource audioSource, float volumeScale = 1f)
    {
        if (clips == null || clips.Length == 0) return 0f;
        var clip = GetRandomClip(clips);
        SFXPlayOneShot(clip, audioSource, volumeScale);
        return clip.length;
    }

    protected void PlayRandomSoundWithDelay(AudioClip[] clips, AudioSource audioSource, float delay, float volumeScale = 1f)
    {
        StartCoroutine(PlayRandomSoundWithDelayRoutine(clips, audioSource, delay, volumeScale));
    }

    IEnumerator PlayRandomSoundWithDelayRoutine(AudioClip[] clips, AudioSource audioSource, float delay, float volumeScale = 1f)
    {
        yield return new WaitForSeconds(delay);
        PlayRandomSound(clips, audioSource, volumeScale);
    }

    protected AudioClip GetRandomClip(AudioClip[] audioClips)
    {
        int randomIdx = Random.Range(0, audioClips.Length);
        return audioClips[randomIdx];
    }

    void SFXPlayOneShot(AudioClip clip, AudioSource audioSource, float volumeScale = 1f)
    {
        if (!clipsPlayedThisFrame.Contains(clip))
        {
            audioSource.PlayOneShot(clip, volumeScale);
            clipsPlayedThisFrame.Add(clip);
        }
    }


    protected void PlayBGMMusic(AudioClip clip, bool loop)
    {
        BGMAudioSource.loop = loop;
        BGMAudioSource.Stop();
        BGMAudioSource.clip = clip;
        BGMAudioSource.Play();
    }

    protected void StopAudioRoutine()
    {
        if (audioRoutine != null)
            StopCoroutine(audioRoutine);
    }

    protected void StopGameMusic()
    {
        StopAudioRoutine();
        BGMAudioSource.Stop();
    }
}