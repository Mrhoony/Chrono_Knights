using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Merchant : NPCControl
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (inPlayer)
        {

        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            inPlayer = true;
        }
    }
}
