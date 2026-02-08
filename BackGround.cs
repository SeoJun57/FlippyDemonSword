using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGround : MonoBehaviour
{
    private SpriteRenderer _sr;
    private float _offset;
    public static float BackGroundSpeed = 0.5f;
    void Start()
    {
        _sr = GetComponent<SpriteRenderer>();   
    }


    void Update()
    {
        _offset += BackGroundSpeed * Time.deltaTime;
        _sr.material.mainTextureOffset = new Vector2(_offset, 0);
    }
}
