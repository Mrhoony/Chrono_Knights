using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public int frameRate = 20;
    // Start is called before the first frame update
    void Start()
    {
        Time.captureFramerate = frameRate;
    }
}
