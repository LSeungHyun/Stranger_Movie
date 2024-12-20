using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Photon.Pun;
using UnityEngine;

public class CamDontDes_Multi : MonoBehaviour
{
    public static CamDontDes_Multi instance;

    public CinemachineConfiner2D confinerBound;
    public CinemachineVirtualCamera virtualCam;
    public Collider2D boundingShape;
    public GameObject thePlayer;

    private bool isTeleporting = false;

    // Start is called before the first frame update
    void Awake()
    {
#if !(!UNITY_EDITOR && UNITY_WEBGL)
        // WebGL이 아닌 Unity일때는 조이스틱 off / unity 이동함수 사용
        virtualCam.m_Lens.OrthographicSize = 250f;

#endif
        // WebGL이면서 모바일일때
        if (Application.isMobilePlatform)
            virtualCam.m_Lens.OrthographicSize = 370f;

        //WebGL이면서 컴퓨터일때
        else
            virtualCam.m_Lens.OrthographicSize = 250f;
    }

    private void Start()
    {
        StartCoroutine(FindPlayerCoroutine());
    }
    private void Update()
    {
        if (isTeleporting)
        {
            // 순간이동
            this.transform.position = new Vector3(thePlayer.transform.position.x, thePlayer.transform.position.y, -10f);
            isTeleporting = false;
        }
    }

    private IEnumerator FindPlayerCoroutine()
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
                    thePlayer = player.gameObject;
                    break;
                }
            }

            yield return null; // 다음 프레임까지 대기
        }

        // 여기서 player 컴포넌트를 참조하여 초기화
        var playerComponent = thePlayer.GetComponent<PlayerManager_Multi>();
        if (playerComponent != null)
        {
            // 필요한 참조 설정
            virtualCam.Follow = thePlayer.transform;
            virtualCam.LookAt = thePlayer.transform;
        }
    }
    public void SetBound(Collider2D newBound)
    {
        boundingShape = newBound;
        confinerBound.m_BoundingShape2D = boundingShape;
    }

    public void Teleport()
    {
        isTeleporting = true;
    }
}