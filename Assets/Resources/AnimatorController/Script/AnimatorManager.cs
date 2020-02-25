using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorManager : StateMachineBehaviour
{
    public PlayerControl playerControl;
    public int multyHitCount;
    public bool move;

    public void Init()
    {
        move = false;
        multyHitCount = 0;
    }
    public void AnimationInit()
    {
        move = false;
        multyHitCount = 0;
        playerControl = PlayerControl.instance;
    }
}
