using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransferLoc : MonoBehaviour
{
    public Transform target;
    public BoxCollider2D targetBound;

    private PlayerManager thePlayer;
    public CameraController theCamera;

    void Start()
    {
        thePlayer = FindObjectOfType<PlayerManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            theCamera = FindObjectOfType<CameraController>();

            // 이동 코루틴 중지
            if (thePlayer.moveCoroutine != null)
            {
                thePlayer.StopCoroutine(thePlayer.moveCoroutine);
                thePlayer.moveCoroutine = null;
            }

            // 플레이어 위치 설정
            thePlayer.transform.position = target.transform.position;
            thePlayer.canMove = false;

            // 카메라 설정
            theCamera.Teleport();
            theCamera.SetBound(targetBound);

            // 이동 가능 설정
            thePlayer.canMove = true;
        }
    }
}
