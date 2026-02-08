using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Rigidbody2D _rb;
    private Animator _an;
    private SpriteRenderer _sr;
    private AudioSource _as;

    public AudioClip HitSound;
    public GameObject Soul;

    private bool _isCanMove = true;
    void Start()
    {   // 컴포넌트 할당
        _rb = GetComponent<Rigidbody2D>();
        _an = GetComponent<Animator>(); 
        _sr = GetComponent<SpriteRenderer>();
        _as = GetComponent<AudioSource>();
    }
    void Update()
    {
        if (_isCanMove)
        {   // 오른쪽에서 왼쪽으로 이동시켜주기   
            transform.Translate(new Vector2(-1, 0) * PlayerStatsManager.Instance.EnemySpeed * Time.deltaTime);
        }
        if(transform.position.x < -11)
        {   // 만약 카메라를 벗어났다면 해당 오브젝트 삭제
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Weapon"))
        {   // 히트가 중첩되지 않도록 콜라이더 비활성화
            GetComponent<CircleCollider2D>().enabled = false;
            // 히트 사운드 실행
            _as.PlayOneShot(HitSound);
            // 히트시 움직임 제어
            _isCanMove = false;
            // 히트 애니메이션 실행
            _an.SetTrigger("Hit");
            // 히트시 반대 방향으로 밀리는 힘 추가
            _rb.AddForce(new Vector2(1, 0) * 10, ForceMode2D.Impulse);
            // 히트 로직 실행 후 죽는 로직 실행
            StartCoroutine(CoHit());
            StartCoroutine(CoDead());
        }
    }
    IEnumerator CoHit()
    {   // 히트시 색 변경
        _sr.color = new Color(1, 0.3f, 0.3f, 1);
        yield return new WaitForSeconds(0.2f);
        _sr.color = Color.white;
    }
    IEnumerator CoDead()
    {   // 히트 후 0.5초 이후 실행
        yield return new WaitForSeconds(0.5f);
        // 플레이어의 경험치 올려주기
        PlayerStatsManager.Instance.CurrentExp += 20 * PlayerStatsManager.Instance.EXPUp;
        // 소울 생성 후 오브젝트 삭제
        Instantiate(Soul, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

}
