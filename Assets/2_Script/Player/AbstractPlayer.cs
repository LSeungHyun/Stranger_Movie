using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractPlayer : MonoBehaviour
{
    [Header("Player Components")]
    public Rigidbody2D rigid;
    public Animator anim;

    [Header("Player Interaction")]
    public Sprite[] sprite;
    public SpriteRenderer confirmOnSprite;
    public Material originalMaterial;
    public Material outlineMaterial;

    [Header("Direct Assignment")]
    public FixedJoystick joystick;
    public GameObject webglBtn;

    [Header("Interaction Data")]
    [HideInInspector]
    public List<Collider2D> list = new List<Collider2D>();

    [Header("Map and Scene Management")]
    [HideInInspector]
    public string lastMapName;
    [HideInInspector]
    public string currentMapName;

    [Header("Player Movement")]
    [HideInInspector]
    public float playerSpeed = 150f;
    [HideInInspector]
    public Vector2 inputVec;

    [Header("Boolean Variables")]
    public bool canMove = true;
    public bool isMoblie;

    [Header("Input Keys")]
    public readonly KeyCode[] horizontalKeys = { KeyCode.LeftArrow, KeyCode.RightArrow, KeyCode.A, KeyCode.D };
    public readonly KeyCode[] verticalKeys = { KeyCode.UpArrow, KeyCode.DownArrow, KeyCode.W, KeyCode.S };

    #region Movement Methods
    public abstract void Move(); //플레이어 이동 메서드 - Unity
    public abstract void JoystickMove(); //플레이어 이동 메서드 - WebGL(조이스틱)
    #endregion

    #region Animation Methods
    public abstract void AnimController(); //플레이어 애니메이션 회전 메서드
    public abstract void ResetInputOnKeyUp();
    #endregion

    #region ObjInteract
    public abstract void ObjInteract(bool apply, Collider2D collision); //오브젝트 상호작용(색변환) 관리 메서드
    #endregion

    #region Utility Methods
    public abstract void ConfigurePlatformSettings(); //Unity Editor / WebGL 판단 메서드
    #endregion

    #region KeyCode Input
    /// <summary>
    /// 전달된 KeyCode 중 하나라도 눌렸는지 확인
    /// </summary>
    /// <param name="keys">확인할 KeyCode 배열</param>
    /// <returns>키가 눌렸으면 true</returns>
    public bool IsKeyPressed(params KeyCode[] keys)
    {
        foreach (var key in keys)
        {
            if (Input.GetKeyDown(key))
            {
                return true;
            }
        }
        return false;
    }
    #endregion
}