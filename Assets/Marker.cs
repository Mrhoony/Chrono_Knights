using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum Markers
{
    // NF = Next Floor, TF = This Floor
    // Pos = Positive, Neg = Negative

    SetMonster_NF,
    SetDrop_NF,
    SetSpecialMonster_NF,
    SetDamageBuffOnFloor_NF,
    SetDamageBuffOnMonster_NF,
    SetDamageBuffOnPlayer_NF,

    SetPosHPOnMonster_NF,
    SetNegHPOnMonster_NF,
    SetPosDashSpeedOnPlayer_NF,
    SetNegDashSpeedOnPlayer_NF,
    SetPosDamageOnPlayer_NF,
    SetNegDamageOnPlayer_NF
}

public class Marker : MonoBehaviour
{
    public Markers ThisMarker;

    /*
        아래 모든 함수는 인수로 키값이 필요
        DungeonManger.cs에서 Marker_Variable 클래스 생성
        Execute() 실행시 DungeonManager.cs 내에 Marker_Variable 클래스에 Execute()의 결과값을 받아서 계산하여 적용
    */

    public void Execute()
    {
        switch (ThisMarker)
        {
            case Markers.SetDamageBuffOnFloor_NF:
                {
                    SetDamageBuffOnFloor_NF();
                }break;
            case Markers.SetDamageBuffOnMonster_NF:
                {
                    SetDamageBuffOnMonster_NF();
                }break;
            case Markers.SetDamageBuffOnPlayer_NF:
                {
                    SetDamageBuffOnPlayer_NF();
                }break;
            case Markers.SetDrop_NF:
                {
                    SetDrop_NF();
                }break;
            case Markers.SetMonster_NF:
                {
                    SetMonster_NF();
                }break;
            case Markers.SetNegDamageOnPlayer_NF:
                {
                    SetNegDamageOnPlayer_NF();
                }break;
            case Markers.SetNegDashSpeedOnPlayer_NF:
                {
                    SetNegDashSpeedOnPlayer_NF();
                }break;
            case Markers.SetNegHPOnMonster_NF:
                {
                    SetNegHPOnMonster_NF();
                }break;
            case Markers.SetPosDamageOnPlayer_NF:
                {
                    SetPosDamageOnPlayer_NF();
                }break;
            case Markers.SetPosDashSpeedOnPlayer_NF:
                {
                    SetPosDashSpeedOnPlayer_NF();
                }break;
            case Markers.SetPosHPOnMonster_NF:
                {
                    SetPosHPOnMonster_NF();
                }break;
            case Markers.SetSpecialMonster_NF:
                {
                    SetSpecialMonster_NF();
                }break;
        }
    }

    private void SetMonster_NF()
    {
        // DungeonManager.Instance.Marker_Variable.SetMonster_NF = keyValue;
    }
    private void SetDrop_NF()
    {

    }
    private void SetSpecialMonster_NF()
    {

    }
    private void SetDamageBuffOnFloor_NF()
    {

    }
    private void SetDamageBuffOnMonster_NF()
    {

    }
    private void SetDamageBuffOnPlayer_NF()
    {

    }
    private void SetPosHPOnMonster_NF()
    {

    }
    private void SetNegHPOnMonster_NF()
    {

    }
    private void SetPosDashSpeedOnPlayer_NF()
    {

    }
    private void SetNegDashSpeedOnPlayer_NF()
    {

    }
    private void SetPosDamageOnPlayer_NF()
    {

    }
    private void SetNegDamageOnPlayer_NF()
    {

    }
}
