using Photon.Pun;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Photon.Pun.UtilityScripts.PunTeams;


public class InteractionQuest_Multi : MonoBehaviour
{
    //스크립트 캐싱
    public QuestManager_Multi theQM;
    public TextManager_Multi theTM;

    //오브젝트 캐싱
    public PhotonView PV;

    // Sprite 배열 미리 받아두는곳
    public List<Sprite> dialogueSprite;
    public List<SpriteAnimation> dialogueAnimation;
    public List<AudioSetting> dialogueAudio;


    public int questIndex = 0;
    public string correctAnswer = "feline";
    public string temptext = "※ マウスを使ってください。\r\n （モバイルはタッチ。)";
    public Sprite wrongSprite;

    //게임 중 변하는 불값
    private bool isColliding = false;

    //이벤트
    public Action onQuestStartedData;
    public Action onQuestStartedObject;

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
        theQM = FindObjectOfType<QuestManager_Multi>();
        theTM = FindObjectOfType<TextManager_Multi>();
    }

    private void Update()
    {
        if (isColliding && !theQM.isTalking)
        {
            //동기화가 필요한 메인 퀘스트는 이벤트스크립트에서 처리
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (PhotonNetwork.IsMasterClient)
                {
                    SendDialogue();
                    Debug.Log("내가 마스터임ㅁㅁㅁㅁㅁㅁㅁ");

                    if (PhotonNetwork.CurrentRoom.PlayerCount >= 2)
                    {
                        PV.RPC("SendDialogue", RpcTarget.AllBuffered);
                    }
                }
                else
                {
                    // 마스터한테 상호작용 요청 (RPC로 처리 가능)
                    Debug.Log("마스터야 진행시켜 ㄱㄱ");
                    PV.RPC("ShowTextRPC", RpcTarget.AllBuffered, "타인 플레이어 한 명이 퀘스트 진행을 요청했습니다.");
                }
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        isColliding = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        isColliding = false;
        theQM.isTalking = false;
    }
    [PunRPC]
    void SendDialogue()
    {
        if (theQM != null && dialogueSprite.Count > 0)
        {
            theQM.questIndex = questIndex;
            theQM.correctAnswer = correctAnswer;
            theQM.wrongWindow.sprite = wrongSprite;
            theQM.text.text = temptext;
            onQuestStartedData?.Invoke();
            onQuestStartedObject?.Invoke();

            theQM.ShowQuest(dialogueSprite, dialogueAnimation, dialogueAudio);
        }
    }

    // ShowText를 호출하는 RPC 메서드
    [PunRPC]
    void ShowTextRPC(string message)
    {
        theTM.ShowText(message); // theTM 인스턴스에서 ShowText 호출
        Invoke("CloseTMText", 2.0f);
    }
    public void CloseTMText()
    {
        theTM.CloseText();
    }
}