using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerSpeedManager_Multi : MonoBehaviour
{
    //생성된 플레이어를 받아줄 변수
    public PlayerManager_Multi thePlayer;

    //플레이어 스피드 변수
    public float SetSpeed = 0f;

    void Start()
    {
        StartCoroutine(FindPlayerCoroutine());
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

        //// 여기서 player 컴포넌트를 참조하여 초기화
        var playerComponent = thePlayer.GetComponent<PlayerManager_Multi>();
        if (playerComponent != null)
        {
            thePlayer.playerSpeed = SetSpeed;
        }
    }
}