﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    DungeonManager dm;

    public void OnEnable()
    {
        dm = DungeonManager.instance;
    }
    
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (Input.GetButtonDown("Fire1"))
            {
                Debug.Log("t1");
                dm.Teleport();
            }
        }
    }
}
