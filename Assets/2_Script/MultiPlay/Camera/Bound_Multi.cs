using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bound_Multi : MonoBehaviour
{
    public Collider2D bound;

    public CamDontDes_Multi theCamera;

    void Start()
    {
        bound = GetComponent<Collider2D>();
        theCamera = FindObjectOfType<CamDontDes_Multi>();
        theCamera.SetBound(bound);
    }
}
