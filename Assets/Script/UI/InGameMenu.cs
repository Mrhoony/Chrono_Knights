using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameMenu : MonoBehaviour
{
    public GameObject[] Menus;
    public GameObject _Player;

    public GameObject CancelMenu;

    public Scrollbar[] sb;

    public bool InventoryOn;
    public bool CancelOn;

    int Focused = 0;
    int count = 0;
    
    private void Awake()
    {
        InventoryOn = false;
        CancelOn = false;
        sb = FindObjectsOfType<Scrollbar>();
    }

    private void Start()
    {
        for(int i = 0; i < Menus.Length; ++i)
        {
            Menus[i].SetActive(false);
        }
    }

    private void Update()
    {
        if (!CancelOn)
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
            if (InventoryOn)
            {
                if (Input.GetKeyDown(KeyCode.U))
                {
                    ChangeMenu(-1);
                }
                if (Input.GetKeyDown(KeyCode.O))
                {
                    ChangeMenu(1);
                }
            }
        }

        if (Input.GetButtonDown("Cancel"))
        {
            if (!CancelOn)
            {
                CancelOn = !CancelOn;
                OpenCancelMenu();
            }
            else
            {
                CancelOn = !CancelOn;
                CloseCancelMenu(true);
            }
        }
    }

    public void OpenCancelMenu()
    {
        _Player.GetComponent<PlayerControl>().enabled = false;
        Time.timeScale = 0;
        CancelMenu.SetActive(true);
    }

    public void CloseCancelMenu(bool inGame)
    {
        Time.timeScale = 1;
        CancelMenu.SetActive(false);
        if(inGame)
            _Player.GetComponent<PlayerControl>().enabled = true;
    }

    public void OpenInGameMenu()
    {
        _Player.GetComponent<PlayerControl>().enabled = false;
        Menus[Focused].SetActive(true);
        foreach (Scrollbar bar in sb)
        {
            ++count;
            bar.size = 0f;
            bar.value = 1f;
        }
        Debug.Log(count);
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
            foreach (Scrollbar bar in sb)
            {
                ++count;
                bar.size = 0f;
                bar.value = 1f;
            }
            Debug.Log(count);
            Menus[Focused].SetActive(false);
            Focused += AdjustValue;
        }
    }
}
