using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserAxis : MonoBehaviour
{
    Transform PlayerTransform;


    void Start()
    {   // 플레이어 위치 참조
        PlayerTransform = GameObject.FindWithTag("Player").transform;
        // 2초 후 삭제
        Destroy(gameObject, 2);
    }

    // Update is called once per frame
    void Update()
    {   // 플레이어 위치로 위치 보정
        if (PlayerTransform != null)
            transform.position = PlayerTransform.position;
        else
            Destroy(gameObject);
    }
}
