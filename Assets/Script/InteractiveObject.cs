using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InteractiveObjectType
{
    NPC,
    Teleport,
    Trap,
    Guide
}

public class InteractiveObject : MonoBehaviour
{
    public int objectNumber;   // 상호작용 하는 오브젝트에 할당된 번호. 일반 1~100, npc 101~
    public GameObject player;
    public bool inPlayer;
    public InteractiveObjectType objectType;
}
