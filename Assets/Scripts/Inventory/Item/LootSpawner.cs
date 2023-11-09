using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootSpawner : MonoBehaviour
{
    [System.Serializable]
    public class LootItem
    {
        [SerializeField]private GameObject[] lootItem;

        [Range(0,1)]
        public float weight;  //物品掉落的权重

        public GameObject GetRandomObject()
        {
            return lootItem[Random.Range(0, lootItem.Length)];
        }

    }

    [SerializeField]private LootItem[] lootItems;

    public void SpawnLoot()
    {

        foreach(var item in  lootItems)
        {
            float value = Random.value;
            if (value >= item.weight)
            {
                Instantiate(item.GetRandomObject(), transform.position + Vector3.up * 2, Quaternion.identity);
            }
        }
    }

}
