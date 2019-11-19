using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu_Upgrade : MonoBehaviour
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
        if (DungeonManager.instance.possible_Upgrade)
        {
            button.SetActive(true);
        }
    }

    public void Traning(int stat)
    {
        if (limitUpgrade[stat] > 0)
        {
            switch (stat)
            {
                case 0:     // HP
                    playerData.Traning_HP += 1;
                    break;
                case 1:     // moveSpeed
                    playerData.Traning_moveSpeed += 0.1f;
                    break;
                case 2:     // Atk
                    playerData.Traning_Atk += 1;
                    break;
                case 3:     // atkSpeed
                    playerData.Traning_attackSpeed += 0.1f;
                    break;
                case 4:     // jumpPower
                    playerData.Traning_jumpPower += 0.1f;
                    break;
                case 5:     // defense
                    playerData.Traning_defense += 1;
                    break;
            }
            playerStat.Init();
            DungeonManager.instance.possible_Traning = false;
            --limitUpgrade[stat];
            button.SetActive(false);
        }
    }
}
