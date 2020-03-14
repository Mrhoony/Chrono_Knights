using UnityEngine;

/*
 * 몬스터 프리팹에 스크립트를 추가
 * 드랍될 아이템프리팹과 드랍확률을 인스펙터창에서 설정
 * 몬스터가 죽을때 ItemDropChance() 호출
 * */

public class DropItemList : MonoBehaviour
{
    public GameObject[] dropItems;
    public int[] dropChances;

    public void ItemDropChance()
    {
        int dropChance = Random.Range(1, 101);
        int dropItemList = dropItems.Length;

        for (int i=0; i< dropItemList; i++)
        {
            if(dropChance > dropChances[i])
            {
                GameObject dropItem;
                dropItem = Instantiate(dropItems[i], new Vector2(transform.position.x + Random.Range(-0.1f, 0.1f), transform.position.y + 0.2f), Quaternion.identity);
                dropItem.transform.parent = GameObject.Find("DropItemPool").transform;
            }
        }
    }
}
