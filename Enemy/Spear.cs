using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spear : MonoBehaviour
{
    public GameObject ParentObject;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            var playerHit = GameObject.FindWithTag("Player").GetComponent<Player>();
            playerHit.Hit(5);
            Destroy(ParentObject);
        }
       
    }
}
