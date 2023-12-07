using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    //通用的两个Source
    [SerializeField] private AudioSource BGMSource;

    [SerializeField] private AudioSource FXSource;

    [SerializeField] private AudioClip bgmInMenu;
    [SerializeField] private AudioClip bgmInRest;
    [SerializeField] private AudioClip bgmInFight;

    public void PlayBgmBySceneType(SceneType sceneType)
    {
        AudioClip targetClip = sceneType switch
        {
            SceneType.Menu => bgmInMenu,
            SceneType.RestScene => bgmInRest,
            SceneType.FightScene => bgmInFight,
            _ => null
        };
        PlayGeneralBGM(targetClip);
    }
    
    public void PlayGeneralBGM(AudioClip clip)
    {
        if (clip == null) return;
        BGMSource.clip = clip;
        BGMSource.Play();
    }

    public void PlayGeneralFX(AudioClip clip)
    {
        if (clip == null) return;
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
