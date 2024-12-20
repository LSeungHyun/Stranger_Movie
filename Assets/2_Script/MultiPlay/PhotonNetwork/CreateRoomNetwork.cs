using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using Unity.VisualScripting;
using static Photon.Pun.UtilityScripts.PunTeams;

public class CreateRoomNetwork : MonoBehaviourPunCallbacks
{
    public static CreateRoomNetwork instance;
    [Header("DisconnectPanel")]
    public InputField NickNameInput;

    [Header("LobbyPanel")]
    public GameObject LobbyPanel;
    public InputField RoomInput;
    public Text WelcomeText;
    public Text LobbyInfoText;
    public Button[] CellBtn;
    public Button PreviousBtn;
    public Button NextBtn;

    [Header("RoomPanel")]
    public GameObject RoomPanel;
    public Text ListText;
    public Text RoomInfoText;
    public Text[] ChatText;
    public InputField ChatInput;

    [Header("ETC")]
    public Text StatusText;
    public PhotonView PV;

    [Header("ActiveObj")]
    public GameObject Movie_Title;
    public GameObject Movie_Start;
    public GameObject LedaScreen;
    public GameObject LedaLoading;

    [Header("Script")]
    public TextManager_Multi theTM;

    List<RoomInfo> myList = new List<RoomInfo>();
    int currentPage = 1, maxPage, multiple;

    // 무한 루프 방지를 위한 최대 시도 횟수 설정
    private const int MaxAttempts = 100;
    #region 방리스트 갱신
    // ◀버튼 -2 , ▶버튼 -1 , 셀 숫자
    public void MyListClick(int num)
    {
        if (num == -2) --currentPage;
        else if (num == -1) ++currentPage;
        else
        {
            PhotonNetwork.JoinRoom(myList[multiple + num].Name);
        }
        MyListRenewal();
    }

    /// <summary>
    /// 방 참가시  패널 on/off해줘야하는 상황에 쓰일 메서드
    /// </summary>
    public void ActiveObj()
    {
        LedaLoading.SetActive(true);
        Movie_Start.SetActive(true);
        Movie_Title.SetActive(false);
        LedaScreen.SetActive(false);
    }
    void MyListRenewal()
    {
        // 최대페이지
        maxPage = (myList.Count % CellBtn.Length == 0) ? myList.Count / CellBtn.Length : myList.Count / CellBtn.Length + 1;

        // 이전, 다음버튼
        PreviousBtn.interactable = (currentPage <= 1) ? false : true;
        NextBtn.interactable = (currentPage >= maxPage) ? false : true;

        // 페이지에 맞는 리스트 대입
        multiple = (currentPage - 1) * CellBtn.Length;
        for (int i = 0; i < CellBtn.Length; i++)
        {
            bool isActive = (multiple + i < myList.Count);
            CellBtn[i].interactable = isActive;

            //버튼이 켜졌을때
            if (isActive)
            {
                CellBtn[i].transform.GetChild(0).GetComponent<Text>().text = myList[multiple + i].Name;
                CellBtn[i].transform.GetChild(1).GetComponent<Text>().text = myList[multiple + i].PlayerCount + "/" + myList[multiple + i].MaxPlayers;

                // 방 참여자 수와 최대 인원이 같다면 색상을 초록색으로 설정
                if (myList[multiple + i].PlayerCount == myList[multiple + i].MaxPlayers)
                {
                    CellBtn[i].transform.GetChild(0).GetComponent<Text>().color = Color.green;
                    CellBtn[i].transform.GetChild(1).GetComponent<Text>().color = Color.green;
                }
                else
                {
                    // 기본 색상으로 초기화
                    CellBtn[i].transform.GetChild(0).GetComponent<Text>().color = Color.white;
                    CellBtn[i].transform.GetChild(1).GetComponent<Text>().color = Color.white;
                }
            }
            //버튼이 꺼졌을 때
            else
            {
                // 빈 칸 처리
                CellBtn[i].transform.GetChild(0).GetComponent<Text>().text = "";
                CellBtn[i].transform.GetChild(1).GetComponent<Text>().text = "";
                CellBtn[i].transform.GetChild(0).GetComponent<Text>().color = Color.white;
                CellBtn[i].transform.GetChild(1).GetComponent<Text>().color = Color.white;
            }
        }
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        int roomCount = roomList.Count;
        for (int i = 0; i < roomCount; i++)
        {
            if (!roomList[i].RemovedFromList)
            {
                if (!myList.Contains(roomList[i])) myList.Add(roomList[i]);
                else myList[myList.IndexOf(roomList[i])] = roomList[i];
            }
            else if (myList.IndexOf(roomList[i]) != -1) myList.RemoveAt(myList.IndexOf(roomList[i]));
        }
        MyListRenewal();
    }
    #endregion


