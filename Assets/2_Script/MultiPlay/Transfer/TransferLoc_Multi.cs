using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransferLoc_Multi : ChaseManager_Multi
{
    public Transform target;
    public Collider2D targetBound;
    //public Collider2D bound;

    public PlayerManager_Multi thePlayer;
    public CamDontDes_Multi theCamera;

    public GameObject[] objDisable; // 비활성화할 오브젝트들
    public GameObject[] objEnable;
    public string bound;
    private void Awake()
    {
        theCamera = FindObjectOfType<CamDontDes_Multi>();
    }
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
    }
    // 트리거 이벤트
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Transfer();
        }
    }

    public void Transfer()
    {
        // 플레이어 위치 설정
        PlayerMove(thePlayer, target);

        // 카메라 설정
        SetCamera(theCamera, targetBound);

        if (objDisable != null)
        {
            for (int i = 0; objDisable.Length > i; i++)
            {
                objDisable[i].SetActive(false);
            }
        }

        if (objEnable != null)
        {
            for (int i = 0; objEnable.Length > i; i++)
            {
                objEnable[i].SetActive(true);
            }
        }
        //추격씬 이동에서만 작동하는 메서드
        //TransferPlayerSet();
    }
    //[PunRPC] // 다른 클라이언트에게 플레이어 위치 전달
    //public void MovePlayer(Vector3 newPosition)
    //{
    //    if (thePlayer != null)
    //    {
    //        thePlayer.transform.position = newPosition;
    //    }
    //    if (objDisable != null)
    //    {
    //        for (int i = 0; objDisable.Length > i; i++)
    //        {
    //            objDisable[i].SetActive(false);
    //        }
    //    }

    //    if (objEnable != null)
    //    {
    //        for (int i = 0; objEnable.Length > i; i++)
    //        {
    //            objEnable[i].SetActive(true);
    //        }
    //    }
    //}

    


    /// <summary>
    /// 추격씬에서 플레이어가 이동 할 때 정면 바라보는 기본 세팅
    /// 리스폰 될때 정면을 바라보게 할거면 이 메서드를
    /// ChaseManager에 옮긴 뒤에 HazzardCol에 적용시키면 됨
    /// </summary>
    public void TransferPlayerSet()
    {
        //씬 이동할때 플레이어가 정면을 바라보도록 설정하는 로직
        thePlayer.inputVec.y = -1;
        thePlayer.anim.SetFloat("DirY", thePlayer.inputVec.y);

        //애니메이션 조건문으로 바로 들어가지않도록 Vector값 0,0초기화
        thePlayer.inputVec.x = 0;
        thePlayer.inputVec.y = 0;
    }
}