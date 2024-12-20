using UnityEngine;
using UnityEngine.SceneManagement;

public class TransferMap : MonoBehaviour
{
    [Header("Transfer Settings")]
    public string transferMapName;

    private PlayerManager playerManager;
    private string currentSceneName;

    #region Lifecycle Methods
    void Awake()
    {
        playerManager = FindObjectOfType<PlayerManager>();
        currentSceneName = SceneManager.GetActiveScene().name;
    }
    #endregion

    #region OnTrigger Methods
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) // "Player" 태그를 가진 오브젝트만 반응
        {
            Transfer();
        }
    }
    #endregion

    #region Transfer Methods
    /// <summary>
    /// 씬 이동을 처리하는 메서드.
    /// </summary>
    private void Transfer()
    {
        if (playerManager == null) return; // PlayerManager가 없는 경우 예외 처리

        // 이동 상태와 현재/이전 맵 이름 설정
        playerManager.lastMapName = currentSceneName;
        playerManager.currentMapName = transferMapName;

        // 씬 이동
        SceneManager.LoadScene(transferMapName);

        // 플레이어 입력 초기화 (애니메이션 방지)
        playerManager.inputVec = Vector2.zero;
    }
    #endregion
}