    #region 서버연결
    void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            DontDestroyOnLoad(this.gameObject);
            instance = this;
        }

        //// PhotonView ID 충돌 방지를 위해 PhotonView를 동적으로 할당
        //if (PV == null)
        //{
        //    PV = gameObject.AddComponent<PhotonView>();
        //    PV.ViewID = PhotonNetwork.AllocateViewID(true); // 동적 ID 할당
        //}

        Screen.SetResolution(960, 540, false);
        PhotonNetwork.SendRate = 60; // 서버로 보내는 빈도수 (프레임수)
        PhotonNetwork.SerializationRate = 30; // 객체 업데이트 회수
        //이거 없어도 바로 게임 참가했을때 플레이어 동기화가 잘 되네??
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    void Update()
    {
        StatusText.text = PhotonNetwork.NetworkClientState.ToString();
        LobbyInfoText.text = (PhotonNetwork.CountOfPlayers - PhotonNetwork.CountOfPlayersInRooms) + "로비 / " + PhotonNetwork.CountOfPlayers + "접속";
    }

    public void Connect()
    {
        PhotonNetwork.ConnectUsingSettings();
        //방에 접속하면 랜덤 닉네임 제공
        SetUniqueNickname();
    }

    public override void OnConnectedToMaster() => PhotonNetwork.JoinLobby();

    public override void OnJoinedLobby()
    {
        LobbyPanel.SetActive(true);
        RoomPanel.SetActive(false);
        //PhotonNetwork.LocalPlayer.NickName = NickNameInput.text;
        //WelcomeText.text = PhotonNetwork.LocalPlayer.NickName + "님 환영합니다";
        myList.Clear();
    }

    public void Disconnect() => PhotonNetwork.Disconnect();

    public override void OnDisconnected(DisconnectCause cause)
    {
        LobbyPanel.SetActive(false);
        RoomPanel.SetActive(false);
    }
    #endregion


    #region 방
    public void CreateRoom() { 
        PhotonNetwork.CreateRoom(RoomInput.text == "" ? "Room" + Random.Range(0, 100) : RoomInput.text, new RoomOptions { MaxPlayers = 4 });
    }

    public void JoinRandomRoom() => PhotonNetwork.JoinRandomRoom();

    public void LeaveRoom() => PhotonNetwork.LeaveRoom();

    public override void OnJoinedRoom()
    {
        //RoomPanel.SetActive(true);
        //LobbyPanel.SetActive(false);
        RoomRenewal();
        ActiveObj();

        //시작지점에(0,-800,0)에 플레이어 생성
        PhotonNetwork.Instantiate("Multi_Hero", new Vector3(0, -800, 0), Quaternion.identity);
        ShowCenterLabel(PhotonNetwork.CurrentRoom.Name + "에 입장하셨습니다");
        
        //채팅창 초기화
        ChatInput.text = "";
        for (int i = 0; i < ChatText.Length; i++) ChatText[i].text = "";
    }
    /// <summary>
    /// 플레이어 닉네임 중복 없이 랜덤으로 1~100 부여
    /// </summary>
