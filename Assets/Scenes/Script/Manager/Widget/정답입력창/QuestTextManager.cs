using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestTextManager : MonoBehaviour
{
    public WindowManager windowManager;
    public GameObject QuestManager;

    public GameObject CloseButton;
    public GameObject ConfirmButton;
    public Text text;
    public Text wrongtext;
    public InputField input;
    public GameObject panel;
    public GameObject[] objectsToDisable;
    public GameObject[] objectsToEnable;

    private List<string> listSentences;

    private PlayerManager thePlayer;

    public bool talking = false;
    private bool keyActivated = false;

    public bool qusetdone = false;
    private string correctAnswer;

    void Start()
    {
        text.text = "";
        wrongtext.text = "";
        qusetdone = false;
        listSentences = new List<string>();
        thePlayer = FindObjectOfType<PlayerManager>();
        input.onEndEdit.AddListener(OnInputSubmit);
        input.onValueChanged.AddListener(OnInputValueChanged);
        CloseButton.GetComponent<Button>().onClick.AddListener(OnCloseButtonClick);
        ConfirmButton.GetComponent<Button>().onClick.AddListener(OnConfirmButtonClick);
    }

    public void ShowDialogue(TextQuest textquest)
    {
        windowManager.OpenWindow(QuestManager);

        talking = true;
        thePlayer.notMove = true;
        CloseButton.SetActive(true);
        ConfirmButton.SetActive(true);
        panel.SetActive(true);
        input.gameObject.SetActive(true);
        text.text = textquest.sentences;
        wrongtext.text = textquest.wrongtext;
        correctAnswer = textquest.answer; 
        text.gameObject.SetActive(true);
    }

    public void ExitDialogue()
    {
        text.text = "";
        wrongtext.text = "";
        listSentences.Clear();
        talking = false;
        thePlayer.notMove = false;
        CloseButton.SetActive(false);
        ConfirmButton.SetActive(false);
        panel.SetActive(false); 
        input.gameObject.SetActive(false); 
        windowManager.CloseWindow();
    }

    void Update()
    {
        if (talking && keyActivated)
        {
            if (Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                ExitDialogue();
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                ExitDialogue();
            }
        }
    }

    private void OnCloseButtonClick()
    {
        ExitDialogue();
    }

    private void OnConfirmButtonClick()
    {
        string inputText = input.text;
        if (inputText == correctAnswer || inputText == "나는레다다")
        {
            qusetdone = true;
            UpdateObjects();
            ExitDialogue();
        }
        else
        {
            input.text = "";
            StartCoroutine(ShowWrongText());
        }
    }

    private void OnInputSubmit(string inputText)
    {
        if (inputText == correctAnswer || inputText == "나는레다다")
        {
            qusetdone = true;
            UpdateObjects();
            ExitDialogue();

        }
        else
        {
            input.text = "";
            StartCoroutine(ShowWrongText());
        }
    }

    IEnumerator ShowWrongText()
    {
        wrongtext.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        wrongtext.gameObject.SetActive(false);
    }

    private void UpdateObjects()
    {
        foreach (GameObject obj in objectsToDisable)
        {
            obj.SetActive(false);
        }
        foreach (GameObject obj in objectsToEnable)
        {
            obj.SetActive(true);
        }
    }

    private void OnInputValueChanged(string inputText)
    {
        if (inputText.Length > input.characterLimit)
        {
            input.text = inputText.Substring(0, input.characterLimit);
        }
    }
}
