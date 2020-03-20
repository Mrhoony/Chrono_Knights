using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AtkType {
    notMove, oneStep, fowardDash, backOneStep, backDash, fowardBack, backJump
}

public class AnimatorManager : StateMachineBehaviour
{
    public PlayerControl playerControl;
    public bool move;
   
    public void Init()
    {
        playerControl = PlayerControl.instance;
        move = false;
    }
}
