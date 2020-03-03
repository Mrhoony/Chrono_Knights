using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatus : MonoBehaviour
{
    public GameObject enemyHPBar;
    public Monster monster;
    public BoxCollider2D boxCollider2D;

    public string monsterName;
    public int _HP; // 최대 체력
    public float _currentHP; // 현재 체력
    public float _moveSpeed; // 이동 속도
    public float _attackSpeed;
    public float _attackRange;
    public int _attack; // 공격력
    public int _defense;

    private bool bossMonster;
    
    public void Update()
    {
        if(!bossMonster)
            enemyHPBar.transform.position = new Vector3(Camera.main.WorldToScreenPoint(transform.position).x, Camera.main.WorldToScreenPoint(transform.position).y + boxCollider2D.bounds.size.y + 100f, transform.position.z);
    }

    public void MonsterInit(int _monsterCode)
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
        
        EnemyStatInit(Database_Game.instance.GetMonsterStatus(_monsterCode));
        _currentHP = _HP;
        
        int hpbarNum = GameObject.Find("DungeonUI/DungeonPoolManager/HealthBarPool").transform.childCount;

        for (int i = 0; i < hpbarNum; ++i)
        {
            if (!GameObject.Find("DungeonUI/DungeonPoolManager/HealthBarPool").transform.GetChild(i).GetComponent<EnemyHPBar>().SetMonster(this))
            {
                continue;
            }
            enemyHPBar = GameObject.Find("DungeonUI/DungeonPoolManager/HealthBarPool").transform.GetChild(i).gameObject;
            break;
        }

        if(enemyHPBar == null)
        {
            enemyHPBar = Instantiate(Resources.Load("Prefabs/Unit/Mob/HPbar"), Vector3.zero, Quaternion.identity) as GameObject;
            enemyHPBar.transform.parent = GameObject.Find("DungeonUI/DungeonPoolManager/HealthBarPool").transform;
            enemyHPBar.GetComponent<EnemyHPBar>().SetMonster(this);
        }
        enemyHPBar.SetActive(true);
        enemyHPBar.GetComponent<EnemyHPBar>().SetHPBar();
        bossMonster = false;
    }

    public void BossMonsterInit(int _bossMonsterCode)
    {
        EnemyStatInit(Database_Game.instance.GetMonsterStatus(_bossMonsterCode));
        _currentHP = _HP;

        int hpbarNum = GameObject.Find("DungeonUI/DungeonPoolManager/BossHealthBarPool").transform.childCount;
        for (int i = 0; i < hpbarNum; ++i)
        {
            if (!GameObject.Find("DungeonUI/DungeonPoolManager/BossHealthBarPool").transform.GetChild(i).GetComponent<EnemyHPBar>().SetMonster(this))
            {
                continue;
            }
            enemyHPBar = GameObject.Find("DungeonUI/DungeonPoolManager/BossHealthBarPool").transform.GetChild(i).gameObject;
            break;
        }

        if (enemyHPBar == null)
        {
            enemyHPBar = Instantiate(Resources.Load("Prefabs/Unit/Mob/HPbar"), Vector3.zero, Quaternion.identity) as GameObject;
            enemyHPBar.transform.parent = GameObject.Find("DungeonUI/DungeonPoolManager/BossHealthBarPool").transform;
            enemyHPBar.GetComponent<EnemyHPBar>().SetMonster(this);
        }
        enemyHPBar.SetActive(true);
        enemyHPBar.GetComponent<EnemyHPBar>().SetHPBar();
        bossMonster = true;
    }

    public void EnemyStatInit(Monster _monster)
    {
        monsterName = _monster.monsterName;
        _HP = _monster.monsterHP;
        _moveSpeed = _monster.monsterMoveSpeed;
        _attackSpeed = _monster.monsterAttackSpeed;
        _attackRange = _monster.monsterAttackRange;
        _attack = _monster.monsterAttack;
        _defense = _monster.monsterDefense;
    }

    public void IncreaseHP(int damage)
    {
        _currentHP += damage;
        if(_currentHP > _HP)
        {
            _currentHP = _HP;
        }
        enemyHPBar.GetComponent<EnemyHPBar>().SetHPBar();
    }
    public void DecreaseHP(int damage)
    {
        _currentHP -= damage;
        if (_currentHP <= 0)
        {
            _currentHP = 0;
        }
        enemyHPBar.GetComponent<EnemyHPBar>().SetHPBar();
    }

    public bool IsDeadCheck()
    {
        if (_currentHP <= 0)
        {
            _currentHP = 0;
            enemyHPBar.GetComponent<EnemyHPBar>().MonsterDie();
            enemyHPBar.SetActive(false);
            enemyHPBar = null;
            return true;
        }
        return false;
    }

    public void Set_attack(int value)
    {
        _attack = value;
    }
    public float GetMoveSpeed()
    {
        return _moveSpeed;
    }
    public int GetAttack()
    {
        return _attack;
    }
}
