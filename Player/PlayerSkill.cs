using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkill : MonoBehaviour
{   // 타겟 설정을 위해 필요
    public LayerMask Enemys;
    public static Transform Target;
    public static Vector3 Dir;

    public SkillSO SSO;
    

    private Skill _magnet; // 1
    private Skill _swordwave; // 2
    private Skill _defend; // 3
    private Skill _healUp; // 4
    private Skill _hpDecreases; // 5
    private Skill _effect; // 6
    private Skill _Sheild; // 7
    private Skill _laser; // 8
    private Skill _blaze; // 9
    private Skill _bookOfWisdom; // 10
    private Skill _bookOfCoolDown;

    private bool _isSwordwaveDelay = true;
    private bool _isShieldDelay = true;
    private bool _isLaserDelay = true;
    private bool _isBlazeDelay = true;

    public GameObject DaggerAxis;
    public GameObject Swordwave;
    public GameObject Shield;
    public GameObject Laser;
    public GameObject Blaze;
    void Start()
    {   
        DaggerAxis = GameObject.FindWithTag("DaggerAxis");
        _magnet = SSO.Skills[1];
        _swordwave = SSO.Skills[2];
        _defend = SSO.Skills[3];
        _healUp = SSO.Skills[4];
        _hpDecreases = SSO.Skills[5];
        _effect = SSO.Skills[6];
        _Sheild = SSO.Skills[7];
        _laser = SSO.Skills[8];
        _blaze = SSO.Skills[9];
        _bookOfWisdom = SSO.Skills[10];
        _bookOfCoolDown = SSO.Skills[11];
    }

    void Update()
    {
        if (PlayerSkillManager.Instance.SkillList.ContainsKey(0))
        {
            // 대거축 스크립트 참조
            var axis = DaggerAxis.GetComponent<WhiringDaggerAxis>();
            // 대거축 함수의 대거 갯수 추가 로직 실행
            // 레벨은 0으로 설정되어 있으니 0 + 1만큼 추가
            axis.WeaponReset(PlayerSkillManager.Instance.SkillList[0] + 1);
        }
        if (PlayerSkillManager.Instance.SkillList.ContainsKey(1))
        {
            PlayerStatsManager.Instance.BonusRadius = _magnet.LevelValue[PlayerSkillManager.Instance.SkillList[_magnet.Key]];
        }
        if (PlayerSkillManager.Instance.SkillList.ContainsKey(2) && _isSwordwaveDelay)
        {
            _isSwordwaveDelay = false;
            SetTarget();
        }
        if (PlayerSkillManager.Instance.SkillList.ContainsKey(3))
        {
            PlayerStatsManager.Instance.Defend = _defend.LevelValue[PlayerSkillManager.Instance.SkillList[_defend.Key]];
        }
        if (PlayerSkillManager.Instance.SkillList.ContainsKey(4))
        {
            PlayerStatsManager.Instance.HealUp = _healUp.LevelValue[PlayerSkillManager.Instance.SkillList[_healUp.Key]];
        }
        if (PlayerSkillManager.Instance.SkillList.ContainsKey(5))
        {
            PlayerStatsManager.Instance.HpDecrease = _hpDecreases.LevelValue[PlayerSkillManager.Instance.SkillList[_hpDecreases.Key]];
        }
        if (PlayerSkillManager.Instance.SkillList.ContainsKey(6))
        {
            PlayerSkillManager.Instance.IsEffect = true;
        }
        if (PlayerSkillManager.Instance.SkillList.ContainsKey(7) && _isShieldDelay)
        {
            _isShieldDelay = false;
            OnShield();
        }
        if (PlayerSkillManager.Instance.SkillList.ContainsKey(8) && _isLaserDelay)
        {
            OnLaser();
        }
        if (PlayerSkillManager.Instance.SkillList.ContainsKey(9) && _isBlazeDelay)
        {
            _isBlazeDelay = false;
            BlazeCreat();
        }
        if(PlayerSkillManager.Instance.SkillList.ContainsKey(10))
        {
            PlayerStatsManager.Instance.EXPUp = _bookOfWisdom.LevelValue[PlayerSkillManager.Instance.SkillList[_bookOfWisdom.Key]];
        }
        if(PlayerSkillManager.Instance.SkillList.ContainsKey(11))
        {
            PlayerStatsManager.Instance.CoolDown = _bookOfCoolDown.LevelValue[PlayerSkillManager.Instance.SkillList[_bookOfCoolDown.Key]];
        }
    }
    // 검기 로직 --------------------------------------------

    void SetTarget()
    {
        Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, 20f, Enemys);
        float dis = float.MaxValue;

        if (cols.Length > 0)
        {
            for (int i = 0; i < cols.Length; i++)
            {
                float tempDis = Vector3.Distance(transform.position, cols[i].transform.position);
                if (dis > tempDis)
                {
                    dis = tempDis;
                    Target = cols[i].transform;
                }
            }
            Dir = (Target.transform.position - transform.position).normalized;
            Instantiate(Swordwave, transform.position, Quaternion.identity);
            StartCoroutine(CoSwordwaveOn());
        }
        else
        {
            _isSwordwaveDelay = true;
        }
    }
    IEnumerator CoSwordwaveOn()
    {
        

        yield return new WaitForSeconds(_swordwave.LevelValue[PlayerSkillManager.Instance.SkillList[_swordwave.Key]]);
        _isSwordwaveDelay = true;
    }





    // 쉴드 로직 --------------------------------------
    void OnShield()
    {
        Shield.SetActive(true);
        Player.IsHit = false;
        StartCoroutine(CoShield());
    }

    IEnumerator CoShield()
    {
        yield return new WaitForSeconds(_Sheild.LevelValue[ PlayerSkillManager.Instance.SkillList[_Sheild.Key]]);
        Shield.SetActive(false);
        Player.IsHit = true;
        StartCoroutine(CoShieldDelay());
    }
    IEnumerator CoShieldDelay()
    {
        yield return new WaitForSeconds(9);
        _isShieldDelay = true;
    }

    // 레이저 로직 --------------------------------
    void OnLaser()
    {
        if (_isLaserDelay == false)
            return;
        else
        {
            _isLaserDelay = false;
            StartCoroutine(CoLaserOn());
        }
    }
    IEnumerator CoLaserOn()
    {
        Instantiate(Laser, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(_laser.LevelValue[ PlayerSkillManager.Instance.SkillList[_laser.Key]]);
        _isLaserDelay = true;
    }
    // 블레이즈 소환 로직 -------------------------------------------------
    void BlazeCreat()
    {
        
        for (int i = 0; i < _blaze.LevelValue[PlayerSkillManager.Instance.SkillList[_blaze.Key]]; i++)
        {
            Instantiate(Blaze, transform.position, Quaternion.identity);
        }
        StartCoroutine(CoBlazeDelay());
    }
    IEnumerator CoBlazeDelay()
    {
        yield return new WaitForSeconds(5);
        _isBlazeDelay = true;
    }
}
