using UnityEngine;
using UnityEngine.UI;

public class ConfirmButton : MonoBehaviour
{
    private Button confirmButton;

    private void Awake()
    {
        confirmButton = GetComponent<Button>();
        confirmButton.onClick.AddListener(OnConfirmButtonClicked);
    }

    private void OnConfirmButtonClicked()
    {
        Debug.Log("»Æ¿Œ");
    }
}
