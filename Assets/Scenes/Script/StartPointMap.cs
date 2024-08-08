using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartPointMap : MonoBehaviour
{
    private PlayerManager thePlayer;

    public string fromPoint;

    void Start()
    {
        thePlayer = FindObjectOfType<PlayerManager>();
        if (SceneManager.GetActiveScene().name == thePlayer.currentMapName)
        {
            if (fromPoint == thePlayer.lastMapName)
            {
                thePlayer.transform.position = this.transform.position;
            }
        }
    }
}
