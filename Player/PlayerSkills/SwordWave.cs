using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordWave : MonoBehaviour
{   // 최종 발사될 위치
    private Vector3 _dir;
    private void Start()
    {   // 5초 후 삭제
        Destroy(gameObject, 5);
        // 플레이어 스킬에서 정해진 위치 참조
        _dir = PlayerSkill.Dir;
        // 앵글 설정을 위해 역탄젠트로 위치정보 구하기
        float angle = Mathf.Atan2(_dir.y, _dir.x) * Mathf.Rad2Deg;
        // 구한 위치정보로 앵글 변경
        transform.eulerAngles = new Vector3(0, 0, angle);
    }
    void Update()
    {    // 앵글이 변경되었으니 오른쪽으로만 이동시킴
        transform.Translate(Vector3.right * 15 * Time.deltaTime);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {   // 적과 부딪히면 삭제
            Destroy(gameObject);
        }
    }
}
