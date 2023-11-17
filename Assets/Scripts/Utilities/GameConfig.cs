using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game Config")] 
public class GameConfig : ScriptableObject
{
    [SerializeField] private List<AudioClip> characterHitAudioClips;      //角色的受击音效

    [SerializeField] private List<AudioClip> normalParryAudioClips;       //角色正常格挡的音效

    [SerializeField] private List<AudioClip> perfectParryAudioClips;      //角色完美格挡的音效

    [SerializeField] private List<ItemData_SO> allItems;

    #region 外界调用的接口
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
