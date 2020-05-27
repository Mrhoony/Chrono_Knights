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
    Queue<GameObject> eliteMonsterHpBarQueue = new Queue<GameObject>();
    Queue<GameObject> bossMonsterHpBarQueue = new Queue<GameObject>();
    
    public void Awake()
    {
        instance = this;
        bossMonsterCount = 0;
        CreateHpBar(20);
        CreateEliteBar(10);
        CreateBossHpBar(3);
    }

    public GameObject GetMonsterHpBar(bool _IsElite)
    {
        if (_IsElite)
        {
            if (eliteMonsterHpBarQueue.Count < 1)
            {
                CreateHpBar(10);
            }
            return eliteMonsterHpBarQueue.Dequeue();
        }
        else
        {
            if (monsterHpBarQueue.Count < 1)
            {
                CreateHpBar(10);
            }
            return monsterHpBarQueue.Dequeue();
        }
    }

    public GameObject GetBossMonsterHpBar()
    {
        if (bossMonsterHpBarQueue.Count < 1)
        {
            CreateBossHpBar(1);
        }
        GameObject hpBar = bossMonsterHpBarQueue.Dequeue();
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
    public void CreateEliteBar(int _amount)
    {
        GameObject hpBar;
        for (int i = 0; i < _amount; ++i)
        {
            hpBar = Instantiate(eliteMonsterHpBar, Vector2.zero, Quaternion.identity);
            hpBar.transform.SetParent(transform.GetChild(0).transform);
            hpBar.SetActive(false);
            eliteMonsterHpBarQueue.Enqueue(hpBar);
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
            bossMonsterHpBarQueue.Enqueue(hpBar);
        }
    }
    public void EliteMonsterDie(GameObject _hpBar)
    {
        eliteMonsterHpBarQueue.Enqueue(_hpBar);
    }
    public void MonsterDie(bool _bossMonster, GameObject _hpBar)
    {
        if (_bossMonster)
        {
            bossMonsterHpBarQueue.Enqueue(_hpBar);
        }
        else
        {
            monsterHpBarQueue.Enqueue(_hpBar);
        }
    }
}
