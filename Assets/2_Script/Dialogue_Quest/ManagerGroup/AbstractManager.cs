using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class AbstractManager : MonoBehaviour
{
    [Header("Managers")]
    public AudioManager audioManager;
    public WindowManager windowManager;
    public PlayerManager playerManager;

    [Header("Sound Effect")]
    public EnterSound enterSound;

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

    #region Dialogue State
    /// <summary>
    /// Dialogue 끌 때 상태 초기화
    /// </summary>
    public virtual void ResetState()
    {
        playerManager.canMove = true;
        isTalking = false;

        ActivateWindow(false);
        currentIndex = 0;
        text.text = "";
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
        if (spriteList != null && currentIndex < spriteList.Count - 1)
        {
            Debug.Log("다음스프라이트 출력 :" + currentIndex);
            currentIndex++;
            ShowSprite();
        }
    }

    public void ShowSprite()
    {
        //조건이 만족되지 않으면 빠르게 return 하여 코드 중첩을 줄임.(Guard Clause)
        if (spriteList == null || currentIndex >= spriteList.Count) return;

        sprite.sprite = spriteList[currentIndex];
        UpdateButtons();
        enterSound.EnterSoundPlay();
        PlayAudioForCurrentSprite();
    }
    #endregion

    #region UpdateButtons
    public abstract void UpdateButtons();
    #endregion

    #region Sprite Animation
    public void StopCurrentAnimation()
    {
        Animator animator = sprite.GetComponent<Animator>();
        if (animator != null)
        {
            animator.runtimeAnimatorController = null;
        }
    }
    #endregion

    #region PlayAudioForCurrentSprite
    /// <summary>
    /// Dialogue 순서에 맞게 사운드 출력
    /// </summary>
    public abstract void PlayAudioForCurrentSprite();
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