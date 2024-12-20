using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class AbstractManager_Multi : MonoBehaviour
{
    [Header("Managers")]
    public WindowManager windowManager;
    public PlayerManager_Multi playerManager;

    [Header("DialogueWindow")]
    public GameObject dialogueWindow;
    public GameObject questObj;
    public GameObject dialObj;
    public SpriteRenderer sprite;
    public Text text;

    [Header("Dialogue Data")]
    public List<Sprite> spriteList;

    [Header("Status")]
    public bool isTalking = false;
    public int currentIndex = 0;
    #region FindPlayer
    public IEnumerator FindPlayerCoroutine()
    {
        while (playerManager == null)
        {
            // 모든 PlayerManager 객체를 찾음
            PlayerManager_Multi[] players = FindObjectsOfType<PlayerManager_Multi>();

            // 로컬 플레이어를 찾음 (PhotonView.IsMine이 true인 플레이어)
            foreach (PlayerManager_Multi player in players)
            {
                PhotonView playerPV = player.GetComponent<PhotonView>();
                if (playerPV != null && playerPV.IsMine) // 나 자신의 플레이어인지 확인
                {
                    playerManager = player;
                    break;
                }
            }

            yield return null; // 다음 프레임까지 대기
        }
    }
    #endregion

    #region Dialogue State
    /// <summary>
    /// Dialogue 상태 초기화
    /// </summary>
    public void ResetState(bool canMove, bool isTalking, bool activateWindow)
    {
        playerManager.canMove = canMove;
        this.isTalking = isTalking;

        ActivateWindow(activateWindow);
        currentIndex = 0;
    }

    /// <summary>
    /// 대화창과 관련된 UI 요소(dialogueWindow, dialObj) 활성화 or 비활성화
    /// </summary>
    /// <param name="isActive"></param>
    public virtual void ActivateWindow(bool isActive)
    {
        dialogueWindow.SetActive(isActive);
    }
    #endregion

    #region Next/Show Sprite
    public virtual void NextSprite()
    {
        if (spriteList == null || currentIndex >= spriteList.Count - 1) return;

        currentIndex++;
        ShowSprite();
    }

    public virtual void ShowSprite()
    {
        if (spriteList == null || currentIndex >= spriteList.Count)
            return;

        sprite.sprite = spriteList[currentIndex];
        UpdateButtons();
    }
    #endregion

    #region UpdateButtons
    public abstract void UpdateButtons();
    #endregion

    #region Sprite Animation
    /// <summary>
    /// Dialogue에 출력되는 Sprite를 종료
    /// </summary>
    public void StopCurrentAnimation()
    {
        Animator animator = sprite.GetComponent<Animator>();
        if (animator != null)
        {
            animator.runtimeAnimatorController = null;
        }
    }

    #endregion

    #region KeyCode_Input
    public abstract void InputFunc();

    /// <summary>
    /// 전달된 KeyCode 중 하나라도 눌렸는지 확인
    /// </summary>
    /// <param name="keys">확인할 KeyCode 배열</param>
    /// <returns>키가 눌렸으면 true</returns>
    public bool IsKeyPressed(params KeyCode[] keys)
    {
        foreach (var key in keys)
        {
            if (Input.GetKeyDown(key))
            {
                return true;
            }
        }
        return false;
    }
    #endregion
}