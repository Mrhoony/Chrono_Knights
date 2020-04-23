using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuraSpear
{
    public void OnSkill(PlayerStatus _PlayerStatus)
    {
        _PlayerStatus.auraAttackOn = true;
    }
    public void OffSKill(PlayerStatus _PlayerStatus)
    {
        _PlayerStatus.auraAttackOn = false;
    }
}
