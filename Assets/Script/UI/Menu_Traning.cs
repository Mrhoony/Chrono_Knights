using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu_Traning : MonoBehaviour
{
    public PlayerStat playerStat;
    public PlayerData playerData;
    public GameObject button;
    public int[] limitUpgrade;

    public void Start()
    {
        playerStat = GameObject.Find("PlayerCharacter").GetComponent<PlayerStat>();
        playerData = playerStat.pd;
        limitUpgrade = playerData.limitUpgrade;
    }

    public void OnEnable()
    {
        if (DungeonManager.instance.newDay)
        {
            button.SetActive(true);
        }
    }

    public void Upgrade(int stat)
    {
        if (limitUpgrade[stat] > 0)
        {
            switch (stat)
            {
                case 0:     // HP
                    playerData.up_HP += 1;
                    break;
                case 1:     // moveSpeed
                    playerData.up_moveSpeed += 0.1f;
                    break;
                case 2:     // Atk
                    playerData.up_Atk += 1;
                    break;
                case 3:     // atkSpeed
                    playerData.up_attackSpeed += 0.1f;
                    break;
                case 4:     // jumpPower
                    playerData.up_jumpPower += 0.1f;
                    break;
                case 5:     // defense
                    playerData.up_defense += 1;
                    break;
            }
            playerStat.Init();
            //DungeonManager.instance.newDay = false;
            --limitUpgrade[stat];
            button.SetActive(false);
        }
    }
}
