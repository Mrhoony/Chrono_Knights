using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownUI_Shop : MonoBehaviour
{
    public TownUI townUI;
    public CanvasManager canvasManager;
    public GameObject slots;
    public GameObject[] slot;
    public Slot slotInstance;
    public bool isTownMenuOn = false;

    public int slotCount;
    public bool[] isSell;

    public void OpenTownUIMenu()
    {
        isTownMenuOn = true;
        Transform[] transforms = slots.transform.GetComponentsInChildren<Transform>();
        slotCount = transforms.Length - 1;

        slot = new GameObject[8];
        for (int i = 1; i < slotCount + 1; ++i)
        {
            slot[i - 1] = transforms[i].gameObject;
            isSell[i - 1] = false;
        }
    }
    public void CloseTownUIMenu()
    {
        isTownMenuOn = false;
        townUI.OpenShopMenu();
    }
}
