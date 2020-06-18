using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport_Exit : InteractiveObject
{
    private PlayerControl playerControl;
    private EventTrigger eventTrigger;

    private void Start()
    {
        objectType = InteractiveObjectType.Teleport;
        player = GameObject.FindWithTag("Player");
        playerControl = player.GetComponent<PlayerControl>();
        eventTrigger = GetComponent<EventTrigger>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!GameManager.instance.gameStart) return;

        if (collision.CompareTag("Player"))
        {
            playerControl.playerInputKey.SetActive(false);

            if (eventCheckType != EventCheckType.TriggerEnter) return;
            // 이벤트 트리거가 비어있지 않고
            if (eventTrigger.eventEndTrigger == null) return;
            if (eventTrigger.eventName.Length == 0) return;

            eventTrigger.EventStart();
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!GameManager.instance.gameStart) return;

        if (collision.CompareTag("Player"))
        {
            playerControl.playerInputKey.SetActive(false);

            if (eventCheckType != EventCheckType.TriggerEnter) return;
            // 이벤트 트리거가 비어있지 않고
            if (eventTrigger.eventEndTrigger == null) return;
            if (eventTrigger.eventName.Length == 0) return;

            eventTrigger.EventStart();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerControl.playerInputKey.SetActive(false);
        }
    }
}