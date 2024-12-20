using Photon.Pun;
using System.Collections; //기존 namespace
using System.Collections.Generic;//기존 namespace
using System.Linq;
using UnityEngine; //기존 namespace
using UnityEngine.SceneManagement; //기존 namespace
using static Photon.Pun.UtilityScripts.PunTeams;
using ExitGames.Client.Photon;

public class PlayerManager_Multi : AbstractPlayer_Multi
{
    /// <summary>
    /// 생명주기함수
    /// </summary>
    #region
    void Awake()
    {
        SetValues();
        // 소유권 자동 이전 방지
        //PV.OwnershipTransfer = OwnershipOption.Fixed;        

        CheckVersion();
    }

    void Update()
    {
        if (PV.IsMine)
        {
            AnimController();

            Multi_SendDialogue();

            Multi_ShowText();

            //if (채팅창이 열린 조건 && (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)))
            //{
            //    //엔터키로 채팅 보내는 기능
            //    //theNetwork.Send();
        }
    }

    
    void FixedUpdate()
    {
        if (canMove && PV.IsMine)
        {
            if (isMoblie)
            {
                JoystickMove();
            }

            else
            {
                Move();
            }
        }
    }
    #endregion


    /// <summary>
    /// 플레이어 OnTrigger
    /// </summary>
    /// <param name="collision"></param>

    #region
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (PV.IsMine)
        {
            if ((collision.tag == "Interaction" || collision.tag == "OnlyOneTouch" || collision.tag == "MainQuest") && (list.Count == 0 || list.Count == 1))
            {
                // 충돌한 오브젝트에서 InteractionDialogue_Multi 스크립트를 가져옴
                interactionScript = collision.GetComponent<InteractionDialogue_Multi>();
                //interactionQuest = collision.GetComponent<InteractionQuest_Multi>();
                confirmOnBtn.SetActive(true);

                if (!list.Contains(collision))
                {
                    list.Add(collision);
                }

                if (list[0] == collision)
                    ObjInteract(true, collision);
            }

            //센터라벨 개별 작동 시키는 조건문
            if (collision.tag == "Label")
            {
                interactionText = collision.GetComponent<InteractionText_Multi>();
                isColliding = true;
            }

            //p2독백 sound이벤트 개별 작동시키는 조건문
            if (collision.tag == "OnlyOneEvent")
            {
                Debug.Log("나만소리들을거");
                audioManager.EffectSoundPlay(1);
                collision.gameObject.SetActive(false);
                //이렇게하면 내것만 끄긴하는데 그렇다면 소리를 틀어주는것도 여기서만 진행해주면 독립이벤트가능?
            }
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (PV.IsMine)
        {
            if (collision.tag == "Label")
            {
                isColliding = false;
            }
            if (collision.tag == "Interaction" || collision.tag == "OnlyOneTouch" || collision.tag == "NeverTouch" || collision.tag == "MainQuest")
            {
                confirmOnBtn.SetActive(false);
                if (list.Count == 0)
                {
                    Debug.Log("리스트에 없으므로 종료");
                    return;
                }

                if (collision != null)
                {
                    if (list.Count == 0)
                    {
                        Debug.Log("리스트에 없으므로 종료");
                        return;
                    }

                    // 리스트의 첫 번째 아이템을 처리
                    if (list[0] == collision)
                    {
                        ObjInteract(false, list[0]);
                        list.RemoveAt(0);

                        if (list.Count == 1)
                        {
                            ObjInteract(true, list[0]);
                        }
                    }

                    // 리스트의 두 번째 아이템을 처리
                    if (list.Count == 2 && list[1] == collision)
                    {
                        Debug.Log("리스트1 빼기");
                        list.RemoveAt(1);
                    }
                }
                else
                {
                    Debug.LogError("충돌한 오브젝트를 찾을 수 없습니다: " + collision.name);
                }
            }
        }    
    }

    #endregion

    /// <summary>
    /// 플레이어 이동 관리 추상메서드
    /// playerSpeed값으로 이동속도 조절 가능
    /// </summary>
    #region
    public override void Move()
    {
        inputVec.x = Input.GetAxisRaw("Horizontal");
        inputVec.y = Input.GetAxisRaw("Vertical");

        Vector2 nextVec = inputVec.normalized * playerSpeed * Time.fixedDeltaTime;

        rigid.MovePosition(rigid.position + nextVec);
    }

    public override void JoystickMove()
    {
        inputVec.x = joystick.Horizontal;
        inputVec.y = joystick.Vertical;

        Vector2 nextVec = inputVec.normalized * playerSpeed * Time.fixedDeltaTime;

        rigid.MovePosition(rigid.position + nextVec);
    }
    #endregion

    /// <summary>
    /// 플레이어 애니메이션 관리 추상메서드
    /// </summary>
    #region
    public override void AnimController()
    {
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            // 좌우 키 입력이 있을 때
            if (inputVec.x != 0)
            {
                anim.SetBool("Walking", true);
                anim.SetFloat("DirX", inputVec.x);
                anim.SetFloat("DirY", 0);
            }
            // 상하 키 입력이 있을 때
            else if (inputVec.y != 0)
            {
                anim.SetBool("Walking", true);
                anim.SetFloat("DirY", inputVec.y);
            }
        }

