using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpinnerManager : MonoBehaviour
{
    public WindowManager windowManager;
    public GameObject DialogueManager;

    public GameObject LeftButton;
    public GameObject RightButton;
    public GameObject CloseButton;
    public GameObject Sprite;
    public GameObject DialogueWindow;

    public Text text;
    public SpriteRenderer rendererSprite;
    public SpriteRenderer rendererDialogueWindow;

    private List<string> listSentences;
    private List<Sprite> listSprites;
    private List<Sprite> listDialogueWindows;

    private int count;

    public string Entersound;

    private PlayerManager thePlayer;
    private AudioManager theAudio;
    private OrderManager theOrder;

    public bool talking = false;
    private bool keyActivated = false;

    static public SpinnerManager instance;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            DontDestroyOnLoad(this.gameObject);
            instance = this;
        }
    }
    void Start()
    {
        count = 0;
        text.text = "";
        listSentences = new List<string>();
        listSprites = new List<Sprite>();
        listDialogueWindows = new List<Sprite>();
        theAudio = FindObjectOfType<AudioManager>();
        thePlayer = FindObjectOfType<PlayerManager>();

        // Add button click listeners
        if (RightButton != null)
        {
            RightButton.GetComponent<Button>().onClick.AddListener(OnRightButtonClick);
        }
        if (LeftButton != null)
        {
            LeftButton.GetComponent<Button>().onClick.AddListener(OnLeftButtonClick);
        }
        if (CloseButton != null)
        {
            CloseButton.GetComponent<Button>().onClick.AddListener(OnCloseButtonClick);
        }
    }

    public void ShowDialogue(Dialogue dialogue)
    {
        windowManager.OpenWindow(DialogueManager);

        talking = true;
        thePlayer.notMove = true;

        LeftButton.SetActive(true);
        RightButton.SetActive(true);
        CloseButton.SetActive(true);

        listSentences.Clear();
        listSprites.Clear();
        listDialogueWindows.Clear();

        for (int i = 0; i < dialogue.sentences.Length; ++i)
        {
            listSentences.Add(dialogue.sentences[i]);
            listSprites.Add(dialogue.sprites[i]);
            listDialogueWindows.Add(dialogue.dialogueWindows[i]);
        }

        text.gameObject.SetActive(true);
        Sprite.SetActive(true);
        DialogueWindow.SetActive(true);
        StartCoroutine(StartDialogueCoroutine());
    }

    public void ExitDialogue()
    {
        text.text = "";
        count = 0;
        talking = false;
        thePlayer.notMove = false;
        listSentences.Clear();
        listSprites.Clear();
        listDialogueWindows.Clear();
        text.gameObject.SetActive(false);
        LeftButton.SetActive(false);
        RightButton.SetActive(false);
        CloseButton.SetActive(false);
        Sprite.SetActive(false);
        DialogueWindow.SetActive(false);
        windowManager.CloseWindow();
    }

    IEnumerator StartDialogueCoroutine()
    {
        if (count > 0)
        {
            if (listDialogueWindows[count] != listDialogueWindows[count - 1])
            {
                rendererDialogueWindow.enabled = false;
                yield return new WaitForSeconds(0.2f);
                rendererDialogueWindow.sprite = listDialogueWindows[count];
                rendererSprite.sprite = listSprites[count];
                rendererDialogueWindow.enabled = true;
            }
            else
            {
                if (listSprites[count] != listSprites[count - 1])
                {
                    yield return new WaitForSeconds(0.1f);
                    rendererSprite.sprite = listSprites[count];
                }
                else
                {
                    yield return new WaitForSeconds(0.05f);
                }
            }
        }
        else
        {
            rendererDialogueWindow.sprite = listDialogueWindows[count];
            rendererSprite.sprite = listSprites[count];
        }

        keyActivated = true;
        text.text = listSentences[count];
    }

    void Update()
    {
        if (talking && keyActivated)
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            {
                IncrementCount();
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            {
                DecrementCount();
            }
            if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Escape))
            {
                ExitDialogue();
            }
        }
    }

    private void IncrementCount()
    {
        count++;
        if (count >= listSentences.Count)
        {
            count = 0; 
        }
        keyActivated = false;
        theAudio.Play(Entersound);
        StopAllCoroutines();
        StartCoroutine(StartDialogueCoroutine());
    }

    private void DecrementCount()
    {
        count--;
        if (count < 0)
        {
            count = listSentences.Count - 1; 
        }
        keyActivated = false;
        theAudio.Play(Entersound);
        StopAllCoroutines();
        StartCoroutine(StartDialogueCoroutine());
    }

    private void OnRightButtonClick()
    {
        IncrementCount();
    }

    private void OnLeftButtonClick()
    {
        DecrementCount();
    }

    private void OnCloseButtonClick()
    {
        ExitDialogue();
    }
}
