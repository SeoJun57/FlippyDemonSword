using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiringDaggerAxis : MonoBehaviour
{   // 실제 움직이는 대거
    public GameObject Dagger;
    // 대거가 생성될 위치의 반지름 길이
    public float radius = 2.5f;
    Transform PlayerTransform;
    // 생성된 대거의 오브젝트를 담아놓을 리스트
    private List<GameObject> _daggers = new List<GameObject>();
    private void Start()
    {   // 플레이어의 위치참조
        PlayerTransform = GameObject.FindWithTag("Player").transform;
    }
    void Update()
    {   // 플레이어가 없다면 오브젝트 삭제
        if (PlayerTransform == null)
        {
            Destroy(gameObject);   
        }
        else
        {   // 플레이어가 있다면 위치를 플레이어 위치로 설정
            transform.position = PlayerTransform.position;
            // 일정하게 회전
            transform.Rotate(0, 0, -100 * Time.deltaTime);
        }
    }
    // 대거 추가 로직
    public void WeaponReset(int currentLevel)
    {   // 현재 단검 개수와 새로 만들어야할 개수가 같으면 리턴
        if (_daggers.Count == currentLevel) return;
        // 실제 보이는 단검 전부 삭제
        foreach (var dagger in _daggers)
        {
            if (dagger != null) Destroy(dagger);
        }
        // 이전 생성된 단검정보 리스트 전부 삭제
        _daggers.Clear();
        // 단검 재생성 ---------(삼각함수로 위치를 다시 지정해야 하기 때문에 전부 삭제 후 재생성)
        // 시작점 설정후 라디안값으로 변경
        float offset = 90f * Mathf.Deg2Rad;
        // 단검 개수만큼 반복
        for (int i = 0; i < currentLevel; i++)
        {   // 단검을 자신의 자식 오브젝트로 생성
            GameObject newWeapon = Instantiate(Dagger, transform);

            // 각도를 생성해야할 단검의 수만큼 나눠 앵글 구하기
            float angle = (360f / currentLevel) * i;
            // 시작점에서부터 앵글의 값을 라디안 값으로 변환해 더해주기
            float radian = offset + (angle * Mathf.Deg2Rad) ;
            // 최종 각도를 구했으니 해당 각도에 해당하는 위치 구하기
            float x = Mathf.Cos(radian) * radius;
            float y = Mathf.Sin(radian) * radius;
            // 최종 위치에 생성
            newWeapon.transform.localPosition = new Vector3(x, y, 0);
            // 생성된 단검 리스트에 추가해주기
            _daggers.Add(newWeapon);
        }
    }
}
