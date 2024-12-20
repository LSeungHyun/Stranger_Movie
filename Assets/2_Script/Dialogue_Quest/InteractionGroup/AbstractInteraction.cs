using System.Linq;
using UnityEngine;

public abstract class AbstractInteraction : MonoBehaviour
{
    [Header("UI Components")]
    public GameObject confirmOn;
    public WebGLBtn webglBtn;

    [Header("Interaction Settings")]
    public bool isColliding = false;

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
    public abstract void HandleInteractionEvent();

    #region OnTrigger
    public void OnTriggerEnter2D(Collider2D collision)
    {
        isColliding = true;
    }
    public virtual void OnTriggerExit2D(Collider2D collision)
    {
        isColliding = false;
        confirmOn.SetActive(false);
    }
    #endregion

    #region FindComponents
    public virtual void FindComponents()
    {
        webglBtn = Resources.FindObjectsOfTypeAll<WebGLBtn>().FirstOrDefault();
        confirmOn = Resources.FindObjectsOfTypeAll<GameObject>().FirstOrDefault(g => g.name == "ConfirmOn");
    }
    #endregion

    #region InteractionTrigger
    /// <summary>
    /// 입력이나 WebGL 버튼 클릭 상태를 확인
    /// </summary>
    /// <returns>상호작용 트리거 여부</returns>
    public bool IsInteractionTriggered()
    {
        return Input.GetKeyDown(KeyCode.F) || (webglBtn?.isClick ?? false);
    }
}
#endregion