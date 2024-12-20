using UnityEngine;

public class TransferManager : MonoBehaviour
{
    /// <summary>
    /// 플레이어 위치 이동
    /// 하자드와 Loc에서 공용으로 사용
    /// </summary>
    /// <param name="thePlayer">플레이어 매니저</param>
    /// <param name="targetPosition">이동할 위치</param>
    public void PlayerMove(PlayerManager thePlayer, Transform targetPosition)
    {
        if (thePlayer == null) return;
        thePlayer.transform.position = targetPosition.position;
    }

    /// <summary>
    /// 카메라 설정
    /// Teleport와 Bound 처리
    /// 하자드와 Loc에서 공용으로 사용
    /// </summary>
    /// <param name="theCamera">카메라 매니저</param>
    /// <param name="bound">카메라 바운드 설정</param>
    public void SetCamera(CamDontDes theCamera, Collider2D bound)
    {
        if (theCamera == null) return;
        theCamera.Teleport();
        theCamera.SetBound(bound);
    }
}