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

    private static float maxBellRotation = 80;
    //private static float doublemaxBellRotation = 6400;
    private float beforeRotation;
    public float targetRotation;
    private float bellRotation;
    private float bellPower;
    private float bellPower2;
    private float bellspd;
    private bool isright;

    public float ringTime;
    public float dmgMulti;
    public float dmgRecovery;

    IEnumerator monsterHit;
    
    private void Start()
    {
        monsterHit = MonsterHit();
    }
    public void Init()
    {
        UIStateGauge.fillAmount = 0f;

        ringTime = 4;
        dmgMulti = 5;
        dmgRecovery = 5;

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
        // 체력바 갱신
        HPBar.fillAmount = playerStatus.currentHP / playerStatus.playerData.GetStatus((int)Status.HP);
        //buffBar.fillAmount = playerStatus.currentAmmo / playerStatus.playerData.GetMaxAmmo();
        Ring();
    }

    // 피격 후 안정화
    IEnumerator MonsterHit()
    {
        yield return new WaitForSeconds(4f - playerStatus.GetRecovery_Result() * 0.1f);
        
        targetRotation -= dmgRecovery;
        if (0 >= targetRotation) targetRotation = 0;
        else {
            monsterHit = MonsterHit();
            StartCoroutine(monsterHit);
        }
    }
    public void Ring()
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
    public void Hit(float monsterAtk)
    {
        Debug.Log("Hit monster atk UI test Damage : "+monsterAtk);
        targetRotation += monsterAtk * dmgMulti;
        if (maxBellRotation <= targetRotation) targetRotation = maxBellRotation;
    }
    public void SetHPCut(int i)
    {
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
