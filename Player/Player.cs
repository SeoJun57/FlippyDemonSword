using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{   // 메인 카메라
    public Camera MainCamera;

    public GameObject HitEffect;
    public GameObject DaggerAxis;
    public GameObject SkillEffect;
    public GameObject HardUltSkill;

    public AudioClip ClickSound;
    public AudioClip PlayerHit;

    private Rigidbody2D _rb;
    private SpriteRenderer _sr;
    private AudioSource _as;
    
    public static bool IsHit = true;
    private float _shakeTime = 0.2f;
    private float _shakePower = 3f;
    private void Awake()
    {   // 대거축을 미리 추가해줌
        Instantiate(DaggerAxis, new Vector3(0, 0, 0), Quaternion.identity);
    }
    void Start()
    {   // 컴포넌트 할당 및 카메라 오브젝트 가져오기
        _as = GetComponent<AudioSource>();
        _sr = GetComponent<SpriteRenderer>();
        _rb = GetComponent<Rigidbody2D>();
        MainCamera = Camera.main;
    }


    void Update()
    {
        Move();
        // 기본적으로 시계방향으로 회전
        transform.Rotate(0, 0, -100 * PlayerStatsManager.Instance.CurrentRotationSpeed * Time.deltaTime);
        transform.localScale = new Vector3(PlayerStatsManager.Instance.CurrentScale, PlayerStatsManager.Instance.CurrentScale, PlayerStatsManager.Instance.CurrentScale);
        // 클릭하면 위로 튀어오르는 로직
        if (Input.GetMouseButtonDown(0))
        {   // 클릭 사운드 
            _as.PlayOneShot(ClickSound);
            // 이펙트가 있다면 활성화 해주기
            if (PlayerSkillManager.Instance.IsEffect && SkillEffect.activeSelf == false)
            {
                SkillEffect.SetActive(true);
            }
            // 돌아가는 속도 올려주기
            PlayerStatsManager.Instance.CurrentRotationSpeed = 25;
            // 최대 높이보다 낮다면 현재 운동량을 없애고 위로 튀어오르는 힘 추가
            if (transform.position.y < 4.4f)
            {
                _rb.velocity = Vector3.zero;
                _rb.AddForce(new Vector2(0, 1) * 8, ForceMode2D.Impulse);
            }
        }
        // 궁극기 사용중이 아닐때 속도 원래대로 해주기
        if (PlayerStatsManager.Instance.CurrentRotationSpeed > 3 && BackGround.BackGroundSpeed == 0.5f)
        {
            PlayerStatsManager.Instance.CurrentRotationSpeed -= 20 * Time.deltaTime;
        }
        if(PlayerStatsManager.Instance.HardUlt && GameManager.Instance.PlayerSelect == 1)
        {
            PlayerStatsManager.Instance.HardUlt = false;
            Instantiate(HardUltSkill, transform.position + new Vector3(2, 0, 0), HardUltSkill.transform.rotation);
        }
        
    }
    void Move()
    {
        if (transform.position.y < -4.4f)
        {
            _rb.velocity = Vector3.zero;
            transform.position = new Vector2(transform.position.x, -4.4f);
        }
        if (transform.position.y > 4.4f)
        {
            _rb.velocity = Vector3.zero;
            transform.position = new Vector2(transform.position.x, 4.4f);
        }
    }

    public void Hit(int damage)
    {   // 데미지를 받을 수 있는 상황에만 데미지 로직 실행
        if (IsHit)
        {   // 연속 데미지 방지를 위해 변수 false
            IsHit = false;
            // 히트시 카메라 흔들림 로직 실행
            StartCoroutine(CoCameraMove());
            // 히트 사운드 한번 실행
            _as.PlayOneShot(PlayerHit);
            // 데미지를 방어력 수치만큼 깎은 후 체력 감소
            PlayerStatsManager.Instance.CurrentHp -= damage * PlayerStatsManager.Instance.Defend;
            // 히트 후 로직 실행
            StartCoroutine(CoHit());
        }
    }
    // 히트시 카메라 흔들리는 로직
    IEnumerator CoCameraMove()
    {   // 처음 카메라 위치 기억
        Vector3 savePos = MainCamera.transform.position;
        // 카메라가 흔들릴 시간 설정
        float time = _shakeTime;
        while (time > 0)
        {   // 메인 카메라 위치를 처음 위치에서 랜덤하게 더해주거나 빼주기
            MainCamera.transform.position = savePos + new Vector3(Random.Range(-0.2f, 0.2f), Random.Range(-0.1f, 0.1f), 0) * _shakePower;
            // 흔들림 지속시간 감소
            time -= 5 * Time.deltaTime;
            yield return null;
        }
        // 흔들림 로직이 끝나면 원래 위치로 되돌려줌
        MainCamera.transform.position = savePos;
    }
    IEnumerator CoHit()
    {   // 히트시 색 변경(빨간색)
        _sr.color = new Color(1, 0.3f, 0.3f, 1);
        yield return new WaitForSeconds(0.2f);
        // 0.2초 이후 다시 원래대로 변경 후 다시 히트가 가능하게끔 변수 true로 변경
        _sr.color = Color.white;
        IsHit = true;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {   // 태그가 적인 적과 부딪힐 경우
        if (collision.gameObject.CompareTag("Enemy"))
        {   // 부딪힌 위치보다 살짝 오른쪽에 히트 이펙트 생성
            Vector2 hitPos = collision.ClosestPoint(transform.position + new Vector3(1.5f, 0));
            Instantiate(HitEffect, hitPos, Quaternion.identity);
        }
    }
}
