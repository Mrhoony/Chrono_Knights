using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyInventory : MonoBehaviour
{
    public GameObject slot;
    public bool[] isFull = new bool[5];
    GameObject[] Inventory = new GameObject[5];

    int _selected = 0;

    private void Awake()
    {
        for (int cnt = 0; cnt < 5; cnt++)
        {
            Inventory[cnt] = Instantiate(slot, new Vector3(0.55f * (cnt - 2), 0f), Quaternion.identity);
            Inventory[cnt].transform.localScale = new Vector3(3, 3);
            Inventory[cnt].transform.parent = gameObject.transform;
        }
    }

    public void Update()
    {
        if (gameObject.activeInHierarchy)
        {
            //GameObject.Find("Player").GetComponent<PlayerController>().SetActive(false); // 퀵슬롯이 활성화된 동안 PlayerController 중지            

            // 또는 PlayerController 스크립트에서 input.getkeydown(keycode.q)를 입력받았을때 PlayerController 스크립트를 비활성화 하고 QuickSlot 게임오브젝트를 활성화                          
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            gameObject.SetActive(false);
            //GameObject.Find("Player").GetComponent<PlayerController>().SetActive(true); // 퀵슬롯이 비활성화 될때 PlayerController 실행
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Selected("Right");
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Selected("Left");
        }
    }


    void Selected(string adjust)
    {
        if (adjust == "Right" && _selected < 4)
        {
            Inventory[_selected].transform.GetChild(0).gameObject.SetActive(false);
            _selected++;
            Inventory[_selected].transform.GetChild(0).gameObject.SetActive(true);
        }

        if (adjust == "Left" && _selected > 0)
        {
            Inventory[_selected].transform.GetChild(0).gameObject.SetActive(false);
            _selected--;
            Inventory[_selected].transform.GetChild(0).gameObject.SetActive(true);
        }
    }

    public bool Loot(GameObject Item)
    {
        for (int cnt = 0; cnt < Inventory.Length; cnt++)
        {
            if (isFull[cnt] == false)
            {
                isFull[cnt] = true;
                Instantiate(Item, Inventory[cnt].transform.position, Quaternion.identity).transform.parent = Inventory[cnt].transform;
                return true;
            }
        }
        return false;
    }
}
