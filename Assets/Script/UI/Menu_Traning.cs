using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu_Traning : MonoBehaviour
{
    public PlayerStatus playerStat;
    public PlayerData playerData;
    public GameObject button;

    public int[] limitUpgrade;
    public GameObject[] traningButton;
    public Button btn;
    public Image[] gauge;
    ColorBlock cor;

    public float[] traningStat;
    public float[] limit_traning;
    public int[] traning_count;

    public void Awake()
    {
        playerStat = GameObject.Find("PlayerCharacter").GetComponent<PlayerStatus>();
        playerData = playerStat.playerData;
    }

    public void Init()
    {
        limit_traning = playerData.GetLimitTraning();
        traningStat = playerData.GetTraningStat();
        traning_count = playerData.GetTraningCount();
    }

    public void OpenTraningMenu()
    {
        Init();
        button.SetActive(true);

        if (DungeonManager.instance.possible_Traning)
        {
            for (int i = 0; i < 6; ++i)
            {
                gauge[i].fillAmount = traningStat[i] / limit_traning[i];
                btn = traningButton[i].GetComponent<Button>();
                
                cor = btn.colors;

                if (traningStat[i] >= limit_traning[i])
                {
                    traningButton[i].GetComponent<Button>().interactable = false;
                    cor.normalColor = new Color(cor.normalColor.r, cor.normalColor.g, cor.normalColor.b, 120f);
                }
                else
                {
                    traningButton[i].GetComponent<Button>().interactable = true;
                    cor.normalColor = new Color(cor.normalColor.r, cor.normalColor.g, cor.normalColor.b, 255f);
                }
                btn.colors = cor;
            }
        }
    }

    public void CloseTraningMenu()
    {
        button.SetActive(false);
    }

    public void Traning(int stat)
    {
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

            playerStat.Init();

            DungeonManager.instance.possible_Traning = false;
            button.SetActive(false);
        }
    }
}
