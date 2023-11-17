using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game Config")] 
public class GameConfig : ScriptableObject
{
    [SerializeField] private List<AudioClip> characterHitAudioClips;      //��ɫ���ܻ���Ч

    [SerializeField] private List<AudioClip> normalParryAudioClips;       //��ɫ�����񵲵���Ч

    [SerializeField] private List<AudioClip> perfectParryAudioClips;      //��ɫ�����񵲵���Ч

    [SerializeField] private List<ItemData_SO> allItems;

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

    public void InitItemDict(Dictionary<string,ItemData_SO> itemNameToItemData)
    {
        foreach (var item in allItems)
        {
            itemNameToItemData.Add(item.itemName, item);
        }
    }
    
    #endregion
}
