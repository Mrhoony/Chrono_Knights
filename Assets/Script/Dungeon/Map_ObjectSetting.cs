using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MapType
{
    tower,
    forest
}

public class Map_ObjectSetting : MonoBehaviour
{
    public MapType mapType;
    public GameObject[] spawner;
    public GameObject teleporter;
    public GameObject entrance;

    public bool bossStage;
}
