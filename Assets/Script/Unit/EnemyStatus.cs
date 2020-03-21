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
            if(enemyHPBar != null)
                enemyHPBar.transform.position = new Vector3(Camera.main.WorldToScreenPoint(transform.position).x, Camera.main.WorldToScreenPoint(transform.position).y + boxCollider2D.bounds.size.y * 100f + 10f, transform.position.z);
    }

    public void MonsterInit(int _monsterCode)
    {
        boxCollider2D = GetComponent<BoxCollider2D>();

        Debug.Log(_monsterCode);
        
        EnemyStatInit(Database_Game.instance.GetMonsterStatus(_monsterCode));
        _currentHP = _HP;
        
        enemyHPBar = DungeonPoolManager.instance.GetMonsterHpBar();
        enemyHPBar.SetActive(true);
        enemyHPBar.GetComponent<EnemyHPBar>().SetMonster(this);
        bossMonster = false;
    }
    public void BossMonsterInit(int _bossMonsterCode)
    {
        EnemyStatInit(Database_Game.instance.GetMonsterStatus(_bossMonsterCode));
        _currentHP = _HP;
        
        enemyHPBar = DungeonPoolManager.instance.GetBossMonsterHpBar();
        enemyHPBar.SetActive(true);
        enemyHPBar.GetComponent<EnemyHPBar>().SetMonster(this);
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
            HPbarReset();
            return true;
        }
        return false;
    }
    public void HPbarReset()
    {
        if (enemyHPBar == null) return;
        enemyHPBar.GetComponent<EnemyHPBar>().MonsterDie();
        enemyHPBar.SetActive(false);
        DungeonPoolManager.instance.MonsterDie(bossMonster, enemyHPBar);
        enemyHPBar = null;
    }

    public void Set_def(int _value) {
        _defense += _value;
    }

    public void Set_hp(int _value, bool _upgrade)
    {
        if (_upgrade)
        {
            _HP *= _value;
        }
        else
        {
            _HP /= _value;
        }
        _currentHP = _HP;
    }
    public void Set_attack(int _value)
    {
        _attack += _value;
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
