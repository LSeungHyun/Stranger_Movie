using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseManager_Multi : MonoBehaviour
{
    //public BoxCollider2D targetBound;// 플레이어를 옮길 목표 위
    /// <summary>
    /// 플레이어 위치 이동 메서드
    /// 하자드,Loc 공용
    /// </summary>
    /// <param name="transform"></param>
    public void PlayerMove(PlayerManager_Multi thePlayer, Transform transform)
    {
        thePlayer.transform.position = transform.position;
    }
    /// <summary>
    /// 카메라 관련 메서드 정리
    /// Teleport / Bound
    /// 하자드,Loc 공용
    /// </summary>
    public void SetCamera(CamDontDes_Multi theCamera, Collider2D bound)
    {
        theCamera.Teleport();
        theCamera.SetBound(bound);
    }
}