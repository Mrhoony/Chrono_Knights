using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 몬스터 프리팹에 스크립트를 추가
 * 드랍될 아이템프리팹과 드랍확률을 인스펙터창에서 설정
 * 몬스터가 죽을때 ItemDropChance() 호출
 * */

public class DropItemList : MonoBehaviour
{
    public GameObject[] DropItems;
    public int[] DropChances;

    public void ItemDropChance()
    {
        int DropChance = Random.Range(1, 101);

        for(int i=0; i<DropItems.Length; i++)
        {
            if(DropChance > DropChances[i])
            {
                Instantiate(DropItems[i]);
            }
        }
    }
}
