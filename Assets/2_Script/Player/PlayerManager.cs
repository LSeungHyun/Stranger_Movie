using UnityEngine;

public class PlayerManager : AbstractPlayer
{
    #region Lifecycle Methods
    void Awake()
    {
        ConfigurePlatformSettings();
    }

    void Update()
    {
        AnimController();
    }
    void FixedUpdate()
    {
        if (canMove)
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

    #region OnTrigger Methods
    /// <summary>
    /// 플레이어 OnTrigger
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.CompareTag("Interaction")) && (list.Count == 0 || list.Count == 1))
        {
            if (!list.Contains(collision))
                list.Add(collision);

            if (list[0] == collision)
                ObjInteract(true, collision);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Interaction"))
        {
            if (list.Count == 0) return;

            if (list[0] == collision)
            {
                ObjInteract(false, list[0]);
                list.RemoveAt(0);

                if (list.Count > 0)
                {
                    ObjInteract(true, list[0]);
                }
            }
            else if (list.Count > 1 && list[1] == collision)
            {
                list.RemoveAt(1);
            }
        }
    }
    #endregion

    #region Movement Methods
    /// <summary>
    /// 플레이어 이동 관리 추상메서드
    /// playerSpeed값으로 이동속도 조절 가능
    /// </summary>
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

    #region Animation Methods
    /// <summary>
    /// 플레이어 애니메이션 관리 추상메서드
    /// </summary>
    public override void AnimController()
    {
        bool isMoving = inputVec.x != 0 || inputVec.y != 0;
        if (isMoving || joystick != null && (joystick.Horizontal != 0 || joystick.Vertical != 0))
        {
            anim.SetBool("Walking", true);

            if (inputVec.x != 0)
            {
                anim.SetFloat("DirX", inputVec.x);
                anim.SetFloat("DirY", 0);
            }
            else if (inputVec.y != 0)
            {
                anim.SetFloat("DirX", 0);
                anim.SetFloat("DirY", inputVec.y);
            }
        }
        else
        {
            anim.SetBool("Walking", false);
        }

        ResetInputOnKeyUp();
    }

    public override void ResetInputOnKeyUp()
    {
        if (IsKeyPressed(horizontalKeys))
        {
            inputVec.x = 0;
        }

        if (IsKeyPressed(verticalKeys))
        {
            inputVec.y = 0;
        }
    }
    
    #endregion

    #region ObjInteract Methodes
    /// <summary>
    /// 오브젝트 상호작용 처리
    /// </summary>
    /// <param name="apply">상호작용 적용 여부</param>
    /// <param name="collision">대상 Collider2D</param>
    public override void ObjInteract(bool apply, Collider2D collision)
    {
        if(collision == null) return;

        Renderer rend = collision.GetComponent<Renderer>();
        if (rend != null)
        {
            rend.material = apply ? outlineMaterial : originalMaterial;
        }
    }
    #endregion

    #region Utility Methods
    /// <summary>
    /// Unity Editor / WebGL 판단 메서드
    /// </summary>
    public override void ConfigurePlatformSettings()
    {
#if !(!UNITY_EDITOR && UNITY_WEBGL)
        isMoblie = false;
        confirmOnSprite.sprite = sprite[0];
#endif
        if (Application.isMobilePlatform)
        {
            joystick.enabled = true;
            webglBtn.SetActive(true);
            isMoblie = true;
            confirmOnSprite.sprite = sprite[1];
        }
        else
        {
            isMoblie = false;
            confirmOnSprite.sprite = sprite[0];
        }
    }
    #endregion
}