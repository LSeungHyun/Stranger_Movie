using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialManager : AbstractManager
{
    [Header("PopUp Button")]
    public GameObject btnNext;
    public GameObject btnClose;
    public GameObject btnLeft;
    public GameObject btnRight;

    [Header("Dialogue Data")]
    public List<InteractionDialogue.SpriteAnimation> listSpriteAnimations;
    public List<InteractionDialogue.AudioSetting> listAudio;
    // Events
    public Action onDialogueEndedObject;
    public Action onDialogueEndedData;

    // KeyCode 상수로 관리
    private readonly KeyCode[] closeKeys = { KeyCode.Return, KeyCode.Escape, KeyCode.KeypadEnter };
    private readonly KeyCode[] nextPageKeys = { KeyCode.Space };

    void Update()
    {
        if (isTalking)
        {
            //대화중에 키입력을 통해 팝업창 조작
            InputFunc();
        }
    }

    #region Show/Close Dialogue Flow Management
    /// <summary>
    /// Dialogue 기본값 세팅 후 열어주는 메서드
    /// </summary>
    /// <param name="sprites"></param>
    /// <param name="animations"></param>
    /// <param name="audios"></param>
    public void ShowDialogue(List<Sprite> sprites, List<InteractionDialogue.SpriteAnimation> animations, List<InteractionDialogue.AudioSetting> audios)
    {
        if(sprites == null || sprites.Count == 0 || isTalking) return;
        PrepareDialogueState(sprites, animations, audios);

        windowManager.OpenWindow(dialogueWindow);
        ShowSprite();
    }
    /// <summary>
    /// Dialogue 초기화 및 닫는 메서드
    /// </summary>
    public void CloseDialogue()
    {
        if (!isTalking) return;

        ResetState();
        onDialogueEndedObject?.Invoke();
        onDialogueEndedData?.Invoke();
    }
    #endregion

    #region Dialogue State
    /// <summary>
    /// Dialogue 킬 때 상태 초기화
    /// </summary>
    /// <param name="sprites"></param>
    /// <param name="animations"></param>
    /// <param name="audios"></param>
    private void PrepareDialogueState(List<Sprite> sprites, List<InteractionDialogue.SpriteAnimation> animations,List<InteractionDialogue.AudioSetting> audios)
    {
        playerManager.canMove = false;
        isTalking = true;

        ActivateWindow(true);
        questObj.SetActive(false);

        spriteList = sprites;
        listSpriteAnimations = animations;
        listAudio = audios;

        currentIndex = 0;
        text.text = "※ マウスを使ってください。\r\n （モバイルはタッチ。)";
    }
    /// <summary>
    /// Dialogue 끌 때 상태 초기화
    /// </summary>
    public override void ResetState()
    {
        base.ResetState();
        questObj.SetActive(false);

        spriteList = null;
        listSpriteAnimations = null;
        listAudio = null;

        windowManager.CloseWindow();
    }
    #endregion

    #region ActivateDialogueWindow
    /// <summary>
    /// 대화창과 관련된 UI 요소(dialogueWindow, dialObj) 활성화 or 비활성화
    /// </summary>
    /// <param name="isActive"></param>
    public override void ActivateWindow(bool isActive)
    {
        base.ActivateWindow(isActive);
        dialObj.SetActive(isActive);
    }
    #endregion

    #region Next/Back/Show Sprite
    /// <summary>
    /// 스프라이트 전환 메서드
    /// </summary>
    public override void NextSprite()
    {
        base.NextSprite();

        if (spriteList == null || currentIndex >= spriteList.Count -1)
        {
            ShowSprite();
        }
    }

    public void BackSprite()
    {
        if (spriteList != null && currentIndex > 0)
        {
            currentIndex--;
            ShowSprite();
        }
        else
        {
            currentIndex = spriteList.Count;
            ShowSprite();
        }
    }
    #endregion

    #region UpdateButtons
    /// <summary>
    /// 팝업창 버튼 On/Off 메서드
    /// </summary>
    public override void UpdateButtons()
    {
        bool isLastSprite = (currentIndex == spriteList.Count - 1);
        SetButtonStates(btnLeft: false, btnRight: false, btnClose: isLastSprite, btnNext: !isLastSprite);
    }
    private void SetButtonStates(bool btnLeft, bool btnRight, bool btnClose, bool btnNext)
    {
        this.btnLeft.SetActive(btnLeft);
        this.btnRight.SetActive(btnRight);
        this.btnClose.SetActive(btnClose);
        if (this.btnNext != null)
        {
            this.btnNext.SetActive(btnNext);
        }
    }
    #endregion

    #region Sprite Animation
    /// <summary>
    /// Dialogue 순서에 맞게 Sprite 출력
    /// </summary>
    public void ControlAnimation()
    {
        RuntimeAnimatorController animatorController = null;
        foreach (var spriteAnimation in listSpriteAnimations)
        {
            if (spriteAnimation.sprite == spriteList[currentIndex])
            {
                animatorController = spriteAnimation.animatorController;
                break;
            }
        }

        if (animatorController != null)
        {
            Animator animator = sprite.GetComponent<Animator>();
            if (animator != null)
            {
                animator.runtimeAnimatorController = animatorController;
                animator.Play("AnimationState");
            }
        }
    }
    #endregion

    #region PlayAudioForCurrentSprite
    /// <summary>
    /// Dialogue 순서에 맞게 사운드 출력
    /// </summary>
    public override void PlayAudioForCurrentSprite()
    {
        foreach (var Pages in listAudio)
        {
            if (Pages.PageNum == currentIndex)
                audioManager.EffectSoundPlay(Pages.AudioNum);
        }
    }
    #endregion

    #region KeyCode_Input
    public override void InputFunc()
    {
        //리팩토링 후
        if (IsKeyPressed(closeKeys) && btnClose.activeInHierarchy)
        {
            CloseDialogue();
        }
        else if (IsKeyPressed(nextPageKeys) && btnNext.activeInHierarchy)
        {
            NextSprite();
        }
    }
    #endregion
}