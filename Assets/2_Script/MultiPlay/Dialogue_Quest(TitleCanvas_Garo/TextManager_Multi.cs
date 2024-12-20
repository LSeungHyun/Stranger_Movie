using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TextManager_Multi : MonoBehaviour
{
    [Header("Managers")]
    public DialManager_Multi dialManager;
    public QuestManager_Multi questManager;

    [Header("Caching Object")]
    public GameObject textWindow;
    public Text textComponent;

    private void Awake()
    {
        CloseText();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        CloseText();
    }

    //대화창을 여는 함수
    public void ShowText(string textbox)
    {
        if (textbox == null || dialManager.isTalking || questManager.isTalking)
            return;

        textComponent.text = textbox;
        textWindow.SetActive(true);
    }

    // 대화창을 닫는 함수
    public void CloseText()
    {
        textComponent.text = "";
        textWindow.SetActive(false);
    }
}