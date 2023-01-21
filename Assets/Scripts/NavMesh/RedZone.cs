using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedZone : MonoBehaviour
{
    public GameObject BorderLeft, BorderRight;
    public float speed = 10;
    void FixedUpdate()
    {
        if ((transform.localPosition.x - transform.localScale.x / 2) <= (BorderLeft.transform.localPosition.x - BorderLeft.transform.localScale.z / 2))
            speed *= -1;
        else if ((transform.localPosition.x + transform.localScale.x / 2) >= (BorderRight.transform.localPosition.x + BorderRight.transform.localScale.z / 2))
            speed *= -1;

        transform.position += new Vector3(speed / 100, 0, 0);
    }
}
