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
    public int monsterWeight;

    private bool eliteMonster;
    private bool bossMonster;
    
    public void Update()
    {
        if(!bossMonster)
            if(enemyHPBar != null)
                enemyHPBar.transform.position = new Vector3(
                    Camera.main.WorldToScreenPoint(transform.position).x, 
                    Camera.main.WorldToScreenPoint(transform.position).y + boxCollider2D.bounds.size.y * 100f + 10f, 
                    transform.position.z);
    }

    public void MonsterInit(int _monsterCode, bool _IsElite)
    {
        eliteMonster = _IsElite;
        boxCollider2D = GetComponent<BoxCollider2D>();
        
        EnemyStatInit(Database_Game.instance.GetMonsterStatus(_monsterCode));
        _currentHP = _HP;
        
        enemyHPBar = DungeonManager.instance.dungeonPoolManager.GetMonsterHpBar(eliteMonster);
        enemyHPBar.SetActive(true);
        enemyHPBar.GetComponent<EnemyHPBar>().SetMonster(this);
        bossMonster = false;
    }
    public void BossMonsterInit(int _bossMonsterCode)
    {
        EnemyStatInit(Database_Game.instance.GetMonsterStatus(_bossMonsterCode));
        _currentHP = _HP;
        
        enemyHPBar = DungeonManager.instance.dungeonPoolManager.GetBossMonsterHpBar();
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
        monsterWeight = _monster.monsterWeight;
    }
    public void IncreaseHP(int _Damage)
    {
        _currentHP += _Damage;
        if(_currentHP > _HP)
        {
            _currentHP = _HP;
        }
        enemyHPBar.GetComponent<EnemyHPBar>().SetHPBar();
    }
    public int DecreaseHP(int _Damage)
    {
        _currentHP -= _Damage;
        if (_currentHP <= 0)
        {
            _currentHP = 0;
        }
        enemyHPBar.GetComponent<EnemyHPBar>().SetHPBar();
        return _Damage;
    }

    public bool IsDeadCheck()
    {
        if (_currentHP <= 0)
        {
            HPbarReset();
            return true;
        }
        return false;
    }
    public void HPbarReset()
    {
        if (enemyHPBar == null) return;

        enemyHPBar.GetComponent<EnemyHPBar>().MonsterDie();

        if (eliteMonster)
        {
            DungeonManager.instance.dungeonPoolManager.EliteMonsterDie(enemyHPBar);
        }
        else
        {
            DungeonManager.instance.dungeonPoolManager.MonsterDie(bossMonster, enemyHPBar);
        }
        enemyHPBar.SetActive(false);
        enemyHPBar = null;
    }

    public void Set_Attack(int _value)
    {
        _attack += _value;
    }
    public void Set_Defense(int _value)
    {
        _defense += _value;
    }
    public void Set_Hp(int _value, bool _upgrade)
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
    public float GetMoveSpeed()
    {
        return _moveSpeed;
    }
    public int GetAttack()
    {
        return _attack;
    }
}
