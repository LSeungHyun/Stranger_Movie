using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public class InteractionDialogue : AbstractInteraction
{
    [Header("Managers")]
    public DialManager dialManager;

    [Header("Dialogue Settings")]
    public List<Sprite> dialogueSprite;
    public List<SpriteAnimation> dialogueAnimation;
    public List<AudioSetting> dialogueAudio;

    [Header("Events")]
    public Action onDialogueStartedObject;
    public Action onDialogueStartedData;

    private void Awake()
    {
        FindComponents();
    }

    private void Update()
    {
        HandleInteractionEvent();
    }
    #region OnTrigger
    //public override void OnTriggerEnter2D(Collider2D collision)
    //{
    //    base.OnTriggerEnter2D(collision);
    //}

    public override void OnTriggerExit2D(Collider2D collision)
    {
        base.OnTriggerExit2D(collision);
        dialManager.isTalking = false;
    }
    #endregion

    #region FindComponents
    public override void FindComponents()
    {
        base.FindComponents();
        dialManager = FindObjectOfType<DialManager>();
    }
    #endregion

    #region Dialogue Management
    /// <summary>
    /// 대화 상호작용을 처리합니다.
    /// </summary>
    public override void HandleInteractionEvent()
    {
        if (!isColliding || dialManager.isTalking) return;

        confirmOn.SetActive(true);
        if (IsInteractionTriggered())
        {
            SendDialogue();
        }
    }

    /// <summary>
    /// 대화 시작 트리거를 활성화합니다.
    /// </summary>
    private void SendDialogue()
    {
        if (dialManager == null || dialogueSprite.Count == 0) return;
        dialManager.ShowDialogue(dialogueSprite, dialogueAnimation, dialogueAudio);
        onDialogueStartedData?.Invoke();
        onDialogueStartedObject?.Invoke();
    }
    #endregion
}