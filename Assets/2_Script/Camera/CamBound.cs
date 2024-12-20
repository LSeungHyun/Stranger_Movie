using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamBound : MonoBehaviour
{
    public Collider2D bound;

    public CamDontDes theCamera;

    void Start()
    {
        bound = GetComponent<Collider2D>();
        theCamera = FindObjectOfType<CamDontDes>();
        theCamera.SetBound(bound);
    }
}
