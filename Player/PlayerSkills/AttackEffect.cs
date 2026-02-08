using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackEffect : MonoBehaviour
{
    public SkillSO SSO;
    private Skill _skill;
    private CircleCollider2D _cc;
    private SpriteRenderer _sr;
    private float _hue = 0.3f;
    private void Awake()
    {
        _skill = SSO.Skills[6];
        _cc = GetComponent<CircleCollider2D>(); 
        _sr = GetComponent<SpriteRenderer>();
    }
    private void OnEnable()
    {
        _hue = 0.3f;
        _sr.color = Color.HSVToRGB(_hue, 0.7f, 1f);
    }
    void Update()
    {
        ChangeColor();
        _cc.radius = _skill.LevelValue[ PlayerSkillManager.Instance.SkillList[6]];
    }
    void ChangeColor()
    {
        // 색 더해주기 
        _hue += 0.7f * Time.deltaTime;
        // 더한 색 값으로 컬러 변경
        _sr.color = Color.HSVToRGB(_hue, 0.7f, 1f);
        // 색 값이 1(최댓값)이상이면 오브젝트 비활성화
        if (_hue > 0.8f) 
            gameObject.SetActive(false);
    }
}
