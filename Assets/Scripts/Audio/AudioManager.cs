using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    //通用的两个Source
    public AudioSource BGMSource;

    public AudioSource FXSource;

    [SerializeField]private MultiAudioDefination characterHitAudio;

    public void PlayGenralBGM(AudioClip clip)
    {
        BGMSource.clip = clip;
        BGMSource.Play();
    }

    public void PlayGenralFX(AudioClip clip)
    {
        FXSource.clip = clip;
        FXSource.Play();
    }

    public void PlayClipBySpecifiedSource(AudioClip audioClip,AudioSource audioSource)
    {
        audioSource.clip = audioClip;
        audioSource.Play();
    }

    public void PlayCharacterHit(AudioSource audioSource)
    {
        audioSource.clip = GameManager.Instance.gameConfig.GetRandomHitAudioClip();
        audioSource.Play();
    }

    public void PlayNormalParry(AudioSource audioSource)
    {
        audioSource.clip = GameManager.Instance.gameConfig.GetRandomNormalParryAudioClip();
        audioSource.Play();
    }

    public void PlayPerfectParry(AudioSource audioSource)
    {
        audioSource.clip = GameManager.Instance.gameConfig.GetRandomPerfectParryAudioClip();
        audioSource.Play();
    }

    public void StopBgm()
    {
        BGMSource.Stop();
    }

}
