using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardSprite : MonoBehaviour
{
    void LateUpdate()
    {
        var cam = Camera.main;
        transform.forward = cam.transform.forward;
    }
}
