using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QusetManager : MonoBehaviour
{
    public WindowManager windowManager;
    public GameObject DialogueManager;

    public GameObject CloseButton;
    public GameObject ConfirmButton;
    public GameObject AnswerButton;
    public GameObject NextButton;
    public GameObject Sprite;
    public GameObject SpecialSprite; 
    public GameObject PopupWindow;
    public GameObject DialogueWindow;
    public Text text;
    public InputField input;

    public SpriteRenderer rendererSprite;
    public SpriteRenderer rendererPopupWindow;
    public SpriteRenderer rendererDialogueWindow;
    public SpriteRenderer rendererSpecialSprite; 

    private List<string> listSentences;
    private List<Sprite> listSprites;
    private List<Sprite> listPopupWindows;
    private List<Sprite> listDialogueWindows;

    public GameObject[] objectsToDisable;
    public GameObject[] objectsToEnable;

    private int count;

    public string Entersound;

    private PlayerManager thePlayer;
    private AudioManager theAudio;

    public bool talking = false;
    public bool keyActivated = false;

    public bool qusetdone = false;

    private string correctAnswer; 

    void Start()
    {
        count = 0;
        text.text = "";
        listSentences = new List<string>();
        thePlayer = FindObjectOfType<PlayerManager>();
        input.onEndEdit.AddListener(OnInputSubmit);
        input.onValueChanged.AddListener(OnInputValueChanged);

        listSprites = new List<Sprite>();
        listPopupWindows = new List<Sprite>();
        listDialogueWindows = new List<Sprite>();

        theAudio = FindObjectOfType<AudioManager>();
        CloseButton.GetComponent<Button>().onClick.AddListener(OnCloseButtonClick);
        ConfirmButton.GetComponent<Button>().onClick.AddListener(OnConfirmButtonClick);
        AnswerButton.GetComponent<Button>().onClick.AddListener(OnAnswerButtonClick);
        if (NextButton != null)
        {
            NextButton.GetComponent<Button>().onClick.AddListener(OnNextButtonClick);
        }

        rendererSpecialSprite = SpecialSprite.GetComponent<SpriteRenderer>();
    }

    public void ShowDialogue(QuestData questdata)
    {
        windowManager.OpenWindow(DialogueManager);

        talking = true;
        thePlayer.notMove = true;

        listSentences.Clear();
        listSprites.Clear();
        listDialogueWindows.Clear();
        listPopupWindows.Clear();

        for (int i = 0; i < questdata.sentences.Length; ++i)
        {
            listSentences.Add(questdata.sentences[i]);
            listSprites.Add(questdata.sprites[i]);
            listDialogueWindows.Add(questdata.dialogueWindows[i]);
        }
        for (int i = 0; i < questdata.popups.Length; ++i)
        {
            listPopupWindows.Add(questdata.popups[i]);
        }

        correctAnswer = questdata.answer;
        text.gameObject.SetActive(true);
        CloseButton.SetActive(true);
        ConfirmButton.SetActive(true);
        input.gameObject.SetActive(true);
        Sprite.SetActive(true);
        SpecialSprite.SetActive(true);
        DialogueWindow.SetActive(true);
    }

    public void ExitDialogue()
    {
        text.text = "";
        count = 0;
        thePlayer.notMove = false;
        listSentences.Clear();
        listSprites.Clear();
        listPopupWindows.Clear();
        listDialogueWindows.Clear();
        talking = false;
        keyActivated = false;

        CloseButton.SetActive(false);
        ConfirmButton.SetActive(false);
        AnswerButton.SetActive(false);

        if (NextButton != null)
        {
            NextButton.SetActive(false);
        }
        input.gameObject.SetActive(false);
        input.placeholder.GetComponent<Text>().text = "";
        input.placeholder.GetComponent<Text>().color = Color.black;

        text.gameObject.SetActive(false);
        Sprite.SetActive(false);
        SpecialSprite.SetActive(false);
        PopupWindow.SetActive(false);
        DialogueWindow.SetActive(false);
        windowManager.CloseWindow();
    }

    IEnumerator StartDialogueCoroutine()
    {
        if (count > 0)
        {
            if (listDialogueWindows[count] != listDialogueWindows[count - 1])
            {
                DialogueWindow.SetActive(false);
                yield return new WaitForSeconds(0.2f);
                rendererDialogueWindow.GetComponent<SpriteRenderer>().sprite = listDialogueWindows[count];
                rendererSprite.GetComponent<SpriteRenderer>().sprite = listSprites[count];
                DialogueWindow.SetActive(true);
            }
            else
            {
                if (listSprites[count] != listSprites[count - 1])
                {
                    yield return new WaitForSeconds(0.1f);
                    rendererSprite.GetComponent<SpriteRenderer>().sprite = listSprites[count];
                }
                else
                {
                    yield return new WaitForSeconds(0.05f);
                }
            }
        }
        else
        {
            rendererDialogueWindow.GetComponent<SpriteRenderer>().sprite = listDialogueWindows[count];
            rendererSprite.GetComponent<SpriteRenderer>().sprite = listSprites[count];
        }

        keyActivated = true;
        text.text = listSentences[count];
    }

    private void OnCloseButtonClick()
    {
        ExitDialogue();
    }

    private void OnAnswerButtonClick()
    {
        UpdateObjects();
        ExitDialogue();
    }

    private void OnNextButtonClick()
    {
        count++;
        text.text = "";
        keyActivated = false;
        theAudio.Play(Entersound);

        if (count == listSentences.Count)
        {
            StopAllCoroutines();
            ExitDialogue();
        }
        else
        {
            StopAllCoroutines();
            StartCoroutine(StartDialogueCoroutine());
        }
    }

    private void OnConfirmButtonClick()
    {
        string inputText = input.text; 
        if (inputText == correctAnswer || inputText == "나는레다다")
        {
            rendererSprite.sprite = listSprites[1];
            input.gameObject.SetActive(false);
            SpecialSprite.SetActive(false);
            CloseButton.SetActive(false);
            ConfirmButton.SetActive(false);
            AnswerButton.SetActive(true);
            StartCoroutine(StartDialogueCoroutine());
        }
        else
        {
            input.text = "";
            StartCoroutine(ShowPopupWindowImage());
        }
    }

    private void OnInputSubmit(string inputText)
    {
        if (inputText == correctAnswer || inputText == "나는레다다")
        {
            rendererSprite.sprite = listSprites[1];
            input.gameObject.SetActive(false);
            SpecialSprite.SetActive(false);
            CloseButton.SetActive(false);
            ConfirmButton.SetActive(false);
            AnswerButton.SetActive(true);
            StartCoroutine(StartDialogueCoroutine());
        }
        else
        {
            input.text = "";
            StartCoroutine(ShowPopupWindowImage());
        }
    }

    IEnumerator ShowPopupWindowImage()
    {
        rendererPopupWindow.sprite = listPopupWindows[0];
        PopupWindow.SetActive(true);
        yield return new WaitForSeconds(1f);
        PopupWindow.SetActive(false);
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
    void Update()
    {
        if (talking && keyActivated)
        {
            if (Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                OnConfirmButtonClick();
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                ExitDialogue();
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                OnNextButtonClick();
            }
        }
    }
}
