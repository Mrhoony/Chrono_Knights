using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownUI : MonoBehaviour
{
    public GameObject[] townMenus;
    public PlayerControl playerCharacter;
    public CanvasManager canvasManager;

    private void Awake()
    {
        canvasManager = GameObject.Find("UI").GetComponent<CanvasManager>();
        playerCharacter = PlayerControl.instance;
    }

    public void OpenShopMenu(Menu_Inventory _inventory)
    {
        townMenus[0].SetActive(true);
        townMenus[0].GetComponent<TownUI_Shop>().OpenTownUIMenu(_inventory);
    }
    public void CloseShopMenu()
    {
        townMenus[0].GetComponent<TownUI_Shop>().CloseTownUIMenu();
        townMenus[0].SetActive(false);
    }

    public void OpenTrainingMenu()
    {
        townMenus[1].SetActive(true);
        townMenus[1].GetComponent<TownUI_Training>().OpenTownUIMenu();
    }
    public void CloseTrainingMenu()
    {
        canvasManager.CloseTrainingMenu();
        townMenus[1].SetActive(false);
    }

    public void OpenEnchantMenu()
    {
        townMenus[2].SetActive(true);
        townMenus[2].GetComponent<TownUI_Enchant>().OpenTownUIMenu();
    }
    public void CloseEnchantMenu()
    {
        canvasManager.CloseEnchantMenu();
        townMenus[2].SetActive(false);
    }

    public void OpenUpgradeMenu()
    {
        townMenus[3].SetActive(true);
        townMenus[3].GetComponent<TownUI_Upgrade>().OpenTownUIMenu();
    }
    public void CloseUpgradeMenu()
    {
        canvasManager.CloseUpgradeMenu();
        townMenus[3].SetActive(false);
    }
}
