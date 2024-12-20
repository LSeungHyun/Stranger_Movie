using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //기존 namespace

public abstract class AbstractPlayer_Multi : MonoBehaviour
{
    public PhotonView PV;

    //플레이어가 상호작용,센터라벨을 실행하기 위한 변수
    public TextManager_Multi theTM;
    public QuestManager_Multi theQM;
    public InteractionDialogue_Multi interactionScript;
    public InteractionText_Multi interactionText;

    [Header("*[ SoundManager ]*")]
    public AudioManager audioManager;

    //[Header("*[ Network for Chat ]*")]
    //public CreateRoomNetwork theNetwork;

    //Rigidbody, Animator
    [HideInInspector]
    public Rigidbody2D rigid;
    [HideInInspector]
    public Animator anim;

    [Header("*[ Direct Assignment ]*")]
    //플레이어 닉네임
    public Text playerName;

    //모바일 UI오브젝트
    public FixedJoystick joystick; //조이스틱 프리팹
    public GameObject webglBtn; //웹에서 사용하는 버튼

    //플레이어 상호작용 안내 Sprite
    public Sprite[] Sprite; //모바일,컴퓨터에 각각 다른 이미지 sprite
    public GameObject confirmOnBtn;
    public SpriteRenderer ConrirmOn;

    //상호작용을 위한 메테리얼
    public Material originalMaterial;
    public Material outlineMaterial;
    //상대방에게 보이는 상호작용 색
    public Material testMaterial;

    //상호작용되는 오브젝트 받아올 리스트
    //[HideInInspector]
    public List<Collider2D> list = new List<Collider2D>();

    [HideInInspector]
    public string lastMapName; //맵 이동에 사용되는 변수
    [HideInInspector]
    public string currentMapName; //맵 이동에 사용되는 변수

    [HideInInspector]
    public int isSceneCount = 0; //사운드 관리 씬 카운트 변수
    [HideInInspector]
    public float playerSpeed = 150f;

    [HideInInspector]
    public Vector2 inputVec;

    [Header("*[ Boolen Type Variable ]*")]
    public bool canMove; //팝업창 On/Off에 따라 플레이어 이동을 제한하는 bool값
    //[HideInInspector]
    public bool isTransfer; //씬 이동을 체크하는 bool값
    //[HideInInspector]
    protected bool isMoblie; //모바일인지 아닌지 판단하는 bool값
    public bool isColliding = false;
    public bool isMainQuest = false;

    public abstract void Move(); //플레이어 이동 메서드 - Unity
    public abstract void JoystickMove(); //플레이어 이동 메서드 - WebGL(조이스틱)
    public abstract void AnimController(); //플레이어 애니메이션 회전 메서드
    public abstract void SetValues(); //플레이어 속성 값 세팅 메서드
    public abstract void ObjInteract(bool apply, Collider2D collision); //오브젝트 상호작용(색변환) 관리 메서드
}