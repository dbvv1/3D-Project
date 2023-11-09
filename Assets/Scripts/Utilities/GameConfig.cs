using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game Config")] 
public class GameConfig : ScriptableObject
{
    [SerializeField] private List<AudioClip> characterHitAudioClips;      //��ɫ���ܻ���Ч

    [SerializeField] private List<AudioClip> normalParryAudioClips;       //��ɫ�����񵲵���Ч

    [SerializeField] private List<AudioClip> perfectParryAudioClips;      //��ɫ�����񵲵���Ч


    #region �����õĽӿ�
    public AudioClip GetRandomHitAudioClip()
    {
        return characterHitAudioClips[Random.Range(0, characterHitAudioClips.Count)];
    }

    public AudioClip GetRandomNormalParryAudioClip()
    {
        return normalParryAudioClips[Random.Range(0, normalParryAudioClips.Count)];
    }

    public AudioClip GetRandomPerfectParryAudioClip()
    {
        return perfectParryAudioClips[Random.Range(0, perfectParryAudioClips.Count)];
    }
    #endregion


}
