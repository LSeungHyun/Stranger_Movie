using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.VisualScripting.Member;

public class TestScript2 : MonoBehaviour
{
    BGMManager BGM;

    void Start()
    {
        BGM = FindObjectOfType<BGMManager>();
    }


   private void OnTriggerEnter2D(Collider2D collision)
    {
        StartCoroutine(abc());
        this.gameObject.SetActive(false);
    }
    IEnumerator abc()
    {
       BGM.FadeOutMusic();

        yield return null;
    }
}
