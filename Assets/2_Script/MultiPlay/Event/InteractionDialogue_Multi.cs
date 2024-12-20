using Photon.Pun;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class InteractionDialogue_Multi : MonoBehaviour
{
    //스크립트 캐싱
    public DialManager_Multi theDM;

    //오브젝트 캐싱
    public WebGLBtn webglBtn;
    // Sprite 배열 미리 받아두는곳
    public List<Sprite> dialogueSprite;
    public List<SpriteAnimation> dialogueAnimation;
    public List<AudioSetting> dialogueAudio;

    //인스펙터로 지정해주는 불값
    public bool isTouch = false;
    public bool isSpinner = false;
    //플레이어쪽에서 isMainQuest가 아닐때를 조건으로 사용중
    //전부다 isMainQuest가 아닌거같은데 확인 후 둘다 삭제예정
    //public bool isMainQuest = false;

    //이벤트
    public Action onDialogueStartedObject;
    public Action onDialogueStartedData;

    [System.Serializable]
    public struct SpriteAnimation
    {
        public Sprite sprite;
        public RuntimeAnimatorController animatorController;
    }

    [System.Serializable]
    public struct AudioSetting
    {
        public int PageNum;
        public int AudioNum;
        public bool isLoop;
    }


    private void Awake()
    {
        theDM = FindObjectOfType<DialManager_Multi>();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        theDM.isTalking = false;
    }
    /// <summary>
    /// 상호작용 시 Dialogue창 띄워주는 메서드
    /// </summary>
    [PunRPC]
    public void SendDialogue()
    {
        if (theDM != null && dialogueSprite.Count > 0)
        {
            theDM.ShowDialogue(dialogueSprite, dialogueAnimation, dialogueAudio);
            onDialogueStartedData?.Invoke();
            onDialogueStartedObject?.Invoke();
        }
    }
}