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
        public float weight;  //��Ʒ�����Ȩ��

        public GameObject GetRandomObject()
        {
            return lootItem[Random.Range(0, lootItem.Length)];
        }

    }

    [SerializeField] private int lootItemCount;
    [SerializeField] private LootItem[] lootItems;

    public void SpawnLoot()
    {
        //�������̶��㷨
        float[] accP = new float[lootItems.Length + 1];  //�ۼƵĸ���
        float total = 0;
        for (int i = 0; i < lootItems.Length; i++)
        {
            accP[i] = total;
            total += lootItems[i].weight;
        }
        accP[lootItems.Length] = total;
        for (int i = 0; i < lootItemCount; i++)
        {
            float random = Random.Range(0, total);
            for (int j = 0; j < lootItems.Length; j++)
            {
                if (random <= accP[j + 1])
                {
                    Instantiate(lootItems[j].GetRandomObject(), transform.position + Vector3.up * 2, Quaternion.identity);
                    break;
                }
            }
        }
    }

}
