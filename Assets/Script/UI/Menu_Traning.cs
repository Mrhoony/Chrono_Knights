﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu_Traning : MonoBehaviour
{
    public PlayerStatus playerStat;
    public GameObject button;
    public GameObject[] traningButton;
    public Image[] gauge;
    public NPC_Trainer npc_Trainer;

    public float[] limit_traning;
    public float[] traningStat;
    public int[] traning_count;
    public int focus;
    public bool isTraningOn = false;
    public bool isTraningPossible;

    public void Update()
    {
        if (!isTraningOn) return;

        if (Input.GetKeyDown(KeyCode.Z))
        {
            if(focus < 6)
            {
                if (!isTraningPossible) return;
                Traning(focus);
            }
            else
            {
                npc_Trainer.CloseTraningMenu();
            }
        }

        if (Input.GetKeyDown(KeyCode.UpArrow)) { FocusedSlot(-1); }
        if (Input.GetKeyDown(KeyCode.DownArrow)) { FocusedSlot(1); }
    }
    
    public void OpenTraningMenu(NPC_Trainer _trainer)
    {
        isTraningOn = true;
        npc_Trainer = _trainer;
        Init();

        isTraningPossible = DungeonManager.instance.NewDayCheck();

        if (isTraningPossible)
        {
            button.SetActive(true);
            for (int i = 0; i < 6; ++i)
            {
                gauge[i].fillAmount = traningStat[i] / limit_traning[i];

                if (traningStat[i] >= limit_traning[i])
                {
                    traningButton[i].GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.8f);
                }
                else
                {
                    traningButton[i].GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
                }
            }
            traningButton[0].GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.8f);
            focus = 0;
        }
        else
        {
            button.SetActive(false);
            traningButton[6].GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.8f);
            focus = 6;
        }

        Debug.Log("open traning");
    }
    public void Init()
    {
        playerStat = GameObject.Find("PlayerCharacter").GetComponent<PlayerStatus>();
        limit_traning = playerStat.playerData.GetLimitTraning();
        traningStat = playerStat.playerData.GetTraningStat();
        traning_count = playerStat.playerData.GetTraningCount();
    }

    public void CloseTraningMenu()
    {
        focus = 0;
        button.SetActive(false);
        isTraningOn = false;
    }

    public void Traning(int stat)
    {
        if (traningStat[stat] >= limit_traning[stat]) return;

        if (limit_traning[stat] > 0)
        {
            switch (stat)
            {
                case 0:
                case 1:
                case 5:
                    traningStat[stat] += (1f - traning_count[stat] * 0.1f);
                    break;
                case 2:
                case 3:
                case 4:
                    traningStat[stat] += (0.1f - traning_count[stat] * 0.1f);
                    break;
            }
            if (traningStat[stat] > limit_traning[stat])
                traningStat[stat] = limit_traning[stat];

            ++traning_count[stat];
            if (traning_count[stat] > 5)
                traning_count[stat] = 5;

            gauge[stat].fillAmount = traningStat[stat] / limit_traning[stat];

            playerStat.PlayerStatusInit();
            isTraningPossible = true;
            FocusedSlot(6);
            button.SetActive(false);
        }
    }

    void FocusedSlot(int AdjustValue)
    {
        if (focus < 0 || focus > 6) { return; }

        traningButton[focus].GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);

        focus += AdjustValue;
        if (focus < 0) focus = 0;
        else if (focus > 6) focus = 6;

        traningButton[focus].GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.8f);
    }
}
