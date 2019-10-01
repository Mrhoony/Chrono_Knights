using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStateView : MonoBehaviour
{
    public static PlayerStateView instance;
    public PlayerStat pStat = null;
    public Image HPBar;
    public Image[] HPBarCut;

    public Image buffBar;

    public Image[] buffState;

    public Image bell;
    public bool speedDown;
    public float arrow;
    public float power;
    public float hitCount;

    IEnumerator monsterHit;

    private void Awake()
    {
        pStat = GameObject.Find("PlayerCharacter").GetComponent<PlayerStat>();
    }

    private void Start()
    {
        speedDown = false;
        arrow = 0;
        power = 0;
        hitCount = 0;
        monsterHit = MonsterHit();
    }

    // Update is called once per frame
    void Update()
    {
        HPBar.fillAmount = pStat.currentHP / pStat.pd.HP;
        buffBar.fillAmount = pStat.currentBuffTime / pStat.pd.MaxBuffTime;

        if (bell.transform.rotation.z * 90f > 0.2f * hitCount)
        {
            if (power > 0)
            {
                arrow = -0.1f * hitCount;
            }
        }else if(bell.transform.rotation.z * 90f < -0.2f * hitCount)
            if (power < 0)
            {
                arrow = 0.1f * hitCount;
            }
        
        power += arrow;

        if(hitCount != 0)
        {
            if (power > 2f * hitCount)
                power = 2f * hitCount;
            else if (power < -(2f * hitCount))
                power = -2f * hitCount;
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

    IEnumerator MonsterHit()
    {
        yield return new WaitForSeconds(5f);

        Debug.Log("hittest");

        hitCount -= 2f;

        if (hitCount <= 0f)
            hitCount = 0f;
        else
        {
            monsterHit = MonsterHit();
            StartCoroutine(monsterHit);
        }
    }

    public void Hit(float monsterAtk, float stability)
    {
        ++hitCount;
        if (hitCount > 5)
            hitCount = 5f;

        if (power >= 0)
            power += monsterAtk * hitCount * 0.2f;
        else if (power < 0)
            power -= monsterAtk * hitCount * 0.2f;


        StopCoroutine(monsterHit);

        monsterHit = MonsterHit();
        StartCoroutine(monsterHit);
    }

    public void Init()
    {
        SetBuff();
        for(int i = 0; i < HPBarCut.Length; ++i)
        {
            HPBarCut[i].enabled = true;
        }
    }

    public void SetHPCut(int i)
    {
        HPBarCut[i].enabled = false;
    }

    public void SetBuff()
    {
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
