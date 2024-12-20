using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestManager : AbstractManager
{
    [Header("Quest PopUp Button")]
    private GameObject specialSprite;
    public GameObject btnQuestNext;
    public GameObject btnQuestClose;
    public GameObject btnQuestConfirm;

    public InputField questInput;
    public SpriteRenderer wrongWindow;

    [Header("Quest Data")]
    public List<InteractionQuest.SpriteAnimation> listSpriteAnimations;
    public List<InteractionQuest.AudioSetting> listAudio;

    [Header("Quest Status")]
    public int questIndex = 0;
    public string correctAnswer = "feline";

    // Events
    public Action onQuestEndedObject;
    public Action onQuestEndedData;

    // KeyCode 상수로 관리
    private readonly KeyCode[] confirmKeys = { KeyCode.Return, KeyCode.KeypadEnter };
    private readonly KeyCode[] nextKeys = { KeyCode.Space };
    private readonly KeyCode[] closeKeys = { KeyCode.Escape };
    void Update()
    {
        if (isTalking)
        {
            InputFunc();
        }
    }

    #region Show/Close Quest Flow Management
    /// <summary>
    /// Quest 기본값 세팅 후 열어주는 메서드
    /// </summary>
    /// <param name="sprites"></param>
    /// <param name="animations"></param>
    /// <param name="audios"></param>
    public void ShowQuest(List<Sprite> sprites, List<InteractionQuest.SpriteAnimation> animations, List<InteractionQuest.AudioSetting> audios)
    {
        if (sprites == null || sprites.Count == 0 || isTalking) return;
        PrepareQuestState(sprites,animations,audios);

        windowManager.OpenWindow(dialogueWindow);
        ShowSprite();
    }

    /// <summary>
    /// Quest 초기화 및 닫는 메서드
    /// </summary>
    public void CloseQuest()
    {
        if (sprite == null) return;
        ResetState();
        windowManager.CloseWindow();
    }
    #endregion

    #region Quest State
    /// <summary>
    /// Quest 킬 때 상태 초기화
    /// </summary>
    /// <param name="sprites"></param>
    /// <param name="animations"></param>
    /// <param name="audios"></param>
    private void PrepareQuestState(List<Sprite> sprites, List<InteractionQuest.SpriteAnimation> animations, List<InteractionQuest.AudioSetting> audios)
    {
        playerManager.canMove = false;
        isTalking = true;

        ActivateWindow(true);
        dialObj.gameObject.SetActive(false);

        spriteList = sprites;
        listSpriteAnimations = animations;
        listAudio = audios;

        currentIndex = 0;
        questInput.text = "";
    }

    /// <summary>
    /// Quest 끌 때 상태 초기화
    /// </summary>
    public override void ResetState()
    {
        base.ResetState();

        dialObj.gameObject.SetActive(false);
        questInput.text = "";
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
        questObj.SetActive(isActive);
    }
    #endregion

    #region Next/Show Sprite
    /// <summary>
    /// 스프라이트 전환 메서드
    /// </summary>
    public override void NextSprite()
    {
        base.NextSprite();
    }

    #endregion

    #region Answer Processing
    /// <summary>
    /// 정답과 input 받은 값을 비교하여 정답인지 확인한 뒤 이후 이벤트를 진행하는 메서드
    /// </summary>
    public void ConfirmAnswer()
    {
        string inputText = questInput.text.ToUpper();

        if (inputText == correctAnswer || inputText == "FELINE")
        {
            HandleCorrectAnswer();
        }
        else
        {
            HandleWrongAnswer();
        }
    }

    /// <summary>
    /// 정답을 입력해 Quest를 닫는 함수
    /// </summary>
    public void AnswerQuest()
    {
        if (sprite == null) return;
        onQuestEndedObject?.Invoke();
        onQuestEndedData?.Invoke();
        CloseQuest();
    }
    /// <summary>
    /// 오답 창 활성화
    /// </summary>
    public void ShowPopupWindowImage()
    {
        ActivateWrongWindow(true);
        Invoke("HidePopupWindow", 1f);
    }

    /// <summary>
    /// 1초 후에 오답 창 비활성화
    /// </summary>
    public void HidePopupWindow()
    {
        ActivateWrongWindow(false);
    }

    /// <summary>
    /// 정답 처리
    /// </summary>
    private void HandleCorrectAnswer()
    {
        if (questIndex == spriteList.Count)
        {
            AnswerQuest();
        }

        if (specialSprite != null)
        {
            specialSprite.SetActive(false);
        }
        NextSprite();
    }

    /// <summary>
    /// 오답 처리
    /// </summary>
    private void HandleWrongAnswer()
    {
        questInput.text = string.Empty;
        ShowPopupWindowImage();
    }
    /// <summary>
    /// 오답 창 활성화/비활성화
    /// </summary>
    /// <param name="isActive">활성화 여부</param>
    private void ActivateWrongWindow(bool isActive)
    {
        wrongWindow.gameObject.SetActive(isActive);
    }
    #endregion

    #region UpdateButtons
    /// <summary>
    /// Quest Button On/Off 메서드
    /// </summary>
    public override void UpdateButtons()
    {
        if (currentIndex == questIndex - 1)
        {
            SetButtonStates(
                btnQuestNext: false,
                btnQuestClose: true,
                btnQuestConfirm: true,
                questInputActive: true,
                specialSpriteActive: true
            );
        }
        else if (currentIndex == spriteList.Count - 1)
        {
            SetButtonStates(
                btnQuestNext: false,
                btnQuestClose: false,
                btnQuestConfirm: false,
                questInputActive: false,
                specialSpriteActive: false
            );
        }
        else
        {
            SetButtonStates(
                btnQuestNext: true,
                btnQuestClose: true,
                btnQuestConfirm: false,
                questInputActive: false,
                specialSpriteActive: false
            );
        }
    }
    /// <summary>
    /// 버튼 및 관련 UI 상태를 설정하는 메서드
    /// </summary>
    private void SetButtonStates(
        bool btnQuestNext,
        bool btnQuestClose,
        bool btnQuestConfirm,
        bool questInputActive,
        bool specialSpriteActive)
    {
        this.btnQuestNext?.SetActive(btnQuestNext);
        this.btnQuestClose?.SetActive(btnQuestClose);
        this.btnQuestConfirm?.SetActive(btnQuestConfirm);
        questInput.gameObject.SetActive(questInputActive);

        if (specialSprite != null)
        {
            specialSprite.SetActive(specialSpriteActive);
        }
    }
    #endregion

    #region Sprite Animation
    /// <summary>
    /// Quest에 순서에 맞게 Sprite 출력
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
        if (IsKeyPressed(confirmKeys) && btnQuestConfirm.activeInHierarchy)
        {
            ConfirmAnswer();
        }

        if (IsKeyPressed(nextKeys) && btnQuestNext.activeInHierarchy)
        {
            NextSprite();
        }

        if (IsKeyPressed(closeKeys) && btnQuestClose.activeInHierarchy)
        {
            CloseQuest();
        }
    }
    #endregion
}