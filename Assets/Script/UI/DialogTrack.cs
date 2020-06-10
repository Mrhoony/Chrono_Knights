using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.UI;

[TrackColor(0.0f, 0.3f, 0.7f)]
[TrackClipType(typeof(DialogData))]
[TrackBindingType(typeof(TimeLineDialog))]

public class DialogTrack : TrackAsset
{
    public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
    {
        return ScriptPlayable<DialogMixer>.Create(graph, inputCount);
    }
}
