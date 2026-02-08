using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HardWeapon : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 5);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0,0,1000 * Time.deltaTime);
        transform.position += new Vector3(10, 0) * Time.deltaTime;
    }
}
