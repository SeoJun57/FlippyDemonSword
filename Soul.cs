using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soul : MonoBehaviour
{
    Transform PlayerTransform;


    public AudioClip DestroySound;
    private AudioSource _as;

    private int _heal = 5;
    private bool _isCanMove = true;
    void Start()
    {
        _as = GetComponent<AudioSource>();
        // 플레이어에 따라 힐량 정해주기
        _heal = GameManager.Instance.PlayerSelect == 0 ? 5 : 10;
        PlayerTransform = GameObject.FindWithTag("Player").transform;
    }

    void Update()
    {
        if(PlayerTransform == null)
            return;
        float dis = Vector3.Distance(PlayerTransform.position, transform.position);
        if (_isCanMove)
        {
            transform.Translate(new Vector2(-1, 0) * PlayerStatsManager.Instance.EnemySpeed * Time.deltaTime);
        }
        else
        {
            float posX = PlayerTransform.position.x - transform.position.x;
            float posY = PlayerTransform.position.y - transform.position.y;
            transform.Translate(new Vector2(posX, posY).normalized * 10 * Time.deltaTime);
            transform.localScale -= Vector3.one * 3 * Time.deltaTime;
        }
        if (dis < PlayerStatsManager.Instance.Radius && _isCanMove)
        {
            _isCanMove = false;
            _as.PlayOneShot(DestroySound);
        }

        // 체력회복 증가
        if (transform.localScale.x <= 0)
        {
            PlayerStatsManager.Instance.CurrentHp += _heal + PlayerStatsManager.Instance.HealUp ;
            Destroy(gameObject);
        }
    }  

}
