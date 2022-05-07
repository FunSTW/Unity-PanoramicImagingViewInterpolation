 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AMELookatYou : MonoBehaviour
{
    Transform camTra;

    void Start()
    {
        camTra = Camera.main.transform;
    }

    void Update()
    {
        Vector3 a = transform.position; a.y = 0;
        Vector3 b = camTra.position; b.y = 0;

        Vector3 vec = Vector3.Normalize(a - b);
        transform.forward = vec;
    }
}
