using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum Markers
{
    // NF = Next Floor, TF = This Floor
    // Pos = Positive, Neg = Negative
    SetMonster_NF = 0,
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

public class Marker
{
    public Markers thisMarker = Markers.SetMonster_NF;

    // Marker 생성방법 필요

    /*
        아래 모든 함수는 인수로 키값이 필요
        DungeonManger.cs에서 Marker_Variable 클래스 생성
        Execute() 실행시 DungeonManager.cs 내에 Marker_Variable 클래스에 Execute()의 결과값을 받아서 계산하여 적용
    */

    public void ExecuteMarker(int keyValue)
    {
        switch (thisMarker)
        {
            case Markers.SetDamageBuffOnFloor_NF:
                {
                    SetDamageBuffOnFloor_NF(keyValue);
                }break;
            case Markers.SetDamageBuffOnMonster_NF:
                {
                    SetDamageBuffOnMonster_NF(keyValue);
                }break;
            case Markers.SetDamageBuffOnPlayer_NF:
                {
                    SetDamageBuffOnPlayer_NF(keyValue);
                }break;
            case Markers.SetDrop_NF:
                {
                    SetDrop_NF(keyValue);
                }break;
            case Markers.SetMonster_NF:
                {
                    SetMonster_NF(keyValue);
                }break;
            case Markers.SetNegDamageOnPlayer_NF:
                {
                    SetNegDamageOnPlayer_NF(keyValue);
                }break;
            case Markers.SetNegDashSpeedOnPlayer_NF:
                {
                    SetNegDashSpeedOnPlayer_NF(keyValue);
                }break;
            case Markers.SetNegHPOnMonster_NF:
                {
                    SetNegHPOnMonster_NF(keyValue);
                }break;
            case Markers.SetPosDamageOnPlayer_NF:
                {
                    SetPosDamageOnPlayer_NF(keyValue);
                }break;
            case Markers.SetPosDashSpeedOnPlayer_NF:
                {
                    SetPosDashSpeedOnPlayer_NF(keyValue);
                }break;
            case Markers.SetPosHPOnMonster_NF:
                {
                    SetPosHPOnMonster_NF(keyValue);
                }break;
            case Markers.SetSpecialMonster_NF:
                {
                    SetSpecialMonster_NF(keyValue);
                }break;
        }
    }

    private void SetMonster_NF(int keyValue)
    {
        DungeonManager.instance.marker_Variable.markerVariable[0] = keyValue;
    }
    private void SetDrop_NF(int keyValue)
    {
        DungeonManager.instance.marker_Variable.markerVariable[1] = keyValue;
    }
    private void SetSpecialMonster_NF(int keyValue)
    {
        DungeonManager.instance.marker_Variable.markerVariable[2] = keyValue;
    }
    private void SetDamageBuffOnFloor_NF(int keyValue)
    {
        DungeonManager.instance.marker_Variable.markerVariable[3] = keyValue;
    }
    private void SetDamageBuffOnMonster_NF(int keyValue)
    {
        DungeonManager.instance.marker_Variable.markerVariable[4] = keyValue;
    }
    private void SetDamageBuffOnPlayer_NF(int keyValue)
    {
        DungeonManager.instance.marker_Variable.markerVariable[5] = keyValue;
    }
    private void SetPosHPOnMonster_NF(int keyValue)
    {
        DungeonManager.instance.marker_Variable.markerVariable[6] = keyValue;
    }
    private void SetNegHPOnMonster_NF(int keyValue)
    {
        DungeonManager.instance.marker_Variable.markerVariable[7] = keyValue;
    }
    private void SetPosDashSpeedOnPlayer_NF(int keyValue)
    {
        DungeonManager.instance.marker_Variable.markerVariable[8] = keyValue;
    }
    private void SetNegDashSpeedOnPlayer_NF(int keyValue)
    {
        DungeonManager.instance.marker_Variable.markerVariable[9] = keyValue;
    }
    private void SetPosDamageOnPlayer_NF(int keyValue)
    {
        DungeonManager.instance.marker_Variable.markerVariable[10] = keyValue;
    }
    private void SetNegDamageOnPlayer_NF(int keyValue)
    {
        DungeonManager.instance.marker_Variable.markerVariable[11] = keyValue;
    }
}
