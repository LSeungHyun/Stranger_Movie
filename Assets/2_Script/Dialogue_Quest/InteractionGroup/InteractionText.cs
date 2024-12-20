using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InteractionText : MonoBehaviour
{
    [Header("Managers")]
    public TextManager textManager;

    //출력할 텍스트
    [TextArea]
    public string sentences; // 출력할 텍스트
    public float closeCount = 2.0f; // 대화창 잔류 시간

    private bool isColliding = false; // 충돌 상태 확인
    private bool isActive = false; // 텍스트가 활성 상태인지 확인

    void Awake()
    {
        textManager = FindObjectOfType<TextManager>();
    }

    void Update()
    {
        HandleInteraction();
    }

    #region OnTrigger
    private void OnTriggerEnter2D(Collider2D collision)
    {
        isColliding = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        isColliding = false;
    }
    #endregion

    #region HandleInteraction
    /// <summary>
    /// Interaction 로직을 처리합니다.
    /// </summary>
    private void HandleInteraction()
    {
        if (isColliding && !isActive)
        {
            ShowTMText();
        }
    }
    #endregion

    #region Text Management
    /// <summary>
    /// 텍스트를 표시하고 타이머를 설정합니다.
    /// </summary>
    private void ShowTMText()
    {
        textManager.ShowText(sentences);
        isActive = true;
        Invoke("CloseTMText", closeCount);
    }
    void CloseTMText()
    {
        textManager.CloseText();
        isActive = false;
    }
}
#endregion