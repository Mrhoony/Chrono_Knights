using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonMarker : MonoBehaviour
{
    public GameObject marker;
    public GameObject markerEffect;
    
    public void SetMarker(Markers _markers)
    {
        switch (_markers)
        {
            case Markers.SetMonster_NF:
                {
                    markerEffect.GetComponent<SpriteRenderer>().sprite = SpriteSet.markerSprite[1];
                    marker.GetComponent<SpriteRenderer>().sprite = SpriteSet.markerSprite[15];
                }
                break;
            case Markers.SetDrop_NF:
                {
                    markerEffect.GetComponent<SpriteRenderer>().sprite = SpriteSet.markerSprite[1];
                    marker.GetComponent<SpriteRenderer>().sprite = SpriteSet.markerSprite[14];
                }
                break;
            case Markers.SetDamageBuffOnFloor_NF:
                {
                    markerEffect.GetComponent<SpriteRenderer>().sprite = SpriteSet.markerSprite[1];
                    marker.GetComponent<SpriteRenderer>().sprite = SpriteSet.markerSprite[13];
                }
                break;
            case Markers.SetDamageBuffOnMonster_NF:
                {
                    markerEffect.GetComponent<SpriteRenderer>().sprite = SpriteSet.markerSprite[2];
                    marker.GetComponent<SpriteRenderer>().sprite = SpriteSet.markerSprite[5];
                }
                break;
            case Markers.SetDamageBuffOnPlayer_NF:
                {
                    markerEffect.GetComponent<SpriteRenderer>().sprite = SpriteSet.markerSprite[3];
                    marker.GetComponent<SpriteRenderer>().sprite = SpriteSet.markerSprite[4];
                }
                break;
            case Markers.SetPosHPOnMonster_NF:
                {
                    markerEffect.GetComponent<SpriteRenderer>().sprite = SpriteSet.markerSprite[2];
                    marker.GetComponent<SpriteRenderer>().sprite = SpriteSet.markerSprite[5];
                }
                break;
            case Markers.SetNegHPOnMonster_NF:
                {
                    markerEffect.GetComponent<SpriteRenderer>().sprite = SpriteSet.markerSprite[3];
                    marker.GetComponent<SpriteRenderer>().sprite = SpriteSet.markerSprite[6];
                }
                break;
            case Markers.SetPosDashSpeedOnPlayer_NF:
                {
                    markerEffect.GetComponent<SpriteRenderer>().sprite = SpriteSet.markerSprite[3];
                    marker.GetComponent<SpriteRenderer>().sprite = SpriteSet.markerSprite[12];
                }
                break;
            case Markers.SetNegDashSpeedOnPlayer_NF:
                {
                    markerEffect.GetComponent<SpriteRenderer>().sprite = SpriteSet.markerSprite[2];
                    marker.GetComponent<SpriteRenderer>().sprite = SpriteSet.markerSprite[11];
                }
                break;
            case Markers.SetPosAttackMulty_NF:
                {
                    markerEffect.GetComponent<SpriteRenderer>().sprite = SpriteSet.markerSprite[3];
                    marker.GetComponent<SpriteRenderer>().sprite = SpriteSet.markerSprite[4];
                }
                break;
            case Markers.SetNegAttackMulty_NF:
                {
                    markerEffect.GetComponent<SpriteRenderer>().sprite = SpriteSet.markerSprite[2];
                    marker.GetComponent<SpriteRenderer>().sprite = SpriteSet.markerSprite[10];
                }
                break;
            case Markers.SetSpecialMonster_NF:
                {
                    markerEffect.GetComponent<SpriteRenderer>().sprite = SpriteSet.markerSprite[1];
                    marker.GetComponent<SpriteRenderer>().sprite = SpriteSet.markerSprite[9];
                }
                break;
        }
    }
}
