using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Control : MonoBehaviour
{
    public GameObject player;
    public CanvasManager menu;
    public bool inPlayer;

    public void Start()
    {
        player = GameObject.Find("PlayerCharacter");
        menu = GameObject.Find("UI").GetComponent<CanvasManager>();
        inPlayer = false;
    }
}
