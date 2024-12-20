using UnityEngine;

public class PlayerSpeedManager : MonoBehaviour
{
    public PlayerManager playerManager;
    public float SetSpeed = 0f;
    public WindowManager windowManager;
    private void Awake()
    {
        playerManager = FindObjectOfType<PlayerManager>();
        playerManager.playerSpeed = SetSpeed;
        //팝업초기화
        windowManager = FindObjectOfType<WindowManager>();
        windowManager.CloseWindow();
    }
}