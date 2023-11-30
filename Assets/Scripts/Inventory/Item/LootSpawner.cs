using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

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

        public int LootItemCount => lootItem.Length;

    }

    [SerializeField] private int exp;
    [SerializeField] private int money;

    [SerializeField] private int lootItemCount;
    [SerializeField] private LootItem[] lootItems;
    
    private float[] accP ;  //累计的概率
    private float totalWeigh;

    private EnemyController enemyController;

    private void Awake()
    {
        enemyController = GetComponent<EnemyController>();
    }

    private void Start()
    {
        accP = new float[lootItems.Length + 1];
        totalWeigh = 0;
        for (int i = 0; i < lootItems.Length; i++)
        {
            accP[i] = totalWeigh;
            totalWeigh += lootItems[i].weight;
        }
        accP[lootItems.Length] = totalWeigh;
    }

    public void SpawnLoot()
    {
        //TODO:掉落 经验值 金钱
        //enemyController.player.
        
        //采用轮盘赌算法处理掉落的物品
        for (int i = 0; i < lootItemCount; i++)
        {
            float random = Random.Range(0, totalWeigh);
            for (int j = 0; j < lootItems.Length; j++)
            {
                if (random <= accP[j + 1])
                {
                    if (lootItems[j].LootItemCount != 0)
                        Instantiate(lootItems[j].GetRandomObject(), enemyController.FocusTransform.position,Quaternion.identity);
                    break;
                }
            }
        }
    }

}
