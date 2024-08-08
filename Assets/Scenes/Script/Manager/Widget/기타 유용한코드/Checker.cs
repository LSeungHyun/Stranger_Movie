using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Checker : MonoBehaviour
{ 
    static public Checker instance;
private void Awake()
{
    if (instance != null)
    {
        Destroy(this.gameObject);
    }
    else
    {
        DontDestroyOnLoad(this.gameObject);
        instance = this;
    }
}

    void Start()
    {
        gameObject.SetActive(false);
    }
}
