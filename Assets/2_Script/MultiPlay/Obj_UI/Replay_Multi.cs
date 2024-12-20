using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Replay_Multi : MonoBehaviour
{
    public PlayerManager_Multi thePlayer;

    //public GameObject button;
    //public CanvasGroup endingEndBtn;

    //void Awake()
    //{
    //    //button = GameObject.Find("Button");
    //    endingEndBtn = button.transform.GetChild(0).GetComponent<CanvasGroup>();
    //}
    void Start()
    {
        StartCoroutine(FindPlayerCoroutine());
    }

    public IEnumerator FindPlayerCoroutine()
    {
        while (thePlayer == null)
        {
            // 모든 PlayerManager 객체를 찾음
            PlayerManager_Multi[] players = FindObjectsOfType<PlayerManager_Multi>();

            // 로컬 플레이어를 찾음 (PhotonView.IsMine이 true인 플레이어)
            foreach (PlayerManager_Multi player in players)
            {
                PhotonView playerPV = player.GetComponent<PhotonView>();
                if (playerPV != null && playerPV.IsMine) // 나 자신의 플레이어인지 확인
                {
                    thePlayer = player;
                    break;
                }
            }

            yield return null; // 다음 프레임까지 대기
        }

        ////// 여기서 player 컴포넌트를 참조하여 초기화
        //var playerComponent = thePlayer.GetComponent<PlayerManager>();
        //if (playerComponent != null)
        //{
        //    thePlayer.playerSpeed = SetSpeed;
        //}
    }
    public void ReplayGame()
    {
        if (DatabaseManager.instance != null)
        {
            // 모든 아이템 status를 NotHave로 설정
            foreach (ItemInfo item in DatabaseManager.instance.itemInfos)
            {
                item.status = ItemStatus.NotHave;
            }

            // 모든 퀘스트 status를 NotSeen으로 설정
            foreach (QuestInfo quest in DatabaseManager.instance.questInfos)
            {
                quest.status = QuestStatus.NotSeen;
            }

            // 모든 인물 isActive를 false로 설정
            foreach (PersonInfo person in DatabaseManager.instance.personInfos)
            {
                person.isActive = false;
            }

            DatabaseManager.instance.minutes = 0;
            DatabaseManager.instance.seconds = 0;
        }

        //브금초기화
        //thePlayer.isSceneCount = 0;
        //thePlayer.isTransfer = true;

        //thePM.playerSpeed = 270f;
        //맵 초기화
        //thePlayer.lastMapName = "P4";
        //thePlayer.currentMapName = "P2";
        SceneManager.LoadScene("Movie_Start");
        SceneManager.sceneLoaded += OnSceneLoaded;
        //씬 이동했을때 플레이어 다시 할당할수있도록 기능추가해야함
        //endingBtn 알파값 0으로 초기화 

        //endingEndBtn.alpha = 0f;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // f0 씬에서만 작동하도록 설정
        if (scene.name == "P2")
        {
            // Player 태그를 가진 오브젝트들을 찾음
            GameObject[] objectsToHandle = GameObject.FindGameObjectsWithTag("Player");

            // 각 오브젝트를 삭제하거나 비활성화
            foreach (GameObject obj in objectsToHandle)
            {
                //현재 플레이어와 다른 player오브젝트를 찾아서 삭제
                if (obj != thePlayer)
                    obj.SetActive(false);
            }
        }
    }
}