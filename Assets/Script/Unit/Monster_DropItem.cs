﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_DropItem : MonoBehaviour
{
    public GameObject DropItem;

    private void OnDestroy()
    {
        int DropPercentage = Random.Range(0, 10);
        if (DropPercentage == 0)
        {
            Instantiate(DropItem, transform);
        }
    }
}