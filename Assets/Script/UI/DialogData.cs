using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[Serializable]
public class DialogDataSet : PlayableBehaviour
{
    public string text = "";
    public Color color = Color.white;

    public TimeLineDialog timeLineDialog;
    public bool notice;
}

[Serializable]
public class DialogData : PlayableAsset, ITimelineClipAsset
{
    public DialogDataSet subtitleData = new DialogDataSet();

    // Create the runtime version of the clip, by creating a copy of the template
    public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
    {
        return ScriptPlayable<DialogDataSet>.Create(graph, subtitleData);
    }

    // Use this to tell the Timeline Editor what features this clip supports
    public ClipCaps clipCaps
    {
        get { return ClipCaps.Blending | ClipCaps.Extrapolation; }
    }
}
