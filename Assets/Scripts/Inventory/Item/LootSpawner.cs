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
        //采用轮盘赌算法
        float[] accP = new float[lootItems.Length + 1];  //累计的概率
        float total = 0;
        for (int i = 0; i < lootItems.Length; i++)
        {
            accP[i] = total;
            total += lootItems[i].weight;
        }
        accP[lootItems.Length] = total;
        float random = Random.Range(0, total);
        for (int i = 0; i < lootItems.Length; i++)
        {
            if (random <= accP[i + 1])
            {
                Instantiate(lootItems[i].GetRandomObject(), transform.position + Vector3.up * 2, Quaternion.identity);
                break;
            }
        }
    }

}
