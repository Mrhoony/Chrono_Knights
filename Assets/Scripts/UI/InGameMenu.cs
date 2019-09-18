using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameMenu : MonoBehaviour
{
    public GameObject[] Menus;

    int Focused = 0;

    GameObject _Player;

    private void Awake()
    {
        _Player = GameObject.Find("Player");
    }

    private void Start()
    {
        Menus[Focused].SetActive(true);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.U)) { ChangeMenu(-1); }
        if (Input.GetKeyDown(KeyCode.O)) { ChangeMenu(1); }
    }

    public void CloseInGameMenu()
    {
        // _Player.GetComponent<PlayerController>().enabled = true;
        Focused = 0;
        gameObject.SetActive(false);
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
