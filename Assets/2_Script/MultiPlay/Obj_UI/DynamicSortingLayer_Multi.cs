using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicSortingLayer_Multi : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    private string currentSortingLayer;

    // 오브젝트가 뒤에 있을 때 사용할 레이어 이름
    public string underObjectLayer = "UnderObject";
    // 오브젝트가 앞에 있을 때 사용할 레이어 이름
    public string aboveObjectLayer = "AboveObject";

    // 기준이 될 다른 오브젝트 (예: 플레이어)
    public PlayerManager_Multi thePlayer;

    public bool isColliding;
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentSortingLayer = spriteRenderer.sortingLayerName;
    }
    void Start()
    {
        StartCoroutine(FindPlayerCoroutine());
    }


    void Update()
    {
        if (thePlayer && isColliding)
        {
            string newSortingLayer = (transform.position.y > thePlayer.transform.position.y) ? underObjectLayer : aboveObjectLayer;

            // 레이어가 실제로 변경될 때만 업데이트
            if (newSortingLayer != currentSortingLayer)
            {
                currentSortingLayer = newSortingLayer;
                spriteRenderer.sortingLayerName = currentSortingLayer;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        isColliding = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        isColliding = false;
    }
    public IEnumerator FindPlayerCoroutine()
    {
        while (thePlayer == null)
        {
            // 모든 PlayerManager 객체를 찾음
            PlayerManager_Multi[] players = FindObjectsOfType<PlayerManager_Multi>();

            // 로컬 플레이어를 찾음 (PhotonView.IsMine이 true인 플레이어)
            foreach (PlayerManager_Multi player in players)
            {
                PhotonView playerPV = player.GetComponent<PhotonView>();
                if (playerPV != null && playerPV.IsMine) // 나 자신의 플레이어인지 확인
                {
                    thePlayer = player;
                    break;
                }
            }

            yield return null; // 다음 프레임까지 대기
        }
    }
}