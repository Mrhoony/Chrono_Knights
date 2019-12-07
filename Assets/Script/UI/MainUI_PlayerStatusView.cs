using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainUI_PlayerStatusView : MonoBehaviour
{
    public static MainUI_PlayerStatusView instance;
    public PlayerStatus playerStatus;
    public Image HPBar;
    public Image[] HPBarCut;

    public Image buffBar;

    public Image[] buffState;

    public Image bell;
    public bool speedDown;
    public float arrow;
    public float power;
    public float hitCount;
    public float currentHP;

    IEnumerator monsterHit;

    private void Awake()
    {
        playerStatus = GameObject.Find("PlayerCharacter").GetComponent<PlayerStatus>();
    }

    private void Start()
    {
        speedDown = false;
        arrow = 0;          // 흔들리는 방향
        power = 0;          // 흔들리는 힘
        hitCount = 0;       // 맞은 횟수 카운트
        monsterHit = MonsterHit();
    }

    public void Init()
    {
        SetBuff(0);
        for (int i = 0; i < HPBarCut.Length; ++i)
        {
            HPBarCut[i].enabled = true;
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        // 체력바 갱신
        HPBar.fillAmount = playerStatus.currentHP / playerStatus.playerData.GetStatus(6);
        buffBar.fillAmount = playerStatus.currentBuffTime / playerStatus.playerData.GetMaxBuffTime();

        // 맞은 횟수 비례 최대 흔들리는 각도
        if (bell.transform.rotation.z * 90f > 0.2f * hitCount)
        {
            if (power > 0)
            {
                arrow = -0.1f * hitCount;
            }
        }else if(bell.transform.rotation.z * 90f < -0.2f * hitCount)
        {
            if (power < 0)
            {
                arrow = 0.1f * hitCount;
            }
        }
        
        // 흔들리는 힘 증가
        power += arrow;

        // 흔들리는 힘 최대치
        if(hitCount != 0)
        {
            if (power > (4f - playerStatus.currentHP * 0.02f) * hitCount)
                power = (4f - playerStatus.currentHP * 0.02f) * hitCount;
            else if (power < -((4f - playerStatus.currentHP * 0.02f) * hitCount))
                power = -(4f - playerStatus.currentHP * 0.02f) * hitCount;
        }
        else
        {
            power = 0f;

            if (bell.transform.rotation.z > 0)
            {
                power = -1f;
            }
            else if (bell.transform.rotation.z < 0)
                power = 1f;

            if (power < 0 && bell.transform.rotation.z <= 0)
            {
                bell.transform.Rotate(new Vector3(0f, 0f, 0f));
            }
            else if(power > 0 && bell.transform.rotation.z >= 0)
            {
                bell.transform.Rotate(new Vector3(0f, 0f, 0f));
            }
        }

        bell.transform.Rotate(new Vector3(0, 0, power * 0.2f));
    }

    // 피격 후 안정화
    IEnumerator MonsterHit()
    {
        yield return new WaitForSeconds(5f - playerStatus.defense * 0.02f);
        
        hitCount -= 2f;

        if (hitCount <= 0f)
            hitCount = 0f;
        else
        {
            monsterHit = MonsterHit();
            StartCoroutine(monsterHit);
        }
    }

    public void Hit(float monsterAtk)
    {
        ++hitCount;
        if (hitCount > 5)
            hitCount = 5f;

        // 몬스터 공격력에 따른 흔들림
        if (power >= 0)
            power += monsterAtk * hitCount * 0.2f;
        else if (power < 0)
            power -= monsterAtk * hitCount * 0.2f;
        
        StopCoroutine(monsterHit);
        monsterHit = MonsterHit();
        StartCoroutine(monsterHit);
    }

    public void SetHPCut(int i)
    {
        HPBarCut[i].enabled = false;
    }
    
    public void SetBuff(int value)
    {
        for(int i = 0; i < buffState.Length; ++i)
        {
            buffState[i].enabled = false;
        }
        buffState[value].enabled = true;
    }
}
