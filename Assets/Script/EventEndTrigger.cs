using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventEndTrigger : MonoBehaviour
{
    private void OnEnable()
    {
        CameraManager.instance.mainScenarioOn = false;
        CanvasManager.instance.MainScenarioEnd();
    }
}
