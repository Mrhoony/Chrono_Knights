using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MainUI_PlayerStatusView : MonoBehaviour
{
    public static MainUI_PlayerStatusView instance;
    public PlayerStatus playerStatus;
    public Image HPBar;
    public Image[] HPBarCut;
    public Image buffBar;
    public GameObject debuffState;
    public Image UIStateGauge;
    public Image bell;

    private float maxBellRotation;
    //private static float doublemaxBellRotation = 6400;
    private float beforeRotation;
    private float targetRotation;
    private float bellRotation;
    private float bellPower;
    private float bellPower2;
    private float bellspd;
    private bool isright;

    private float ringTime;
    private float dmgMulti;
    private float dmgRecovery;

    IEnumerator monsterHit;
    
    public void Init()
    {
        monsterHit = MonsterHit();
        UIStateGauge.fillAmount = 0f;

        ringTime = 4;
        dmgMulti = 1;
        dmgRecovery = playerStatus.GetRecovery_Result();

        int count = HPBarCut.Length;
        for (int i = 0; i < count; ++i)
        {
            HPBarCut[i].enabled = true;
        }
        DebuffReset();
        BellReset();
    }

    public void BellReset()
    {
        maxBellRotation = 20;
        bellRotation = 0;
        beforeRotation = 0;
        targetRotation = 0;
        bellPower = 0;
        bellPower2 = 0;
        bellspd = 0;
        isright = true;

        bell.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
    }

    // Update is called once per frame
    void Update()
    {
        if (0 == beforeRotation && 0 == targetRotation) return;

        //로테이션값 오일러값으로 변경
        bellRotation = bell.transform.rotation.eulerAngles.z;
        if (180 <= bellRotation) bellRotation -= 360;

        //각도 끝에 도달한경우 반대방향 힘 재계산
        if (0 != targetRotation)
        {
            if ((isright && bellRotation >= beforeRotation) || (!isright && bellRotation <= (-1 * beforeRotation)))
            {
                //Debug.Log("Rotation : " + bellRotation + "   beforeRotation : " + beforeRotation);
                //로테이션값 0일때 강제로 이동시킴
                if (1 >= bellRotation * bellRotation)
                {
                    if (true == isright)
                    {
                        bellRotation = 1;
                        bell.transform.Rotate(new Vector3(0, 0, bellRotation));
                    }
                    if (false == isright)
                    {
                        bellRotation = -1;
                        bell.transform.Rotate(new Vector3(0, 0, bellRotation));
                    }
                }

                bellspd = 0;

                if (0 <= bellRotation) isright = false;
                else if (0 >= bellRotation) isright = true;

                if (0 >= bellRotation) bellRotation *= -1;

                bellPower = ((bellRotation + targetRotation) * (bellRotation + targetRotation) / (8 * bellRotation * ringTime));
                bellPower2 = ((bellRotation + targetRotation) * (bellRotation + targetRotation) / (12 * targetRotation * ringTime));

                beforeRotation = targetRotation;
            }
        }
        else
        {
            if (1 >= bellRotation) BellReset();
        }

        bellRotation = bell.transform.rotation.eulerAngles.z;
        if (180 <= bellRotation) bellRotation -= 360;

        //속도 계산 4개의 면이 존재함
        if (0 <= bellRotation)
        {
            if (isright) bellspd -= bellPower2 * Time.deltaTime;
            else bellspd -= bellPower * Time.deltaTime;
        }
        else
        {
            if (isright) bellspd += bellPower * Time.deltaTime;
            else bellspd += bellPower2 * Time.deltaTime;
        }

        //벨 실제로 움직임
        bell.transform.Rotate(new Vector3(0, 0, bellspd));
    }

    // 피격 후 안정화
    IEnumerator MonsterHit()
    {
        yield return new WaitForSeconds(4f - dmgRecovery * 0.1f);
        
        targetRotation -= dmgRecovery;
        if (0 >= targetRotation) targetRotation = 0;
        else {
            monsterHit = MonsterHit();
            StartCoroutine(monsterHit);
        }
    }

    public void RenewalHPBar()
    {
        HPBar.fillAmount = playerStatus.currentHP / playerStatus.playerData.GetStatus((int)Status.HP);
    }

    public void DMGMultiRecovery()
    {
        dmgMulti -= 2;
        if (dmgMulti < 1) dmgMulti = 1;
    }
    public void DMGMultiDebuff()
    {
        dmgMulti += 2;
        if (dmgMulti > 5) dmgMulti = 5;
    }
    public void DMGRecoveryDebuff(float _dmgRecovery)
    {
        dmgRecovery = _dmgRecovery;
    }
    public void Hit(float monsterAtk)
    {
        Debug.Log("Hit monster atk UI test Damage : " + monsterAtk);
        HPBar.fillAmount = playerStatus.currentHP / playerStatus.playerData.GetStatus((int)Status.HP);
        targetRotation += monsterAtk * dmgMulti;
        if ((maxBellRotation) <= targetRotation) targetRotation = maxBellRotation;
    }
    public void SetHPCut(int i)
    {
        maxBellRotation = 10f + 20f * (i + 1);
        HPBarCut[i].enabled = false;
        UIStateGauge.fillAmount += i / 4f;
        if (i % 2 == 1)
        {
            DebuffUpgrade();
        }
    }
    public void DebuffUpgrade()
    {
        debuffState.GetComponent<Animator>().SetTrigger("FireUpgrade_Trigger");
    }
    public void DebuffReset()
    {
        debuffState.GetComponent<Animator>().SetTrigger("FireReset_Trigger");
    }
    public void DebuffDownGrade()
    {
        debuffState.GetComponent<Animator>().SetTrigger("FireDownGrade_Trigger");
    }
}
