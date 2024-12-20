using Cinemachine;
using UnityEngine;

public class CamDontDes : MonoBehaviour
{
    public CinemachineConfiner2D confinerBound;
    public CinemachineVirtualCamera virtualCam;
    public Collider2D boundingShape;
    public GameObject Player;

    private bool isTeleporting = false;

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

    private void Update()
    {
        if (isTeleporting)
        {
            // 순간이동
            this.transform.position = new Vector3(Player.transform.position.x, Player.transform.position.y, -10f);
            isTeleporting = false;
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