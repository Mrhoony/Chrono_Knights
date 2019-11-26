using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 인수로 Key를 받게끔 수정 필요

public class Question : MonoBehaviour
{
    int MarkBuffNum;

    private void Start()
    {
        Sprite[] AllMarks = Resources.LoadAll<Sprite>("UI/ui_hpbell_set");
        Sprite Mark = GetComponent<SpriteRenderer>().sprite;

        for (int cnt = 0; cnt < AllMarks.Length; cnt++)
        {
            if(Mark == AllMarks[cnt])
            {
                MarkBuffNum = cnt;
                break;
            }
        }
    }

    public void Execute()   // 인수로 Key 받음
    {
        switch (MarkBuffNum)
        {
            case 3:
                {
                    SetMarkDamage();    // 인수로 Key 받음
                }
                break;
            case 4:
                {
                    SetMarkDefence();   // 인수로 Key 받음
                }
                break;
            case 5:
                {
                    SetMarkSpeed(); // 인수로 Key 받음
                }
                break;
        }
    }

    private void SetMarkDamage()    // 인수로 Key 받음
    {
        PlayerControl.instance.pStat.Atk -= 1;  // 1 대신 Key값으로 대체 필요
    }
    private void SetMarkDefence()   // 인수로 Key 받음
    {
        PlayerControl.instance.pStat.defense += 1;  // 1 대신 Key값으로 대체 필요
    }
    private void SetMarkSpeed() // 인수로 Key 받음
    {
        PlayerControl.instance.pStat.moveSpeed *= 1.5f; // 1.5f 대신 Key값으로 대체 필요
    }
}