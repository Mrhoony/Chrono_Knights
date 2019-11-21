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

    float[] addStat;
    
    int upgradeCount;
    int upgradePercent;
    int downgradeCount;
    int downgradePercent;

    public void Start()
    {
        playerStat = GameObject.Find("PlayerCharacter").GetComponent<PlayerStat>();
        playerData = playerStat.playerData;
        playerEquipment = playerStat.playerEquip;
    }

    public void Upgrade(int num)
    {
        num = Random.Range(0, 6);
        Key key = Item_Database.instance.keyItem[2];
        if (Item_Database.instance.KeyInformation(key) != null)
        {
            addStat = playerEquipment.GetEquipAddStat(num);
            int length = addStat.Length;
            for(int i = 0; i<length; i++)
            {
                if (addStat[i] > 0)
                    upgradeCount = i;
            }

            switch (key.keyRarity)
            {
                case 1:
                    upgradePercent = Random.Range(0, 6);
                    PercentSet(num, upgradeCount, upgradePercent, 0.8f);
                    break;
                case 2:
                    upgradePercent = Random.Range(5, 11);
                    PercentSet(num, upgradeCount, upgradePercent, 1f);
                    break;
                case 3:
                    for (int i = 0; i < length; i++)
                    {
                        if (addStat[i] < 0)
                            downgradeCount = i;
                    }
                    upgradePercent = Random.Range(3, 21);
                    downgradePercent = Random.Range(0, 11);
                    PercentSet(num, upgradeCount, upgradePercent, downgradeCount, downgradePercent, 1.5f, -1f);

                    break;
            }
        }
        playerData.renew();
        //button.SetActive(false);
    }

    public void Enchant(int num)
    {
        num = Random.Range(0, 6);
        Key key = Item_Database.instance.keyItem[2];
        if (Item_Database.instance.KeyInformation(key) != null)
        {
            addStat = new float[] { 0, 0, 0, 0, 0, 0, 0 };
            playerEquipment.Init(num);
            upgradeCount = Random.Range(0, 7);

            switch (key.keyRarity)
            {
                case 1:
                    upgradePercent = Random.Range(5, 11);
                    PercentSet(num, upgradeCount, upgradePercent, 0.8f);
                    break;
                case 2:
                    upgradePercent = Random.Range(40, 61);
                    PercentSet(num, upgradeCount, upgradePercent, 1f);
                    break;
                case 3:
                    do
                    {
                        downgradeCount = Random.Range(0, 7);
                    }
                    while (upgradeCount == downgradeCount);

                    upgradePercent = Random.Range(100, 121);
                    downgradePercent = Random.Range(40, 51);
                    PercentSet(num, upgradeCount, upgradePercent, downgradeCount, downgradePercent, 1.5f, -1f);

                    break;
            }
        }
        playerData.renew();
        //button.SetActive(false);
    }

    void PercentSet(int num,int upCount, float upPercent, float Max)
    {
        addStat[upCount] += upPercent * 0.01f;
        if (addStat[upCount] > Max)
            addStat[upCount] = Max;
        playerEquipment.SetEquipOption(num, "test", addStat);
    }

    void PercentSet(int num, int upCount, float upPercent, int downCount, float downPercent, float Max, float Min)
    {
        addStat[upCount] += upPercent * 0.01f;
        addStat[downCount] -= downPercent * 0.01f;
        if (addStat[upCount] > Max)
            addStat[upCount] = Max;
        if (addStat[downCount] < Min)
            addStat[downCount] = Min;
        playerEquipment.SetEquipOption(num, "test", addStat);
    }
    
    public void boolInit(bool[] b)
    {
        int length = b.Length;
        for (int i = 0; i < length; i++)
            b[i] = false;
    }
}
