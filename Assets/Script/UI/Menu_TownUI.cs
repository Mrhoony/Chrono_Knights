using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu_TownUI : MonoBehaviour
{
    public GameObject[] townMenus;
    public CanvasManager menu;

    private void Awake()
    {
        menu = GameObject.Find("UI").GetComponent<CanvasManager>();
    }
}
