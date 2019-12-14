using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatus : MonoBehaviour
{
    private float _moveSpeed; // 이동 속도
    private int _HP; // 최대 체력
    public int _currentHP; // 현재 체력
    private int _attack; // 공격력

    private void Start()
    {
        _moveSpeed = 1f;
        _HP = 5;
        _attack = 2;
    }

    public void IncreaseHP(int damage)
    {
        _currentHP += damage;
        if(damage > _HP)
        {
            _currentHP = _HP;
        }
    }
    public void DecreaseHP(int damage)
    {
        _currentHP -= damage;
        if (_currentHP <= 0)
        {
            _currentHP = 0;
        }
    }
    #region
    public void EnemyStatInit(float moveSpeed, int HP, int attack)
    {
        _moveSpeed = moveSpeed;
        _HP = HP;
        _attack = attack;
    }
    public void Set_attack(int value)
    {
        _attack = value;
    }
    #endregion      // Set 메소드
    
    public void SetCurrentHP()
    {
        _currentHP = _HP;
    }
    public float GetMoveSpeed()
    {
        return _moveSpeed;
    }
    public int GetAttack()
    {
        return _attack;
    }
    public bool IsDeadCheck()
    {
        if(_currentHP <= 0)
        {
            _currentHP = 0;
            return true;
        }
        return false;
    }
}
