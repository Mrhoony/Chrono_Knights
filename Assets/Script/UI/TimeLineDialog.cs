using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeLineDialog : MonoBehaviour
{
    public GameObject chaseGameObject;

    private void OnEnable()
    {
        if(chaseGameObject != null)
        {
            transform.position = Camera.main.WorldToScreenPoint(chaseGameObject.transform.position);
        }
    }
}