#region
    private void SetUniqueNickname()
    {
        string newNickname;
        bool isUnique;
        int attempts = 0;

        do
        {
            int randomValue = Random.Range(0, 100);
            newNickname = "Player " + randomValue;

            // 중복 여부 확인
            isUnique = IsNicknameUnique(newNickname);

            attempts++;
            if (attempts >= MaxAttempts)
            {
                Debug.LogWarning("중복 없는 닉네임 생성 시도 횟수를 초과했습니다.");
                break;
            }
        } while (!isUnique);

        PhotonNetwork.LocalPlayer.NickName = newNickname;
    }

    private bool IsNicknameUnique(string nickname)
    {
        foreach (var player in PhotonNetwork.PlayerList)
        {
            if (player.NickName == nickname)
            {
                return false; // 중복된 닉네임 발견
            }
        }
        return true; // 중복 없음
    }
    /// <summary>
    /// 방 생성 실패 메서드
    /// </summary>
    /// <param name="returnCode"></param>
    /// <param name="message"></param>
    #endregion
    public override void OnCreateRoomFailed(short returnCode, string message) 
    {
        //방이 중복되었다고 출력
        RoomInput.text = "";
        Debug.Log("방이 이미있음");
        ShowCenterLabel("같은 이름의 방이 있음");
    }
    /// <summary>
    /// 방 참가 실패 메서드
    /// 이곳에서 센터라벨이나 팝업 출력가능할듯
    /// </summary>
    /// <param name="returnCode"></param>
    /// <param name="message"></param>
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        //센터라벨 출력기능
        
        Debug.Log("방못들어가");
        ShowCenterLabel("방입장 실패");
    }
    
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        //값을 직접 할당해주지 못해서 오류 발생
        //RoomInput.text == "" ? "Room" + Random.Range(0, 100) : RoomInput.text; 

        string roomName = RoomInput.text == "" ? "Room" + Random.Range(0, 100) : RoomInput.text;
        CreateRoom(roomName); 
    }
    /// <summary>
    /// 방 랜덤참가 실패시 만들어주는 메서드
    /// </summary>
    /// <param name="roomName"></param>
    private void CreateRoom(string roomName)
    {
        PhotonNetwork.CreateRoom(roomName, new RoomOptions { MaxPlayers = 4 });
        Debug.Log("방 생성 시도 중: " + roomName);
    }
    /// <summary>
    /// 다른 플레이어가 방에 들어왔을때 실행되는 메서드
    /// </summary>
    /// <param name="newPlayer"></param>
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        RoomRenewal();
        ShowCenterLabel(newPlayer.NickName + "님이 참가하셨습니다");
        //ChatRPC("<color=yellow>" + newPlayer.NickName + "님이 참가하셨습니다</color>");
    }

    /// <summary>
    /// 다른 플레이어가 방에 나갔을때 메서드
    /// </summary>
    /// <param name="otherPlayer"></param>
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        RoomRenewal();
        //LobbyPanel.SetActive(true);
        ShowCenterLabel(otherPlayer.NickName + "님이 퇴장하셨습니다");
        ChatRPC("<color=yellow>" + otherPlayer.NickName + "님이 퇴장하셨습니다</color>");
    }

    void RoomRenewal()
    {
        ListText.text = "";
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
            ListText.text += PhotonNetwork.PlayerList[i].NickName + ((i + 1 == PhotonNetwork.PlayerList.Length) ? "" : "\n");
        RoomInfoText.text = PhotonNetwork.CurrentRoom.Name + " / " + PhotonNetwork.CurrentRoom.PlayerCount + "명 / " + PhotonNetwork.CurrentRoom.MaxPlayers + "최대";
    }
    #endregion


    #region 채팅
    public void Send()
    {
        PV.RPC("ChatRPC", RpcTarget.All, PhotonNetwork.NickName + " : " + ChatInput.text);
        ChatInput.text = "";
    }

    [PunRPC] // RPC는 플레이어가 속해있는 방 모든 인원에게 전달한다
    void ChatRPC(string msg)
    {
        bool isInput = false;
        for (int i = 0; i < ChatText.Length; i++)
            if (ChatText[i].text == "")
            {
                isInput = true;
                ChatText[i].text = msg;
                break;
            }
        if (!isInput) // 꽉차면 한칸씩 위로 올림
        {
            for (int i = 1; i < ChatText.Length; i++) ChatText[i - 1].text = ChatText[i].text;
            ChatText[ChatText.Length - 1].text = msg;
        }
    }
    #endregion

    /// <summary>
    /// 본인 / 타인에게 센터라벨 출력하는 메서드
    /// </summary>
    /// <param name="message"></param>
    [PunRPC]
    public void ShowCenterLabel(string message)
    {
        theTM.ShowText(message);
        Invoke("CloseTMText", 2.0f);
    }
    public void CloseTMText()
    {
        theTM.CloseText();
    }
}