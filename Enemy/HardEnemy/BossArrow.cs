using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossArrow : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            var playerHit = GameObject.FindWithTag("Player").GetComponent<Player>();
            playerHit.Hit(10);
            Destroy(gameObject);
        }

    }
}
