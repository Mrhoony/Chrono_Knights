using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport_Exit : InteractiveObject
{
    private PlayerControl playerControl;
    private EventTrigger eventTrigger;
    public Animator animator;

    private void Awake()
    {
        objectType = InteractiveObjectType.Teleport;
        player = GameObject.FindWithTag("Player");
        playerControl = player.GetComponent<PlayerControl>();
        eventTrigger = GetComponent<EventTrigger>();
        animator = GetComponent<Animator>();
    }

    public void FloorSettingEnd()
    {
        animator.SetTrigger("isEnterTrigger");
    }
    public void EnterDungeonFloor()
    {
        player.SetActive(true);
        playerControl.enabled = true;

        if (eventCheckType != EventCheckType.TriggerEnter) return;
        // 이벤트 트리거가 비어있지 않고
        if (eventTrigger.eventName.Length == 0) return;

        Debug.Log("Trigger enter event start");
        eventTrigger.EventStart();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!GameManager.instance.gameStart) return;
        if (DungeonManager.instance.isSceneLoading) return;

        if (collision.CompareTag("Player"))
        {
            playerControl.playerInputKey.SetActive(false);
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!GameManager.instance.gameStart) return;
        if (DungeonManager.instance.isSceneLoading) return;

        if (collision.CompareTag("Player"))
        {
            playerControl.playerInputKey.SetActive(false);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!GameManager.instance.gameStart) return;
        if (DungeonManager.instance.isSceneLoading) return;

        if (collision.CompareTag("Player"))
        {
            playerControl.playerInputKey.SetActive(false);
        }
    }
}