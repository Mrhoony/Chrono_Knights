using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    SetPosAttackMulty_NF,
    SetNegAttackMulty_NF,
    Null
}
public class Marker
{
    public Markers thisMarker = Markers.Null;
    public Markers preMarker = Markers.Null;

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
            case Markers.SetMonster_NF:
                {
                    SetMonster_NF(keyValue);
                }
                break;
            case Markers.SetDrop_NF:
                {
                    SetDrop_NF(keyValue);
                }
                break;
            case Markers.SetDamageBuffOnFloor_NF:
                {
                    SetDamageBuffOnFloor_NF(keyValue);
                }
                break;
            case Markers.SetDamageBuffOnMonster_NF:
                {
                    SetDamageBuffOnMonster_NF(keyValue);
                }
                break;
            case Markers.SetDamageBuffOnPlayer_NF:
                {
                    SetDamageBuffOnPlayer_NF(keyValue);
                }
                break;
            case Markers.SetPosHPOnMonster_NF:
                {
                    SetPosHPOnMonster_NF(keyValue);
                }
                break;
            case Markers.SetNegHPOnMonster_NF:
                {
                    SetNegHPOnMonster_NF(keyValue);
                }
                break;
            case Markers.SetPosDashSpeedOnPlayer_NF:
                {
                    SetPosDashSpeedOnPlayer_NF(keyValue);
                }
                break;
            case Markers.SetNegDashSpeedOnPlayer_NF:
                {
                    SetNegDashSpeedOnPlayer_NF(keyValue);
                }
                break;
            case Markers.SetPosAttackMulty_NF:
                {
                    SetPosAttackMulty_NF(keyValue);
                }
                break;
            case Markers.SetNegAttackMulty_NF:
                {
                    SetNegAttackMulty_NF(keyValue);
                }
                break;
            case Markers.SetSpecialMonster_NF:
                {
                    SetSpecialMonster_NF(keyValue);
                }
                break;
            case Markers.Null:
                DungeonManager.instance.dungeonMaker.MarkerReset();
                break;
        }
    }
    private void SetMonster_NF(int keyValue)
    {
        DungeonManager.instance.dungeonMaker.marker_Variable.markerVariable[0] = keyValue;
    }
    private void SetDrop_NF(int keyValue)
    {
        DungeonManager.instance.dungeonMaker.marker_Variable.markerVariable[1] = keyValue;
    }
    private void SetSpecialMonster_NF(int keyValue)
    {
        DungeonManager.instance.dungeonMaker.marker_Variable.markerVariable[2] = keyValue;
    }
    private void SetDamageBuffOnFloor_NF(int keyValue)
    {
        DungeonManager.instance.dungeonMaker.marker_Variable.markerVariable[3] = keyValue;
    }
    private void SetDamageBuffOnMonster_NF(int keyValue)
    {
        DungeonManager.instance.dungeonMaker.marker_Variable.markerVariable[4] = keyValue;
    }
    private void SetDamageBuffOnPlayer_NF(int keyValue)
    {
        DungeonManager.instance.dungeonMaker.marker_Variable.markerVariable[5] = keyValue;
    }
    private void SetPosHPOnMonster_NF(int keyValue)
    {
        DungeonManager.instance.dungeonMaker.marker_Variable.markerVariable[6] = keyValue;
    }
    private void SetNegHPOnMonster_NF(int keyValue)
    {
        DungeonManager.instance.dungeonMaker.marker_Variable.markerVariable[7] = keyValue;
    }
    private void SetPosDashSpeedOnPlayer_NF(int keyValue)
    {
        DungeonManager.instance.dungeonMaker.marker_Variable.markerVariable[8] = keyValue;
    }
    private void SetNegDashSpeedOnPlayer_NF(int keyValue)
    {
        DungeonManager.instance.dungeonMaker.marker_Variable.markerVariable[9] = keyValue;
    }
    private void SetPosAttackMulty_NF(int keyValue)
    {
        DungeonManager.instance.dungeonMaker.marker_Variable.markerVariable[10] = keyValue;
    }
    private void SetNegAttackMulty_NF(int keyValue)
    {
        DungeonManager.instance.dungeonMaker.marker_Variable.markerVariable[11] = keyValue;
    }
}
public class MarkerVariable
{
    public int[] markerVariable = new int[12];
    public int[] markerPreVariable = new int[12];

