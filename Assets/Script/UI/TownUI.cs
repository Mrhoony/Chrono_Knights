using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownUI : MonoBehaviour
{
    public GameObject[] townMenus;
    public PlayerControl playerCharacter;
    public CanvasManager canvasManager;
    public bool[] isTownUIOn;

    private void Awake()
    {
        canvasManager = GameObject.Find("UI").GetComponent<CanvasManager>();
        playerCharacter = PlayerControl.instance;
        isTownUIOn = new bool[4];
        for(int i = 0; i < 4; ++i)
        {
            isTownUIOn[i] = false;
        }
    }

    public void OpenShopMenu(Menu_Inventory _inventory)
    {
        PlayerControl.instance.StopPlayer();
        PlayerControl.instance.enabled = false;
        isTownUIOn[0] = true;
        playerCharacter.enabled = false;
        townMenus[0].SetActive(true);
        townMenus[0].GetComponent<TownUI_Shop>().OpenTownUIMenu(_inventory);
    }
    public void CloseShopMenu()
    {
        townMenus[0].SetActive(false);
        isTownUIOn[0] = false;
        StartCoroutine(canvasManager.PlayerMoveEnable());
    }
    public void OpenTrainingMenu()
    {
        PlayerControl.instance.StopPlayer();
        PlayerControl.instance.enabled = false;
        isTownUIOn[1] = true;
        playerCharacter.enabled = false;
        townMenus[1].SetActive(true);
        townMenus[1].GetComponent<TownUI_Training>().OpenTownUIMenu();
    }
    public void CloseTrainingMenu()
    {
        townMenus[1].SetActive(false);
        isTownUIOn[1] = false;
        StartCoroutine(canvasManager.PlayerMoveEnable());
    }
    public void OpenEnchantMenu()
    {
        PlayerControl.instance.StopPlayer();
        PlayerControl.instance.enabled = false;
        isTownUIOn[2] = true;
        playerCharacter.enabled = false;
        townMenus[2].SetActive(true);
        townMenus[2].GetComponent<TownUI_Enchant>().OpenTownUIMenu();
    }
    public void CloseEnchantMenu()
    {
        townMenus[2].SetActive(false);
        isTownUIOn[2] = false;
    }
    public void OpenUpgradeMenu()
    {
        PlayerControl.instance.StopPlayer();
        PlayerControl.instance.enabled = false;
        isTownUIOn[3] = true;
        playerCharacter.enabled = false;
        townMenus[3].SetActive(true);
        townMenus[3].GetComponent<TownUI_Upgrade>().OpenTownUIMenu();
    }
    public void CloseUpgradeMenu()
    {
        townMenus[3].SetActive(false);
        isTownUIOn[3] = false;
    }

    public bool GetTownUIOnCheck()
    {
        for(int i = 0; i < 4; ++i)
        {
            if (isTownUIOn[i]) return true;
        }
        return false;
    }
    public bool GetTownUIOnCheck(int _TownUINumber)
    {
        return isTownUIOn[_TownUINumber];
    }
}
