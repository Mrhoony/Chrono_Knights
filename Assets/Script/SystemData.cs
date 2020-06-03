using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SystemData
{
    public int screenSizeNumber;

    public bool cameraShakeOnOff;

    public float volume_BGM;
    public float volume_Effect;
    
    public Dictionary<string, KeyCode> currentDictionary;

    public void Init(KeyBindManager _KeyBindManager)
    {
        currentDictionary = _KeyBindManager.KeyBinds;
        cameraShakeOnOff = true;
    }
}
