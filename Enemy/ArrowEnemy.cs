using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowEnemy : MonoBehaviour
{
    private Rigidbody2D _rb;
    private Animator _an;
    private SpriteRenderer _sr;
    private AudioSource _as;

    public AudioClip HitSound;
    public GameObject _ArrowAxis;
    public GameObject Soul;

    private bool _isCanMove = true;


    void Start()
    {   // 컴포넌트 가져오기
        _rb = GetComponent<Rigidbody2D>();
        _an = GetComponent<Animator>();
        _sr = GetComponent<SpriteRenderer>();
        _as = GetComponent<AudioSource>();
        // 공격 코루틴
        StartCoroutine(Attack());
    }
    void Update()
    {
        if (_isCanMove)
        {   // 움직임이 가능한 상태면 오른쪽에서 왼쪽으로 이동
            transform.Translate(new Vector2(-1, 0) * PlayerStatsManager.Instance.EnemySpeed * Time.deltaTime);
        }
        if (transform.position.x < -11)
        {   // 카메라 밖으로 넘어가면 삭제
            Destroy(gameObject);
        }
    }
    // 공격 코루틴
    IEnumerator Attack()
    {   // 새 화살축 오브젝트 생성
        GameObject myArrow = Instantiate(_ArrowAxis, transform.position, Quaternion.identity);
        // 새 화살축 오브젝트 스크립트 참조
        var ss = myArrow.GetComponent<ArrowAxis>();
        // 새 화살축 오브젝트의 생성자를 해당 오브젝트로 지정
        ss.OwnerTransform = this.transform;
        // 4초마다 화살 재생성
        yield return new WaitForSeconds(4);
        // 재귀함수로 반복공격
        StartCoroutine(Attack());

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Weapon"))
        {   // 맞으면 콜라이더 비활성화
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
    {   // 맞으면 색 변경 히트 표현
        _sr.color = new Color(1, 0.3f, 0.3f, 1);
        yield return new WaitForSeconds(0.2f);
        _sr.color = Color.white;
    }
    IEnumerator CoDead()
    {   // 죽으면 경험치 증가 후 소울 생성
        yield return new WaitForSeconds(0.5f);
        PlayerStatsManager.Instance.CurrentExp += 20 * PlayerStatsManager.Instance.EXPUp;
        Instantiate(Soul, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
