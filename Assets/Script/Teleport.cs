using System.Collections;
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
        if (collision.CompareTag("Player"))
        {
            if (Input.GetButtonDown("Fire1"))
            {
                dm.Teleport();
            }
        }
    }
}
