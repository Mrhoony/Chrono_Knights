using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Control : MonoBehaviour
{
    public GameObject player;
    public Menu_InGame menu_inGame;
    public bool inPlayer;

    public void Start()
    {
        player = GameObject.Find("PlayerCharacter");
        menu_inGame = GameObject.Find("Menus").GetComponent<Menu_InGame>();
        inPlayer = false;
    }
}
