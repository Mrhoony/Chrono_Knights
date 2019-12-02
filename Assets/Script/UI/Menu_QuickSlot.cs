using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu_QuickSlot : MonoBehaviour
{
    public GameObject inventory;
    public GameObject slots;
    public GameObject[] quickSlot = new GameObject[5];
    public bool onQuickSlot;

    int focus = 0;

    private void Awake()
    {

    }

    public void Start()
    {
        onQuickSlot = false;
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
            if (!onQuickSlot)
            {
                slots.SetActive(true);
            }
            if (onQuickSlot)
            {
                slots.SetActive(false);
            }
        }

        if (Input.GetKeyDown(KeyCode.W)) { FocusedSlot(1); }
        if (Input.GetKeyDown(KeyCode.E)) { FocusedSlot(-1); }
    }

    void FocusedSlot(int AdjustValue)
    {
        if (focus + AdjustValue < 0 || focus + AdjustValue > 5) { return; }

        quickSlot[focus].transform.GetChild(0).gameObject.SetActive(false);
        focus += AdjustValue;
        quickSlot[focus].transform.GetChild(0).gameObject.SetActive(true);
    }
}