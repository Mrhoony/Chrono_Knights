using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Storage : MonoBehaviour
{
    public bool inPlayer;
    public bool onStorage;
    public GameObject storage;

    private void Start()
    {
        storage = GameObject.Find("UI/Storage");
        onStorage = false;
    }

    private void Update()
    {
        if (!inPlayer) return;

        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (!onStorage)
            {
                onStorage = true;
                storage.SetActive(true);
            }
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            if (onStorage)
            {
                onStorage = false;
                storage.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            inPlayer = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            inPlayer = false;
        }
    }
}
