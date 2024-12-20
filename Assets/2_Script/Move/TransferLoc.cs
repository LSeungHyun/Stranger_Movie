using UnityEngine;

public class TransferLoc : TransferManager
{
    [Header("Script Objects")]
    private PlayerManager playerManager;
    private CamDontDes camDontDes;

    [Header("Set Objects")]
    public Transform target;
    public BoxCollider2D targetBound;
    public Collider2D bound;

    [Header("Ect...")]
    public GameObject[] objDisable;
    public GameObject[] objEnable;

    public bool isBgm;

    #region Lifecycle Methods
    private void Awake()
    {
        playerManager = FindObjectOfType<PlayerManager>();
        camDontDes = FindObjectOfType<CamDontDes>();
    }
    #endregion

    #region OnTrigger Methods
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Transfer();
            camDontDes.SetBound(bound);

            if (isBgm)
            {
                ToggleGameObjects(objDisable, false);
                ToggleGameObjects(objEnable, true);
            }
        }
    }
    #endregion

    #region Transfer Methods
    /// <summary>
    /// 씬 전환 시 플레이어와 카메라 설정
    /// </summary>
    private void Transfer()
    {
        PlayerMove(playerManager, target);
        SetCamera(camDontDes, targetBound);
        TransferPlayerSet();
    }

    /// <summary>
    /// 추격 씬에서 플레이어 방향과 애니메이션 초기화
    /// </summary>
    private void TransferPlayerSet()
    {
        playerManager.inputVec.y = -1;
        playerManager.anim.SetFloat("DirY", playerManager.inputVec.y);
        playerManager.inputVec = Vector2.zero;
    }

    /// <summary>
    /// 지정된 게임 오브젝트 배열의 활성화 상태 변경
    /// </summary>
    /// <param name="objects">활성화 상태를 변경할 게임 오브젝트 배열</param>
    /// <param name="state">설정할 활성화 상태</param>
    private void ToggleGameObjects(GameObject[] objects, bool state)
    {
        if (objects == null) return;

        foreach (var obj in objects)
        {
            obj.SetActive(state);
        }
    }
    #endregion
}