using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// 쉴드 오브젝트
public class Shield : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("EnemyWeapon"))
        {   // 적 무기하고 부딪히면 적 무기 삭제
            Destroy(collision.gameObject);
        }
    }
}