    public void Reset() // 던전 초기화 시 실행필수
    {
        markerVariable[(int)Markers.SetMonster_NF] = 0;
        markerVariable[(int)Markers.SetDrop_NF] = 0;
        markerVariable[(int)Markers.SetSpecialMonster_NF] = 0;
        markerVariable[(int)Markers.SetDamageBuffOnFloor_NF] = 1;
        markerVariable[(int)Markers.SetDamageBuffOnMonster_NF] = 1;
        markerVariable[(int)Markers.SetDamageBuffOnPlayer_NF] = 1;
        markerVariable[(int)Markers.SetPosHPOnMonster_NF] = 1;
        markerVariable[(int)Markers.SetNegHPOnMonster_NF] = 1;
        markerVariable[(int)Markers.SetPosDashSpeedOnPlayer_NF] = 0;
        markerVariable[(int)Markers.SetNegDashSpeedOnPlayer_NF] = 0;
        markerVariable[(int)Markers.SetPosAttackMulty_NF] = 1;
        markerVariable[(int)Markers.SetNegAttackMulty_NF] = 1;
    }
}
public class DungeonMarker : MonoBehaviour
{
    public GameObject marker;
    public GameObject markerEffect;
    
    public void SetMarker(Markers _markers)
    {
        switch (_markers)
        {
            case Markers.SetMonster_NF:{
                    markerEffect.GetComponent<SpriteRenderer>().sprite = SpriteSet.markerSprite[1];
                    marker.GetComponent<SpriteRenderer>().sprite = SpriteSet.markerSprite[15];
                }
                break;
            case Markers.SetDrop_NF:{
                    markerEffect.GetComponent<SpriteRenderer>().sprite = SpriteSet.markerSprite[1];
                    marker.GetComponent<SpriteRenderer>().sprite = SpriteSet.markerSprite[14];
                }
                break;
            case Markers.SetDamageBuffOnFloor_NF:{
                    markerEffect.GetComponent<SpriteRenderer>().sprite = SpriteSet.markerSprite[1];
                    marker.GetComponent<SpriteRenderer>().sprite = SpriteSet.markerSprite[13];
                }
                break;
            case Markers.SetDamageBuffOnMonster_NF:{
                    markerEffect.GetComponent<SpriteRenderer>().sprite = SpriteSet.markerSprite[2];
                    marker.GetComponent<SpriteRenderer>().sprite = SpriteSet.markerSprite[5];
                }
                break;
            case Markers.SetDamageBuffOnPlayer_NF:{
                    markerEffect.GetComponent<SpriteRenderer>().sprite = SpriteSet.markerSprite[3];
                    marker.GetComponent<SpriteRenderer>().sprite = SpriteSet.markerSprite[4];
                }
                break;
            case Markers.SetPosHPOnMonster_NF:{
                    markerEffect.GetComponent<SpriteRenderer>().sprite = SpriteSet.markerSprite[2];
                    marker.GetComponent<SpriteRenderer>().sprite = SpriteSet.markerSprite[5];
                }
                break;
            case Markers.SetNegHPOnMonster_NF:{
                    markerEffect.GetComponent<SpriteRenderer>().sprite = SpriteSet.markerSprite[3];
                    marker.GetComponent<SpriteRenderer>().sprite = SpriteSet.markerSprite[6];
                }
                break;
            case Markers.SetPosDashSpeedOnPlayer_NF:{
                    markerEffect.GetComponent<SpriteRenderer>().sprite = SpriteSet.markerSprite[3];
                    marker.GetComponent<SpriteRenderer>().sprite = SpriteSet.markerSprite[12];
                }
                break;
            case Markers.SetNegDashSpeedOnPlayer_NF:{
                    markerEffect.GetComponent<SpriteRenderer>().sprite = SpriteSet.markerSprite[2];
                    marker.GetComponent<SpriteRenderer>().sprite = SpriteSet.markerSprite[11];
                }
                break;
            case Markers.SetPosAttackMulty_NF:{
                    markerEffect.GetComponent<SpriteRenderer>().sprite = SpriteSet.markerSprite[3];
                    marker.GetComponent<SpriteRenderer>().sprite = SpriteSet.markerSprite[4];
                }
                break;
            case Markers.SetNegAttackMulty_NF:{
                    markerEffect.GetComponent<SpriteRenderer>().sprite = SpriteSet.markerSprite[2];
                    marker.GetComponent<SpriteRenderer>().sprite = SpriteSet.markerSprite[10];
                }
                break;
            case Markers.SetSpecialMonster_NF:{
                    markerEffect.GetComponent<SpriteRenderer>().sprite = SpriteSet.markerSprite[1];
                    marker.GetComponent<SpriteRenderer>().sprite = SpriteSet.markerSprite[9];
                }
                break;
            case Markers.Null:{
                    markerEffect.GetComponent<SpriteRenderer>().sprite = SpriteSet.markerSprite[1];
                    marker.GetComponent<SpriteRenderer>().sprite = SpriteSet.markerSprite[0];
                }
                break;
        }
    }
}
