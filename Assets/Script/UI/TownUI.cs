using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownUI : MonoBehaviour
{
    public GameObject[] townMenus;
    public PlayerControl playerCharacter;
    public CanvasManager menu;
    public bool[] isTownUIOn;

    private void Awake()
    {
        menu = GameObject.Find("UI").GetComponent<CanvasManager>();
        playerCharacter = PlayerControl.instance;
        isTownUIOn = new bool[4];
        for(int i = 0; i < 4; ++i)
        {
            isTownUIOn[i] = false;
        }
    }

    public void OpenShopMenu()
    {
        isTownUIOn[0] = true;
        playerCharacter.enabled = false;
        townMenus[0].SetActive(true);
        townMenus[0].GetComponent<TownUI_Shop>().OpenTownUIMenu();
    }
    public void CloseShopMenu()
    {
        townMenus[0].SetActive(true);
        playerCharacter.enabled = true;
        isTownUIOn[0] = false;
    }
    public void OpenTrainingMenu()
    {
        isTownUIOn[1] = true;
        playerCharacter.enabled = false;
        townMenus[1].SetActive(true);
        townMenus[1].GetComponent<TownUI_Training>().OpenTownUIMenu();
    }
    public void CloseTrainingMenu()
    {
        townMenus[1].SetActive(true);
        playerCharacter.enabled = true;
        isTownUIOn[1] = false;
    }
    public void OpenEnchantMenu()
    {
        isTownUIOn[2] = true;
        playerCharacter.enabled = false;
        townMenus[2].SetActive(true);
        townMenus[2].GetComponent<TownUI_Enchant>().OpenTownUIMenu();
    }
    public void CloseEnchantMenu()
    {
        townMenus[2].SetActive(true);
        playerCharacter.enabled = true;
        isTownUIOn[2] = false;
    }
    public void OpenUpgradeMenu()
    {
        isTownUIOn[3] = true;
        playerCharacter.enabled = false;
        townMenus[3].SetActive(true);
        townMenus[3].GetComponent<TownUI_Upgrade>().OpenTownUIMenu();
    }
    public void CloseUpgradeMenu()
    {
        townMenus[3].SetActive(true);
        playerCharacter.enabled = true;
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
