using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossArrowAxis : MonoBehaviour
{
    private bool _isAttack = false;
    private bool _isCharge = false;

    public GameObject[] RealArrows;
    public Transform OwnerTransform;

    void Start()
    {
        Destroy(gameObject, 5);
        // 장전후 발사 로직
        StartCoroutine(CoWait());
    }

    void Update()
    {
        if (!_isAttack)
        {   // 생성자가 있다면
            if (OwnerTransform != null)
            {   // 생성자의 위치로 보정
                transform.position = OwnerTransform.position - new Vector3(0, 0.6f, 0);

            }
            else
            {   // 생성자가 죽으면 같이 없애주기
                Destroy(gameObject);
            }
            if (_isCharge)
            {   // 장전하면 오른쪽으로 살짝 이동시켜 장전 표현
                for (int i = 0; i < RealArrows.Length; i++)
                {

                    RealArrows[i].transform.localPosition += Vector3.right * Time.deltaTime;
                }
            }
        }
        else
        {
            for (int i = 0; i < RealArrows.Length; i++)
            {

                if (RealArrows[i] != null)
                {   // 발사중이면 콜라이더 활성화
                    RealArrows[i].GetComponent<CapsuleCollider2D>().enabled = true;
                    RealArrows[i].transform.Translate(Vector3.left * 20 * Time.deltaTime);
                }
            }
        }
    }

    IEnumerator CoWait()
    {   // 2초 후 장전 시작
        yield return new WaitForSeconds(2);
        _isCharge = true;
        // 장전 시작 후 0.5초 이후 발사
        yield return new WaitForSeconds(0.5f);
        _isCharge = false;
        _isAttack = true;
    }
}
