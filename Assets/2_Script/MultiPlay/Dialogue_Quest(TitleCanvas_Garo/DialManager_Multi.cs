using System;
using System.Collections.Generic;
using UnityEngine;

public class DialManager_Multi : AbstractManager_Multi
{
    [Header("Sound Effect")]
    public EnterSound theES;

    [Header("PopUp Button")]
    //next / close / left / right
    public GameObject btnNext;
    public GameObject btnClose;
    public GameObject btnLeft;
    public GameObject btnRight;

    [Header("Dialogue Data")]
    public List<InteractionDialogue_Multi.SpriteAnimation> listSpriteAnimations;
    public List<InteractionDialogue_Multi.AudioSetting> listAudio;

    //Event
    public Action onDialogueEndedObject;
    public Action onDialogueEndedData;

    // KeyCode 상수로 관리
    private readonly KeyCode[] closeKeys = { KeyCode.Return, KeyCode.Escape, KeyCode.KeypadEnter };
    private readonly KeyCode[] nextPageKeys = { KeyCode.Space };
    void Update()
    {
        if (!isTalking) return;

        InputFunc();
    }

    void Start()
    {
        StartCoroutine(FindPlayerCoroutine());
    }
    #region Show/Close Dialogue Flow Management
    /// <summary>
    /// Dialogue 기본값 세팅 후 열어주는 메서드
    /// </summary>
    /// <param name="sprites"></param>
    /// <param name="animations"></param>
    /// <param name="audios"></param>
    public void ShowDialogue(List<Sprite> sprites, List<InteractionDialogue_Multi.SpriteAnimation> animations, List<InteractionDialogue_Multi.AudioSetting> audios)
    {
        if (sprites == null || sprites.Count == 0 || isTalking) return;
        PrepareQuestState(sprites, animations, audios);
        ResetState(false, true, true);

        windowManager.OpenWindow(dialogueWindow);
        ShowSprite();
    }
    /// <summary>
    /// Dialogue 초기화 및 닫는 메서드
    /// </summary>
    public void CloseDialogue()
    {
        if (sprite == null) return;
        ResetState(true, false, false);
        windowManager.CloseWindow();

        onDialogueEndedObject?.Invoke();
        onDialogueEndedData?.Invoke();
    }
    #endregion

    #region Quest State
    private void PrepareQuestState(List<Sprite> sprites, List<InteractionDialogue_Multi.SpriteAnimation> animations, List<InteractionDialogue_Multi.AudioSetting> audios)
    {
        spriteList = sprites;
        listSpriteAnimations = animations;
        listAudio = audios;
        ActivateWindow(true);

        text.text = "※ マウスを使ってください。\r\n （モバイルはタッチ。)";
    }
    #endregion

    #region ActivateQuestWindow
    /// <summary>
    /// Quest와 관련된 UI 요소(dialogueWindow, questObj) 활성화 or 비활성화
    /// </summary>
    /// <param name="isActive"></param>
    public override void ActivateWindow(bool isActive)
    {
        base.ActivateWindow(isActive);
        dialObj.SetActive(isActive);
    }
    #endregion

    #region Next / Show / Back Sprite
    // 다음 스프라이트를 표시하는 함수
    public override void NextSprite()
    {
        base.NextSprite();

        if (spriteList == null || currentIndex >= spriteList.Count - 1)
        {
            currentIndex = 0;
            ShowSprite();
        }

    }
    //현재 인덱스에 해당하는 스프라이트를 대화창에 출력
    public override void ShowSprite()
    {
        base.ShowSprite();
        theES.EnterSoundPlay();
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