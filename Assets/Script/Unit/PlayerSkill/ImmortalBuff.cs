using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImmortalBuff
{
    public void OnSkill(PlayerStatus _PlayerStatus)
    {
        _PlayerStatus.immortalBuffOn = true;
    }
    public void OffSKill(PlayerStatus _PlayerStatus)
    {
        _PlayerStatus.immortalBuffOn = false;
    }
}
