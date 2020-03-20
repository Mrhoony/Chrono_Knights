using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
