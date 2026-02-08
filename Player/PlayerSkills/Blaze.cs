using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Blaze : MonoBehaviour
{   // 적 레이어 가져오기
    public LayerMask Enemy;
    // 타겟 위치정보
    Transform Target;
    // 타겟을 공유하지 않도록 하기 위해 타겟으로 지정된 오브젝트는 리스트에 추가
    static List<GameObject> Targets = new();
    private void Start()
    {   // 10초후 오브젝트 삭제
        Destroy(gameObject, 10);
    }
    void Update()
    {   // 타겟이 없다면 타겟 세팅
        if (Target == null)
        {          
            SetTarget();
        }
        else
        {   // 타겟이 있다면 타겟을 향해 회전 및 이동
            Vector3 dir = (Target.position - transform.position).normalized;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            // 목표 회전값 만들어줌
            Quaternion targetRotation = Quaternion.Euler(0, 0, angle);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 20f * Time.deltaTime);
            transform.Translate(dir * 6 * Time.deltaTime, Space.World);
        }
    }
    void SetTarget()
    {   // 타겟이 정해지지 않으면 오른쪽으로 이동
        transform.Translate(Vector3.right * 6 * Time.deltaTime);
        // 범위 내 모든 적 탐색
        Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, 500f, Enemy);
        Targets.RemoveAll(targets => targets == null);
        // 적이 있다면 실행
        if (cols.Length > 0)
        {   // 랜덤한 적을 타겟으로 설정
            int ran = Random.Range(0, cols.Length);
            if (Targets.Contains(cols[ran].gameObject) == false)
            {   // 위치설정 및 타겟 리스트에 추가
                Target = cols[ran].transform;
                Targets.Add(cols[ran].gameObject);
                return;
            }
        }
        else
        {   // 적이 없다면 null
            Target = null;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Enemy"))
        {   // 적과 부딪히면 오브젝트 삭제
            if(Target != null)
            {   // 타겟이 없어졌다면 리스트에서도 삭제
                Targets.Remove(Target.gameObject);
            }
            Destroy(gameObject);
        }
    }

}
