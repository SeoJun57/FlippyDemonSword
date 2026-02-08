using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearAxis : MonoBehaviour
{
    private bool _isAttack = false;
    private bool _isCharge = false;
    // 진짜 창 오브젝트
    public GameObject RealSpear;
    // 생성자 위치
    public Transform OwnerTransform;
    // 플레이어 위치
    private Transform PlayerTransform;

    void Start()
    {   // 5초후 삭제
        Destroy(gameObject, 5);
        // 플레이어 위치 가져오기
        PlayerTransform = GameObject.FindWithTag("Player").transform;
        // 장전 로직
        StartCoroutine(CoWait());
    }

    void Update()
    {   // 공격이 불가능한 상태가 아닐 경우
        if (!_isAttack)
        {   // 생성자가 아직 죽지 않고 플레이어가 있을 경우
            if (OwnerTransform != null && PlayerTransform != null)
            {   // 창 던지는 축 위치를 생성자의 위치로 보정
                transform.position = OwnerTransform.position - new Vector3(0, 0.6f, 0);
                // 플레이어를 바라보는 임시 위치벡터 생성 후 바라보게 해줌
                Vector3 tempDir = (PlayerTransform.position - transform.position).normalized;
                float angle = Mathf.Atan2(tempDir.y, tempDir.x) * Mathf.Rad2Deg;
                transform.eulerAngles = new Vector3(0, 0, angle);
            }
            else
            {   // 생성자가 없으면 삭제
                Destroy(gameObject);
            }
            if (_isCharge)
            {   // 차지 모션 (창을 축에서 뒤로 살짝 이동시켜서 차지모션 구현)
                RealSpear.transform.localPosition += Vector3.left * Time.deltaTime;
            }
        }
        else
        {   // 진짜 창이 존재 한다면
            if (RealSpear != null)
            {   // 창의 콜라이더 활성화
                RealSpear.GetComponent<CapsuleCollider2D>().enabled = true;
            }
            // 창을 일직선으로 이동시켜줌(정해진 방향 그대로)
            transform.Translate(Vector3.right * 10 * Time.deltaTime);
        }
    }

    IEnumerator CoWait()
    {   // 2초후 장전시작
        yield return new WaitForSeconds(2);
        _isCharge = true;
        // 장전 시작 0.5초 후 발사
        yield return new WaitForSeconds(0.5f);
        _isCharge = false;
        _isAttack = true;
    }

}
