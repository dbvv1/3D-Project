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
        public float weight;  //��Ʒ�����Ȩ��

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
    
    private float[] accP ;  //�ۼƵĸ���
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
        //TODO:���� ����ֵ ��Ǯ
        //enemyController.player.
        
        //�������̶��㷨����������Ʒ
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
