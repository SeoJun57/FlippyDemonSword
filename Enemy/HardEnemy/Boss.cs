using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{   // 컴포넌트 가져오기
    private Rigidbody2D _rb;
    private Animator _an;
    private SpriteRenderer _sr;
    private AudioSource _as;

    public AudioClip HitSound;
    public GameObject _ArrowAxis;


    // UI관련 ----------
    // 보스 체력 이미지
    public Image CurrentHpImage;
    public TextMeshProUGUI WarningText;

    private bool _isAlpha = true;
    private bool _isCanMove = true;
    private float _maxHp = 300;
    private float _currentHp = 300;


    void Start()
    {   // 컴포넌트 가져오기
        _rb = GetComponent<Rigidbody2D>();
        _an = GetComponent<Animator>();
        _sr = GetComponent<SpriteRenderer>();
        _as = GetComponent<AudioSource>();
        // 등장 코루틴
        StartCoroutine(CoWarning());

    }
    void Update()
    {
        CurrentHpImage.fillAmount = _currentHp / _maxHp;
        if (_isCanMove)
        {   // 움직임이 가능한 상태면 오른쪽에서 왼쪽으로 이동
            transform.Translate(new Vector2(-1, 0) * 2 * Time.deltaTime);
        }
        if (transform.position.x < 7)
        {
            _isCanMove = false;
        }
    }
    IEnumerator CoWarning()
    {
        float time = 0;
        while (time < 3)
        {
            time += Time.deltaTime;
            if (_isAlpha)
            {
                WarningText.color -= new Color(0, 0, 0, 2f * Time.deltaTime);
                if(WarningText.color.a < 0.2f)
                    _isAlpha = false;  
            }
            else
            {
                WarningText.color += new Color(0, 0, 0, 2f * Time.deltaTime);
                if (WarningText.color.a >= 1f)
                    _isAlpha = true;
            }
            yield return null;
        }
        WarningText.gameObject.SetActive(false);
        StartCoroutine(CoAttack());
    }
    // 공격 코루틴
    IEnumerator CoAttack()
    {   // 새 화살축 오브젝트 생성
        GameObject myArrow = Instantiate(_ArrowAxis, transform.position, Quaternion.identity);
        // 새 화살축 오브젝트 스크립트 참조
        var ss = myArrow.GetComponent<BossArrowAxis>();
        // 새 화살축 오브젝트의 생성자를 해당 오브젝트로 지정
        ss.OwnerTransform = this.transform;
        // 4초마다 화살 재생성
        yield return new WaitForSeconds(4);
        // 재귀함수로 반복공격
        StartCoroutine(CoAttack());

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Weapon"))
        {   // 맞으면 콜라이더 비활성화
            GetComponent<CircleCollider2D>().enabled = false;
            // 맞으면 체력 감소
            _currentHp -= 5;
            _as.PlayOneShot(HitSound);
            StartCoroutine(CoHit());
            if (_currentHp <= 0)
            {
                _rb.AddForce(new Vector2(1, 0) * 3, ForceMode2D.Impulse);
                _an.SetTrigger("Die");
                StartCoroutine(CoDead());
            }
        }
    }
    IEnumerator CoHit()
    {   // 맞으면 색 변경 히트 표현
        _sr.color = new Color(1, 0.3f, 0.3f, 1);
        yield return new WaitForSeconds(0.2f);
        GetComponent<CircleCollider2D>().enabled = true;
        _sr.color = Color.white;
    }
    IEnumerator CoDead()
    {  
        yield return new WaitForSeconds(0.5f);
        if (GameManager.Instance.ModeSelect == 0)
        {
            GameUI.IsClear = true;
            GameManager.Instance.IsClear = 1;
        }
        else
        {
            GameUI.IsSpawnedBoss = false;
        }
        Destroy(gameObject);
    }
}
