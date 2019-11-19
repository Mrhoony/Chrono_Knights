using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SystemData
{
    public int screen_width;
    public int screen_height;

    public float volume_BGM;
    public float volume_Effect;

    public Dictionary<string, KeyCode> currentDictionary;

    public void Init()
    {
        currentDictionary = KeyBindManager.instance.KeyBinds;
    }
}
