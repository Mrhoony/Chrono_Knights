using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonPoolManager : MonoBehaviour
{
    public static DungeonPoolManager instance;
    public GameObject monsterHpBar;
    public GameObject eliteMonsterHpBar;
    public GameObject bossMonsterHpBar;

    public GameObject monsterHpBarPool;
    public int bossMonsterCount;

    Queue<GameObject> monsterHpBarQueue = new Queue<GameObject>();
    Queue<GameObject> BossMonsterHpBarQueue = new Queue<GameObject>();
    
    public void Awake()
    {
        instance = this;
        bossMonsterCount = 0;
        CreateHpBar(20);
        CreateBossHpBar(3);
    }

    public GameObject GetMonsterHpBar()
    {
        if (monsterHpBarQueue.Count < 1)
        {
            CreateHpBar(10);
        }
        return monsterHpBarQueue.Dequeue();
    }

    public GameObject GetBossMonsterHpBar()
    {
        if (BossMonsterHpBarQueue.Count < 1)
        {
            CreateBossHpBar(1);
        }
        GameObject hpBar = BossMonsterHpBarQueue.Dequeue();
        ++bossMonsterCount;
        hpBar.transform.position = transform.position + Vector3.down * (420 - (70 * bossMonsterCount));
        return hpBar;
    }
    public void bossMonsterCountReset()
    {
        bossMonsterCount = 0;
    }

    public void CreateHpBar(int _amount)
    {
        GameObject hpBar;
        for (int i = 0; i < _amount; ++i)
        {
            hpBar = Instantiate(monsterHpBar, Vector2.zero, Quaternion.identity);
            hpBar.transform.SetParent(transform.GetChild(0).transform);
            hpBar.SetActive(false);
            monsterHpBarQueue.Enqueue(hpBar);
        }
    }
    public void CreateBossHpBar(int _amount)
    {
        GameObject hpBar;
        for (int i = 0; i < _amount; ++i)
        {
            hpBar = Instantiate(bossMonsterHpBar, Vector2.zero, Quaternion.identity);
            hpBar.transform.SetParent(transform.GetChild(1).transform);
            hpBar.SetActive(false);
            BossMonsterHpBarQueue.Enqueue(hpBar);
        }
    }
    public void MonsterDie(bool _bossMonster, GameObject _hpBar)
    {
        if (_bossMonster)
        {
            BossMonsterHpBarQueue.Enqueue(_hpBar);
        }
        else
        {
            monsterHpBarQueue.Enqueue(_hpBar);
        }
    }
}
