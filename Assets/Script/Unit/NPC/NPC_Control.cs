using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        if (!inPlayer) return true; // 플레이어가 근처에 없으면 리턴
        if (player.GetComponent<PlayerControl>().inputDirection != 0)
        {
            player.GetComponent<PlayerControl>().playerInputKey.SetActive(false);
        }
        else
        {
            player.GetComponent<PlayerControl>().playerInputKey.SetActive(true);
        }
        if (canvasManager.GameMenuOnCheck()) return true;       // 다른 UI가 켜져있으면
        if (canvasManager.TownUIOnCheck()) return true;         // 타운 UI가 켜져있으면
        return false;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            inPlayer = true;
            player = collision.gameObject;
        }
    }
    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            inPlayer = false;
            player = collision.gameObject;
        }
    }
}
