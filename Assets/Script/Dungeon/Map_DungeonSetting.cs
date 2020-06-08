using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MapType
{
    tower,
    forest
}

public class Map_DungeonSetting : Map_ObjectSetting
{
    public MapType mapType;

    public GameObject[] spawner;
    public bool bossStage;
    public GameObject teleporter;
    public GameObject entrance;
}
