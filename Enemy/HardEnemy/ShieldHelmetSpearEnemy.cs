using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldHelmetSpearEnemy : MonoBehaviour
{   // 컴포넌트 가져오기
    private Rigidbody2D _rb;
    private Animator _an;
    private SpriteRenderer _sr;
    private AudioSource _as;

    public AudioClip HitSound;
    public GameObject _SpearAxis;
    public GameObject Enemy;

    private bool _isCanMove = true;


    void Start()
    {   // 컴포넌트 할당
        _rb = GetComponent<Rigidbody2D>();
        _an = GetComponent<Animator>();
        _sr = GetComponent<SpriteRenderer>();
        _as = GetComponent<AudioSource>();
        // 공격 코루틴 시작
        StartCoroutine(Attack());
    }
    void Update()
    {
        if (_isCanMove)
        {   // 이동 가능한 상태면 오른쪽에서 왼쪽으로 이동
            transform.Translate(new Vector2(-1, 0) * PlayerStatsManager.Instance.EnemySpeed * Time.deltaTime);
        }
        if (transform.position.x < -11)
        {   // 카메라 밖으로 이동하면 삭제
            Destroy(gameObject);
        }
    }
    IEnumerator Attack()
    {   // 새로운 창축 오브젝트 생성
        GameObject mySpear = Instantiate(_SpearAxis, transform.position, Quaternion.identity);
        // 생성 후 창축 스크립트 참조
        var ss = mySpear.GetComponent<SpearAxis>();
        // 창축의 생성자 오브젝트를 해당 오브젝트로 설정
        ss.OwnerTransform = this.transform;
        // 공격 쿨타임을 4초로 설정
        yield return new WaitForSeconds(4);
        // 재귀함수
        StartCoroutine(Attack());

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Weapon"))
        {   // 맞으면 콜라이더 비활성화로 연타 방지
            GetComponent<CircleCollider2D>().enabled = false;
            _as.PlayOneShot(HitSound);
            _isCanMove = false;
            _an.SetTrigger("Hit");
            _rb.AddForce(new Vector2(1, 0) * 10, ForceMode2D.Impulse);
            StartCoroutine(CoHit());
            StartCoroutine(CoDead());
        }
    }
    IEnumerator CoHit()
    {   // 맞으면 컬러 변경으로 히트 표현
        _sr.color = new Color(1, 0.3f, 0.3f, 1);
        yield return new WaitForSeconds(0.2f);
        _sr.color = Color.white;
    }
    IEnumerator CoDead()
    {   // 죽으면 플레이어 경험치 확득 후 소울 생성
        yield return new WaitForSeconds(0.5f);
        Instantiate(Enemy, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
