using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStat : MonoBehaviour
{
    private float _moveSpeed; // 이동 속도
    private float _HP; // 최대 체력
    private float _currentHP; // 현재 체력
    private int _attack; // 공격력
    Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
        _moveSpeed = 1f;
        _HP = 5f;
        _attack = 2;
    }

    public void SetCurrentHP()
    {
        _currentHP = _HP;
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
        _currentHP = _currentHP - damage;
        if (_currentHP <= 0)
        {
            _currentHP = 0;
        }
    }
    #region
    public void EnemyStatInit(float moveSpeed, float HP, int attack)
    {
        _moveSpeed = moveSpeed;
        _HP = HP;
        _attack = attack;
    }
    public void SetMoveSpeed(int value)
    {
        _moveSpeed = value;
    }
    public void Set_HP(int value)
    {
        _HP = value;
    }
    public void Set_attack(int value)
    {
        _attack = value;
    }
    #endregion      // Set 메소드
    
    public float GetCurrentHP()
    {
        return _currentHP;
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
