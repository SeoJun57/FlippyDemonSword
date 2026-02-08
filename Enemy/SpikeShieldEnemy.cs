using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeShieldEnemy : MonoBehaviour
{
    private Rigidbody2D _rb;
    private Animator _an;
    private SpriteRenderer _sr;
    private AudioSource _as;

    public AudioClip HitSound;
    public GameObject Enemy;

    private bool _isCanMove = true;
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _an = GetComponent<Animator>();
        _sr = GetComponent<SpriteRenderer>();
        _as = GetComponent<AudioSource>();
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
        if(collision.gameObject.CompareTag("Weapon"))
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
