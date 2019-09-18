using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * 필드 드랍 아이템 프리팹에 스크립트 추가
 * */

public class Looting : MonoBehaviour
{
    Inventory _Invenroy;

    private void Start()
    {
        _Invenroy = GameObject.Find("Inventory").GetComponent<Inventory>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            for(int i=0; i<_Invenroy.Slot.Length; i++)
            {
                if(_Invenroy.isFull[i] == false)
                {
                    _Invenroy.isFull[i] = true;
                    _Invenroy.Slot[i].GetComponent<Image>().sprite = GetComponent<SpriteRenderer>().sprite;
                    Destroy(gameObject);
                    break;
                }
            }
        }
    }
}
