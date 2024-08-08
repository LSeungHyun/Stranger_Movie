using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransferMap : MonoBehaviour
{
    public string transferMapName;

    private PlayerManager thePlayer;
    private FadeManager theFade;

    void Start()
    {
        thePlayer = FindObjectOfType<PlayerManager>();
        theFade = FindObjectOfType<FadeManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            StartCoroutine(TransferCoroutine());
        }
    }

    IEnumerator TransferCoroutine()
    {
        thePlayer.lastMapName = SceneManager.GetActiveScene().name;
        thePlayer.currentMapName = transferMapName;

        SceneManager.LoadScene(transferMapName);
        float startTime = Time.time;
        while (Time.time < startTime + 1f)
        {
            yield return null;
        }


    }

}