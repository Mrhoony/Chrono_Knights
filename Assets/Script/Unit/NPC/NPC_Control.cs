using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Control : MonoBehaviour
{
    public GameObject player;
    public CanvasManager canvasManager;
    public TownUI townUI;
    public bool inPlayer;

    public void OnEnable()
    {
        canvasManager = GameObject.Find("UI").GetComponent<CanvasManager>();
        inPlayer = false;
    }


    public bool OpenUICheck()
    {
        if (!inPlayer) return true;
        if (PlayerControl.instance.GetActionState() != ActionState.Idle) return true;
        if (canvasManager.GameMenuOnCheck()) return true;
        if (townUI.GetTownUIOnCheck()) return true;
        return false;
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            inPlayer = true;
            player = collision.gameObject;
            collision.transform.GetChild(1).gameObject.SetActive(true);
        }
    }
    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            inPlayer = false;
            player = collision.gameObject;
            collision.transform.GetChild(1).gameObject.SetActive(false);
        }
    }
}
