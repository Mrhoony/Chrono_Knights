using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu_Upgrade : MonoBehaviour
{
    public PlayerStat playerStat;
    public PlayerData playerData;
    public PlayerEquipment playerEquipment;
    public GameObject button;
    public int[] limitUpgrade;

    public void Start()
    {
        playerStat = GameObject.Find("PlayerCharacter").GetComponent<PlayerStat>();
        playerData = playerStat.playerData;
    }

    public void ReinforceEquip(int stat)
    {
        if (limitUpgrade[stat] > 0)
        {
            switch (stat)
            {
                case 0:     // recovery
                    playerData.up_recovery += 1;
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
                    playerData.up_jumpCount += 0.1f;
                    break;
                case 5:     // defense
                    playerData.up_defense += 1;
                    break;
            }
            playerStat.Init();
            DungeonManager.instance.possible_Traning = false;
            --limitUpgrade[stat];
            button.SetActive(false);
        }
    }
}