        //좌우 방향키를 땠을 때
        if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow)
             || Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
        {
            inputVec.x = 0;
        }

        //상하 방향키를 땠을 때
        if (Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.DownArrow)
            || Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.S))
        {
            inputVec.y = 0;
        }

        if ((inputVec.x == 0 && inputVec.y == 0) || !canMove)
        {
            anim.SetBool("Walking", false);
        }

        else if (joystick != null && (joystick.Horizontal != 0 || joystick.Vertical != 0))
        {
            if (inputVec.x != 0)
            {
                anim.SetBool("Walking", true);
                anim.SetFloat("DirX", inputVec.x);

            }
            // 상하 키 입력이 있을 때
            if (inputVec.y != 0)
            {
                anim.SetBool("Walking", true);
                anim.SetFloat("DirY", inputVec.y);
            }
        }
    }


    #endregion


    /// <summary>
    /// 플레이어가 다른 오브젝트랑 상호작용하는 추상메서드
    /// 오브젝트랑 상호작용시 isTrigger체크되어있는 오브젝트의 색상 변경 메서드
    /// </summary>
    /// <param name="apply"></param>
    /// <param name="collision"></param>
    public override void ObjInteract(bool apply, Collider2D collision)
    {
        if (collision != null)
        {
            Renderer rend = collision.GetComponent<Renderer>();
            if (rend != null) //닿은 오브젝트의 renderer가 존재한다면
            {
                if (apply) //OnTriggerEnter의 값이 true이면서 HashSet에 아무것도 없을때
                {
                    //rend.material = PV.IsMine ? outlineMaterial : testMaterial; //OutLine 메테리얼 적용
                    rend.material = outlineMaterial;
                    //Debug.Log("색 변하는중");
                }

                else //OnTriggerEnter의 값이 false 혹은 HashSet에 값이 있다면
                {
                    rend.material = originalMaterial;
                    //Debug.Log("색 변하는중");
                }
            }
        }
    }

    /// <summary>
    /// 플레이어 속성 값 세팅하는 추상메서드
    /// </summary>
    #region
    public override void SetValues()
    {
        //플레이어 생성된 후에 팝업창, 이벤트 진행을 위해 받아야할 오브젝트
        theTM = FindObjectOfType<TextManager_Multi>();
        theQM = FindObjectOfType<QuestManager_Multi>();
        
        interactionText = FindObjectOfType<InteractionText_Multi>();

        audioManager = FindObjectOfType<AudioManager>();

        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        //theNetwork = FindObjectOfType<CreateRoomNetwork>();

        //닉네임 머리위에 띄우기 - 닉네임 입력해서 부여하는것도 가능(
        //PV.IsMine을 사용하지 않아야 다른 플레이어에게 닉네임 표시 가능 - 오히려 사용하면 안됨
        playerName.text = PV.Owner.NickName;        
        canMove = true;
    }
    #endregion

    /// <summary>
    /// WebGL / Unity / Window 환경 구분 메서드
    /// </summary>
#region
    public void CheckVersion()
    {
#if !(!UNITY_EDITOR && UNITY_WEBGL)
        // WebGL이 아닌 Unity일때는 조이스틱 off / unity 이동함수 사용
        isMoblie = false;
        //joystick.gameObject.SetActive(false);
        //webglBtn.gameObject.SetActive(false);
        ConrirmOn.sprite = Sprite[0];
#endif
        // WebGL이면서 모바일일때
        if (Application.isMobilePlatform)
        {
            //joystick.enabled = true;
            //webglBtn.SetActive(true);
            isMoblie = true;
            ConrirmOn.sprite = Sprite[1];
        }
        //WebGL이면서 컴퓨터 일때
        else
        {
            //joystick.gameObject.SetActive(false);
            // webglBtn.gameObject.SetActive(false);
            isMoblie = false;
            ConrirmOn.sprite = Sprite[0];
        }
    }
    #endregion
    /// <summary>
    /// 플레이어 개별 Dialogue보여주는 메서드
    /// </summary>
#region
    public void Multi_SendDialogue()
    {
        //플레이어의 confirmOnBtn이 활성화 되었을때 && F키를 눌렀을때 상호작용 이벤트
        if (confirmOnBtn.activeInHierarchy && Input.GetKeyDown(KeyCode.F))
        {
            // 스크립트가 존재하는지 확인한 후 SendDialogue 메서드 호출 / mainQuest가 아닐때
            if (interactionScript != null)
            {
                Debug.Log("IDM가져와서 팝업창 띄워준다???");
                SendDialogueToOthers();
            }
        }
    }
    #endregion
    /// <summary>
    /// 플레이어 개별 센터라벨 보여주는 메서드
    /// </summary>
#region
    public void Multi_ShowText()
    {
        //센터라벨 2초 개별 작동
        if (isColliding && theTM != null)
        {
            theTM.ShowText(interactionText.sentences);
            Invoke("CloseTMText", 2.0f);
            isColliding = false;
        }
    }
    #endregion
    //PUNRPC 메서드
    #region
    /// <summary>
    /// 마스터가 정답입력,메인퀘스트 공유해주는 메서드
    /// </summary>
    [PunRPC]
    public void SendDialogueToOthers()
    {
        interactionScript.SendDialogue();
    }

    /// <summary>
    /// 모든플레이어 정답처리? 
    /// 다른플레이어는 정답이 안적혀있으면 틀린다고뜨나?
    /// </summary>
    [PunRPC]
    public void ShowConfirmRPC()
    {
        Debug.Log("정답입력할게");
        theQM.ConfirmAnswer();
    }
    [PunRPC]
    void ShowTextRPC(string message)
    {
        theTM.ShowText(message); // theTM 인스턴스에서 ShowText 호출
        Invoke("CloseTMText", 2.0f);
    }
    /// <summary>
    /// 센터라벨 끄는 메서드
    /// </summary>
    public void CloseTMText()
    {
        theTM.CloseText();
    }
    #endregion
}