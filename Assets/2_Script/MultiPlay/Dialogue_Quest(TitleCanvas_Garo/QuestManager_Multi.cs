using Photon.Pun;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestManager_Multi : AbstractManager_Multi
{
    [Header("Photon & Managers")]
    public PhotonView PV;
    public TextManager_Multi textManager;

    [Header("PopUp Button")]
    //next / close / confirm / answer
    public GameObject btnQuestNext;
    public GameObject btnQuestClose;
    public GameObject btnQuestConfirm;
    public GameObject btnQuestAnswer;

    [Header("QuestInput")]
    public InputField questInput;
    public SpriteRenderer wrongWindow;
    public GameObject textWindow;
    private GameObject specialSprite;
    public Text textComponent;

    [Header("Quest Data")]
    public List<InteractionQuest_Multi.SpriteAnimation> listSpriteAnimations;
    public List<InteractionQuest_Multi.AudioSetting> listAudio;
    public int questIndex = 0;
    public string correctAnswer = "feline";

    //Event
    public Action onQuestEndedObject;
    public Action onQuestEndedData;

    // KeyCode 상수로 관리
    private readonly KeyCode[] confirmKeys = { KeyCode.Return, KeyCode.KeypadEnter };
    private readonly KeyCode[] nextKeys = { KeyCode.Space };
    private readonly KeyCode[] closeKeys = { KeyCode.Escape };

    void Update()
    {
        if (!isTalking) return;
        
        InputFunc();
    }

    void Start()
    {
        StartCoroutine(FindPlayerCoroutine());
    }

    #region Show/Close Quest Flow Management
    /// <summary>
    /// Quest 기본값 세팅 후 열어주는 메서드
    /// </summary>
    /// <param name="sprites"></param>
    /// <param name="animations"></param>
    /// <param name="audios"></param>
    public void ShowQuest(List<Sprite> sprites, List<InteractionQuest_Multi.SpriteAnimation> animations, List<InteractionQuest_Multi.AudioSetting> audios)
    {
        if (sprites == null || sprites.Count == 0 || isTalking) return;
        PrepareQuestState(sprites, animations, audios);
        ResetState(false, true, true);

        windowManager.OpenWindow(dialogueWindow);
        ShowSprite();
    }
    /// <summary>
    /// Quest 초기화 및 닫는 메서드
    /// </summary>
    public void CloseQuest()
    {
        if (sprite == null) return;
        ResetState(true,false,false);
        windowManager.CloseWindow();

        questInput.text = "";
    }
    #endregion

    #region Quest State
    private void PrepareQuestState(List<Sprite> sprites, List<InteractionQuest_Multi.SpriteAnimation> animations, List<InteractionQuest_Multi.AudioSetting> audios)
    {
        spriteList = sprites;
        listSpriteAnimations = animations;
        listAudio = audios;
        ActivateWindow(true);
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

    #region Next / Show / Sprite
    // 다음 스프라이트를 표시하는 함수
    public override void NextSprite()
    {
        base.NextSprite();
    }
   
    //현재 인덱스에 해당하는 스프라이트를 대화창에 출력
    public override void ShowSprite()
    {
        base.ShowSprite();
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

    #region Answer & PopUp
    /// <summary>
    /// 정답과 input 받은 값을 비교하여 정답인지 확인한 뒤 이후 이벤트를 진행하는 메서드
    /// </summary>
    public void ConfirmAnswer()
    {
        string inputText = questInput.text.ToUpper();
        if (inputText == correctAnswer || inputText == "FELINE")
        {
            //마스터 플레이어라면 정답 인정 후 진행
            if (PhotonNetwork.IsMasterClient)
            {
                if (questIndex == spriteList.Count)
                {
                    PV.RPC("AnswerQuest", RpcTarget.AllBuffered);
                }
                // 정답 입력하면 문제 페이지는 제거
                PV.RPC("NextSpriteRPC", RpcTarget.AllBuffered);
            }

            else
            {
                PV.RPC("ShowTextRPC", RpcTarget.AllBuffered, "플레이어 한 명이 정답을 알고 있습니다.");
            }
        }
        else
        {
            //오답시 입력창 초기화 및 오답 팝업 출력
            questInput.text = "";
            ShowPopupWindowImage();
        }
    }
    /// <summary>
    /// 오답 창 활성화
    /// </summary>
    public void ShowPopupWindowImage()
    {
        wrongWindow.gameObject.SetActive(true);
        Invoke("HidePopupWindow", 1f);
    }

    /// <summary>
    /// 1초 후에 오답 창 비활성화
    /// </summary>
    public void HidePopupWindow()
    {
        wrongWindow.gameObject.SetActive(false);
    }
    #endregion

    #region RPC Methods
    /// <summary>
    /// ShowText를 호출하는 RPC 메서드
    /// 센터라벨 string내용 출력 후 2초 후 사라짐
    /// </summary>
    /// <param name="textbox"></param>
    [PunRPC]
    public void ShowTextRPC(string textbox)
    {
        if (textbox != null)
        {
            textComponent.text = textbox;
            textWindow.SetActive(true);
            Invoke("CloseText", 2.0f);
        }
    }
    // 대화창을 닫는 함수
    public void CloseText()
    {
        textComponent.text = "";
        textWindow.SetActive(false);
    }
    /// <summary>
    /// 정답 입력후 대화창 닫기,Sprite넘기는 기능 RPC
    /// </summary>
    [PunRPC]
    public void NextSpriteRPC()
    {
        //AnswerQuest();
        // 정답 입력하면 문제 페이지는 제거
        if (specialSprite != null)
        {
            specialSprite.SetActive(false);
        }
        NextSprite();
    }

    [PunRPC]
    //정답을 입력해 대화창을 닫는 함수
    public void AnswerQuest()
    {
        if (sprite != null)
        {
            onQuestEndedObject?.Invoke();
            onQuestEndedData?.Invoke();
            Debug.Log("정답이라고할게");
            CloseQuest();
        }
    }
    #endregion
}