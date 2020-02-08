using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatus : MonoBehaviour
{
    public GameObject enemyHPBar;
    public BoxCollider2D boxCollider2D;
    private float _moveSpeed; // 이동 속도
    private int _HP; // 최대 체력
    private int _currentHP; // 현재 체력
    private int _attack; // 공격력
    public bool isMove;
    
    public void Update()
    {
        if (!isMove) return;
        enemyHPBar.transform.position = new Vector3(transform.position.x, transform.position.y + boxCollider2D.bounds.size.y + 0.2f, transform.position.z);
    }

    public void MonsterInit()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();

        EnemyStatInit(1f, 5, 1);
        _currentHP = _HP;

        /*
        int hpbarNum = GameObject.Find("HealthBarPool").transform.childCount;
        for (int i = 0; i < hpbarNum; ++i)
        {
            if (!GameObject.Find("HealthBarPool").transform.GetChild(i).GetComponent<EnemyHPBar>().SetMonster())
            {
                continue;
            }
            enemyHPBar = GameObject.Find("HealthBarPool").transform.GetChild(i).gameObject;
            break;
        }
        if(enemyHPBar == null)
        {
            enemyHPBar = Instantiate(Resources.Load("Prefabs/Unit/Mob/HPbar"), Vector3.zero, Quaternion.identity) as GameObject;
            enemyHPBar.GetComponent<EnemyHPBar>().SetMonster();
        }
        */

        enemyHPBar = Instantiate(Resources.Load("Prefabs/Unit/Mob/HPbar"), Vector3.zero, Quaternion.identity) as GameObject;
        enemyHPBar.GetComponent<EnemyHPBar>().SetMonster();
        enemyHPBar.SetActive(true);
        isMove = true;
    }
    public void EnemyStatInit(float moveSpeed, int HP, int attack)
    {
        _moveSpeed = moveSpeed;
        _HP = HP;
        _attack = attack;
    }

    public void IncreaseHP(int damage)
    {
        _currentHP += damage;
        if(_currentHP > _HP)
        {
            _currentHP = _HP;
        }
        enemyHPBar.GetComponent<EnemyHPBar>().SetHPBar(_currentHP, _HP);
    }
    public void DecreaseHP(int damage)
    {
        _currentHP -= damage;
        if (_currentHP <= 0)
        {
            _currentHP = 0;
        }
        enemyHPBar.GetComponent<EnemyHPBar>().SetHPBar(_currentHP, _HP);
    }

    public bool IsDeadCheck()
    {
        if (_currentHP <= 0)
        {
            _currentHP = 0;
            enemyHPBar.SetActive(false);
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
