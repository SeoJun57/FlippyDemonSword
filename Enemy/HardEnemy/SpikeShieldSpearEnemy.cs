using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeShieldSpearEnemy : MonoBehaviour
{
    private Rigidbody2D _rb;
    private Animator _an;
    private SpriteRenderer _sr;
    private AudioSource _as;

    public AudioClip HitSound;

    public GameObject _SpearAxis;
    public GameObject Enemy;

    private bool _isCanMove = true;
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _an = GetComponent<Animator>();
        _sr = GetComponent<SpriteRenderer>();
        _as = GetComponent<AudioSource>();
        StartCoroutine(Attack());
    }
    void Update()
    {
        if (_isCanMove)
        {
            transform.Translate(new Vector2(-1, 0) * PlayerStatsManager.Instance.EnemySpeed * Time.deltaTime);
        }
        if (transform.position.x < -11)
        {
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
        if (collision.gameObject.CompareTag("Player"))
        {
            GetComponent<CircleCollider2D>().enabled = false;
            _as.PlayOneShot(HitSound);
            _isCanMove = false;
            _an.SetTrigger("Hit");
            var playerHit = GameObject.FindWithTag("Player").GetComponent<Player>();
            playerHit.Hit(5);
            _rb.AddForce(new Vector2(1, 0) * 10, ForceMode2D.Impulse);
            StartCoroutine(CoHit());
            StartCoroutine(CoDead());
        }
        if (collision.gameObject.CompareTag("Weapon"))
        {
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
    {
        _sr.color = new Color(1, 0.3f, 0.3f, 1);
        yield return new WaitForSeconds(0.2f);
        _sr.color = Color.white;
    }
    IEnumerator CoDead()
    {
        yield return new WaitForSeconds(0.5f);
        Instantiate(Enemy, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
