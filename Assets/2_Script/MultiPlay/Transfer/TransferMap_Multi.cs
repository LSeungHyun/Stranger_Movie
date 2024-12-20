using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.VisualScripting;
using Unity.VisualScripting.InputSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransferMap_Multi : MonoBehaviour
{
    //public Tracker theTracker;
    private PlayerManager_Multi thePlayer;

    public string transferMapName;    
    public string currentSceneName;

    void Awake()
    {
        thePlayer = FindObjectOfType<PlayerManager_Multi>();
        currentSceneName = SceneManager.GetActiveScene().name;

        //theTracker = FindObjectOfType<Tracker>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Transfer();
        }
    }

    /// <summary>
    /// 씬 이동할때 실행해주는 메서드
    /// </summary>

    public void Transfer()
    {
        thePlayer.isTransfer = true;

        thePlayer.lastMapName = SceneManager.GetActiveScene().name;
        thePlayer.currentMapName = transferMapName;
        SceneManager.LoadScene(transferMapName);

        // 애니메이션 조건문으로 바로 들어가지 않도록 Vector 값 0,0 초기화
        thePlayer.inputVec.x = 0;
        thePlayer.inputVec.y = 0;


        //if (theTracker != null && theTracker.talkCount != 0)
        //{
        //    theTracker.talkCount = 0;
        //}
    }
}