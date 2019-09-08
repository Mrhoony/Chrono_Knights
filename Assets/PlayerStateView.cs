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

    private void Awake()
    {
        pStat = GameObject.Find("Player Character").GetComponent<PlayerStat>();
    }

    private void Start()
    {
        speedDown = false;
        arrow = 0;
        power = 0;
        StartCoroutine(MonsterHit());
    }

    // Update is called once per frame
    void Update()
    {
        HPBar.fillAmount = pStat.currentHP / pStat.HP;
        buffBar.fillAmount = pStat.currentBuffTime / pStat.MaxBuffTime;

        if (bell.transform.rotation.z * 90f > 1f)
        {
            if (power > 0)
                arrow = -0.3f;
        }else if(bell.transform.rotation.z * 90f < -1f)
            if (power < 0)
                arrow = 0.3f;

        power += arrow;
        if(Mathf.Abs(power) > 10f)
        {
            if (power > 0f)
                power = 10f;
            else
                power = -10f;
        }
        bell.transform.Rotate(new Vector3(0, 0, power * 0.2f));
    }

    IEnumerator MonsterHit()
    {
        yield return new WaitForSeconds(0f);

        StartCoroutine(MonsterHit());
    }

    public void Hit(float monsterAtk)
    {
        if (power >= 0)
            power += monsterAtk;
        else if (power < 0)
            power -= monsterAtk;
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
        foreach(Image buff in buffState)
        {
            buff.enabled = false;
        }
        buffState[value].enabled = true;
    }
}
