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
    public Markers ThisMarker;

    // Marker 생성방법 필요

    /*
        아래 모든 함수는 인수로 키값이 필요
        DungeonManger.cs에서 Marker_Variable 클래스 생성
        Execute() 실행시 DungeonManager.cs 내에 Marker_Variable 클래스에 Execute()의 결과값을 받아서 계산하여 적용
    */

    public void ExecuteMarker()
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
        // DungeonManager.instance.Marker_Variable.DropModifier = ?;
    }
    private void SetSpecialMonster_NF()
    {
        // DungeonManager.instance.Marker_Variable.SpecialMonster = ?;
    }
    private void SetDamageBuffOnFloor_NF()
    {
        // DungeonManager.instance.Marker_Variable.DamageBuffOnFloorModifier = ?;
    }
    private void SetDamageBuffOnMonster_NF()
    {
        // DungeonManager.instance.Marker_Variable.DamageBuffOnMonsterModifier = ?;
    }
    private void SetDamageBuffOnPlayer_NF()
    {
        // DungeonManager.instance.Marker_Variable.DamageBuffOnPlayerModifier = ?;
    }
    private void SetPosHPOnMonster_NF()
    {
        // DungeonManager.instance.Marker_Variable.PosHPOnMonsterModifier = ?;
    }
    private void SetNegHPOnMonster_NF()
    {
        // DungeonManager.instance.Marker_Variable.NegHPOnMonsterModifier = ?;
    }
    private void SetPosDashSpeedOnPlayer_NF()
    {
        // DungeonManager.instance.Marker_Variable.PosDashSpeedOnPlayerModifier = ?;
    }
    private void SetNegDashSpeedOnPlayer_NF()
    {
        // DungeonManager.instance.Marker_Variable.NegDashSpeedOnPlayerModifier = ?;
    }
    private void SetPosDamageOnPlayer_NF()
    {
        // DungeonManager.instance.Marker_Variable.PosDamageOnPlayerModifier = ?;
    }
    private void SetNegDamageOnPlayer_NF()
    {
        // DungeonManager.instance.Marker_Variable.NegDamageOnPlayerModifier = ?;
    }
}
