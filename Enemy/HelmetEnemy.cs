using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelmetEnemy : MonoBehaviour
{
    private Rigidbody2D _rb;
    private Animator _an;
    private SpriteRenderer _sr;
    private AudioSource _as;

    public AudioClip HitSound;
    // 하위 적 오브젝트
    public GameObject Enemy;

    private bool _isCanMove = true;
    void Start()
    {   // 컴포넌트 할당
        _as = GetComponent<AudioSource>();
        _rb = GetComponent<Rigidbody2D>();
        _an = GetComponent<Animator>();
        _sr = GetComponent<SpriteRenderer>();
    }
    void Update()
    {
        if (_isCanMove)
        {   // 움직일 수 있는 상태면 오른쪽에서 왼쪽으로 이동
            transform.Translate(new Vector2(-1, 0) * PlayerStatsManager.Instance.EnemySpeed * Time.deltaTime);
        }
        if (transform.position.x < -11)
        {   // 맵 뒤로 넘어가면 삭제
            Destroy(gameObject);
        }
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
    {   // 맞으면 컬러 변경으로 히트 포현
        _sr.color = new Color(1, 0.3f, 0.3f, 1);
        yield return new WaitForSeconds(0.2f);
        _sr.color = Color.white;
    }
    IEnumerator CoDead()
    {   // 죽으면 하위레벨의 적 생성
        yield return new WaitForSeconds(0.5f);
        Instantiate(Enemy, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
