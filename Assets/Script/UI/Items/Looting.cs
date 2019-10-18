using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * 필드 드랍 아이템 프리팹에 스크립트 추가
 * */

public class Looting : MonoBehaviour
{
    Inventory _Invetroy;

    private void Start()
    {
        _Invetroy = GameObject.Find("Inventory").GetComponent<Inventory>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            for(int i=0; i< _Invetroy.Slot.Length; i++)
            {
                if(_Invetroy.isFull[i] == false)
                {
                    _Invetroy.isFull[i] = true;
                    _Invetroy.Slot[i].GetComponent<Image>().sprite = GetComponent<SpriteRenderer>().sprite;
                    Destroy(gameObject);
                    break;
                }
            }
        }
    }
}
