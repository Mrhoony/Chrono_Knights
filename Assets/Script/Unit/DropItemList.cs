﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 몬스터 프리팹에 스크립트를 추가
 * 드랍될 아이템프리팹과 드랍확률을 인스펙터창에서 설정
 * 몬스터가 죽을때 ItemDropChance() 호출
 * */

public class DropItemList : MonoBehaviour
{
    public GameObject[] dropItems;
    public GameObject item;
    public int[] dropChances;

    public void ItemDropChance()
    {
        int dropChance = Random.Range(1, 101);
        int dropItemList = dropItems.Length;

        for (int i=0; i< dropItemList; i++)
        {
            if(dropChance > dropChances[i])
            {
                item = Instantiate(dropItems[i], new Vector2(transform.position.x, transform.position.y + 0.5f), Quaternion.identity);
                item.transform.parent = GameObject.Find("DungeonPoolManager/DropItemPool").transform;
            }
        }
    }
}
