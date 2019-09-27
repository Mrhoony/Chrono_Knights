using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameMenu : MonoBehaviour
{
    public GameObject[] Menus;
    public GameObject player;

    public bool InventoryOn;

    int Focused = 0;

    GameObject _Player;

    private void Awake()
    {
        _Player = GameObject.Find("PlayerCharacter");
        InventoryOn = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (!InventoryOn)
            {
                InventoryOn = !InventoryOn;
                OpenInGameMenu();
            }
            else
            {
                InventoryOn = !InventoryOn;
                CloseInGameMenu();
            }
        }

        if (Input.GetKeyDown(KeyCode.U)) { ChangeMenu(-1); }
        if (Input.GetKeyDown(KeyCode.O)) { ChangeMenu(1); }
    }

    public void OpenInGameMenu()
    {
        _Player.GetComponent<PlayerControl>().enabled = false;
        Menus[Focused].SetActive(true);
    }

    public void CloseInGameMenu()
    {
        Menus[Focused].SetActive(false);
        Focused = 0;
        _Player.GetComponent<PlayerControl>().enabled = true;
    }

    void ChangeMenu(int AdjustValue)
    {
        if(!(Focused + AdjustValue < 0 || Focused + AdjustValue >= Menus.Length))
        {
            Menus[Focused + AdjustValue].SetActive(true);
            Menus[Focused].SetActive(false);
            Focused += AdjustValue;
        }
    }
}
