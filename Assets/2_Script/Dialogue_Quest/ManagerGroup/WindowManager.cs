using UnityEngine;

public class WindowManager : MonoBehaviour
{
    [Header("UI Windows")]
    public GameObject DialogueWindow;
    public GameObject TextWindow;
    public GameObject Button;

    #region Window Control Methods
    /// <summary>
    /// 모든 창을 비활성화
    /// </summary>
    public void DeactivateAllWindows()
    {
        SetWindowState(false, false, false);
    }
    /// <summary>
    /// 특정 창만 활성화, 나머지는 비활성화
    /// </summary>
    /// <param name="windowToOpen">활성화할 창</param>
    public void OpenWindow(GameObject windowToOpen)
    {
        DeactivateAllWindows();
        if (windowToOpen != null)
        {
            windowToOpen.SetActive(true);
        }
    }

    /// <summary>
    /// 대화 창과 텍스트 창을 닫고 버튼을 활성화
    /// </summary>
    public void CloseWindow()
    {
        SetWindowState(true, false, false);
    }

    /// <summary>
    /// 창들의 상태를 설정
    /// </summary>
    /// <param name="isButtonActive">버튼 활성화 여부</param>
    /// <param name="isDialogueActive">대화 창 활성화 여부</param>
    /// <param name="isTextActive">텍스트 창 활성화 여부</param>
    private void SetWindowState(bool isButtonActive, bool isDialogueActive, bool isTextActive)
    {
        Button.SetActive(isButtonActive);
        DialogueWindow.SetActive(isDialogueActive);
        TextWindow.SetActive(isTextActive);
    }
    #endregion
}