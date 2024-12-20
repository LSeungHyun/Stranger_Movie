using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResetPage_Multi : MonoBehaviour
{
    public Replay_Multi theReplay;
    public PlayerManager_Multi thePlayer;
    public WindowManager windowManager;
    public GameObject QuestTextManager;
    public GameObject CloseButton;
    public GameObject ConfirmButton;
    public Text text;
    public GameObject panel;

    void Awake()
    {
        CloseButton.GetComponent<Button>().onClick.AddListener(ExitDialogue);
        ConfirmButton.GetComponent<Button>().onClick.AddListener(ResetGame);
    }
    void Start()
    {
        StartCoroutine(FindPlayerCoroutine());
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            ShowDialogue();
        }
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
    }
    
    public void ShowDialogue()
    {
        thePlayer.canMove = false;
        windowManager.OpenWindow(QuestTextManager);
        text.text = "キャラクターの位置を初期化したい場合は左ボタン、" +
            "\r\nゲーム初期化をしたい場合は右ボタンを押してください。";
        CloseButton.SetActive(true);
        ConfirmButton.SetActive(true);
        panel.SetActive(true);
        text.gameObject.SetActive(true);
    }

    public void ExitDialogue()
    {
        thePlayer.canMove = true;
        text.text = "";
        QuestTextManager.SetActive(false);
        CloseButton.SetActive(false);
        ConfirmButton.SetActive(false);
        panel.SetActive(false);

        windowManager.CloseWindow();
    }

    public void ResetGame()
    {
        theReplay.ReplayGame();
        ExitDialogue();
    }
}