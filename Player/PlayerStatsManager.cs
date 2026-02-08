using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatsManager : MonoBehaviour
{
    #region Singleton
    private static PlayerStatsManager s_instance = null;

    public static PlayerStatsManager Instance
    {
        get
        {
            if (s_instance == null)
                return null;
            return s_instance;
        }
    }
    #endregion
    private void Awake()
    {
        #region Singleton
        if (s_instance == null)
        {
            s_instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        #endregion
    }
   
    private float _currentHp = 100;
    private float _maxHp = 100;

    private float _currentExp = 0;
    private float _maxExp = 100;

    private float _defend = 0;

    private float _healUp = 0;

    private float _hpDecrease = 0;

    private float _expUp = 0;

    private float _coolDown = 0;
    
    private float _radius = 1;
    private float _bonusRadius = 0;

    // 궁 관련
    private float _currentScale = 1.5f;
    private float _ultScale = 3.5f;
    private float _enemySpeed = 3;
    private float _currentRotationSpeed = 3;
    private bool _hardUlt = false;
    public float CurrentRotationSpeed
    {
        get { return _currentRotationSpeed; }
        set { _currentRotationSpeed = value; }
    }
    public float EnemySpeed
    {
        get { return _enemySpeed; }
        set { _enemySpeed = value; }
    }

    

    public float CurrentHp
    {
        get { return _currentHp; }
        set 
        { 
            if (value >= _maxHp)
            {
                value = _maxHp;
            }
            else
                _currentHp = value; 
        }
    }
    public float MaxHp
    {
        get { return _maxHp; }
        set { _maxHp = value; }
    }

    public float CurrentExp
    {
        get { return _currentExp; }
        set { _currentExp = value; }
    }
    public float MaxExp
    {
        get{ return _maxExp; } 
        set { _maxExp = value; }
    }

    public float Defend
    {
        get { return (100 -  _defend) / 100; }
        set { _defend = value; }
    }
    public float HpDecrease
    {
        get { return (100 - _hpDecrease) /100; }
        set { _hpDecrease = value; }
    }

    public float HealUp
    {
        get { return (100 + _healUp) / 100; }
        set { _healUp = value; }
    }
    public float Radius
    {
        get { return _radius + _bonusRadius; }
        set { _radius = value; }
    }
    public float BonusRadius
    {
        get { return _bonusRadius; }
        set { _bonusRadius = value; }
    }
    public float CurrentScale
    {
        get { return _currentScale; }
        set { _currentScale = value; }
    }
    public float UltScale
    {
        get { return _ultScale; }
    }
    public bool HardUlt
    {
        get { return _hardUlt; }
        set { _hardUlt = value; }
    }
    // 경험치 증가
    public float EXPUp
    {
        get { return (100 + _expUp) / 100; }
        set { _expUp = value; }
    }
    public float CoolDown
    {
        get { return (100 + _coolDown) / 100; }
        set { _coolDown = value; }
    }
}
