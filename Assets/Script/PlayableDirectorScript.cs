using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class PlayableDirectorScript : MonoBehaviour
{
    public PlayableDirector playableDirector;

    public void OnEnable()
    {
        playableDirector = GetComponent<PlayableDirector>();
    }

    public void PlayerPause()
    {
        playableDirector.playableGraph.GetRootPlayable(0).SetSpeed(0);
    }
    public void PlayerRestart()
    {
        playableDirector.playableGraph.GetRootPlayable(0).SetSpeed(1);
    }
}
