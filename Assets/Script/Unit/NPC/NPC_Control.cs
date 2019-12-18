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

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            inPlayer = true;
            collision.transform.GetChild(1).gameObject.SetActive(true);
        }
    }
    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            inPlayer = false;
            collision.transform.GetChild(1).gameObject.SetActive(false);
        }
    }
}
