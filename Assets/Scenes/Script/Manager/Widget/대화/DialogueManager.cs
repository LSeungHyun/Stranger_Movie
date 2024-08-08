using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public WindowManager windowManager;
    public GameObject Dialogue;

    public GameObject CloseButton;
    public GameObject NextButton;
    public GameObject Sprite;
    public GameObject DialogueWindow;
    public Text text;
    public SpriteRenderer rendererSprite;
    public SpriteRenderer rendererDialogueWindow;

    private List<string> listSentences;
    private List<Sprite> listSprites;
    private List<Sprite> listDialogueWindows;
    private List<ObjDeletingEvent.SpriteAnimation> listSpriteAnimations;
    private List<AudioClip> listPageSounds;
    private List<bool> listPlaySoundOnce;
    private bool hasPlayedSound;

    private int count;

    public string Entersound;

    private PlayerManager thePlayer;
    private AudioManager theAudio;

    public bool talking = false;
    public bool keyActivated = false;

    void Start()
    {
        count = 0;
        text.text = "";
        listSentences = new List<string>();
        listSprites = new List<Sprite>();
        listDialogueWindows = new List<Sprite>();
        listSpriteAnimations = new List<ObjDeletingEvent.SpriteAnimation>();
        listPageSounds = new List<AudioClip>();
        listPlaySoundOnce = new List<bool>();
        theAudio = FindObjectOfType<AudioManager>();
        thePlayer = FindObjectOfType<PlayerManager>();
        CloseButton.GetComponent<Button>().onClick.AddListener(OnCloseButtonClick);

        if (NextButton != null)
        {
            NextButton.GetComponent<Button>().onClick.AddListener(OnNextButtonClick);
        }
    }

    public void ShowDialogue(Dialogue dialogue)
    {

        windowManager.OpenWindow(Dialogue);

        talking = true;
        thePlayer.notMove = true;

        listSentences.Clear();
        listSprites.Clear();
        listDialogueWindows.Clear();
        listSpriteAnimations.Clear();
        listPageSounds.Clear();
        listPlaySoundOnce.Clear();

        for (int i = 0; i < dialogue.sentences.Length; ++i)
        {
            listSentences.Add(dialogue.sentences[i]);
            listSprites.Add(dialogue.sprites[i]);
            listDialogueWindows.Add(dialogue.dialogueWindows[i]);
        }

        foreach (var spriteAnimation in dialogue.spriteAnimations)
        {
            listSpriteAnimations.Add(spriteAnimation);
        }


            foreach (var sound in dialogue.pageSounds)
            {
                listPageSounds.Add(sound);
            }

            foreach (var playOnce in dialogue.playSoundOnce)
            {
                listPlaySoundOnce.Add(playOnce);
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
        thePlayer.notMove = false;
        listSentences.Clear();
        listSprites.Clear();
        listDialogueWindows.Clear();
        listSpriteAnimations.Clear();
        listPageSounds.Clear();
        listPlaySoundOnce.Clear();
        talking = false;
        keyActivated = false;

        CloseButton.SetActive(false);

        if (NextButton != null)
        {
            NextButton.SetActive(false);
        }

        text.gameObject.SetActive(false);
        Sprite.SetActive(false);
        DialogueWindow.SetActive(false);
        windowManager.CloseWindow();
        StopAllCoroutines();
    }

    IEnumerator StartDialogueCoroutine()
    {
        StopCurrentAnimation();

        if (count > 0)
        {
            if (listDialogueWindows[count] != listDialogueWindows[count - 1])
            {
                yield return new WaitForSeconds(0.2f);
                rendererDialogueWindow.sprite = listDialogueWindows[count];
                rendererSprite.sprite = listSprites[count];
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

            RuntimeAnimatorController animatorController = null;
            foreach (var spriteAnimation in listSpriteAnimations)
            {
                if (spriteAnimation.sprite == listSprites[count])
                {
                    animatorController = spriteAnimation.animatorController;
                    break;
                }
            }

            if (animatorController != null)
            {
                Animator animator = rendererSprite.GetComponent<Animator>();
                if (animator != null)
                {
                    animator.runtimeAnimatorController = animatorController;
                    animator.Play("AnimationState");
                }
            }
        }

        PlayPageSound();

        keyActivated = true;
        text.text = listSentences[count];

        UpdateButtons();
    }

    private void PlayPageSound()
    {
        if (count >= 0 && count < listPageSounds.Count)
        {
            AudioClip clip = listPageSounds[count];
            if (clip != null)
            {
                if (!listPlaySoundOnce[count] || (listPlaySoundOnce[count] && !hasPlayedSound))
                {
                    theAudio.Play(clip); 
                    hasPlayedSound = true;
                }
            }
            else
            {
                hasPlayedSound = false;
            }
        }
        else
        {
            hasPlayedSound = false;
        }
    }


    private void StopCurrentAnimation()
    {
        Animator animator = rendererSprite.GetComponent<Animator>();
        if (animator != null)
        {
            animator.runtimeAnimatorController = null; 
        }
    }

    private void OnCloseButtonClick()
    {
        ExitDialogue();
    }

    void Update()
    {
        if (talking && keyActivated)
        {
            if (!CloseButton.activeInHierarchy && Input.GetKeyDown(KeyCode.Space))
            {
                IncrementCount();
            }
            if (CloseButton.activeInHierarchy && (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Escape)))
            {
                ExitDialogue();
            }
        }
    }

    private void OnNextButtonClick()
    {
        IncrementCount();
    }

    private void IncrementCount()
    {
        if (count < listSentences.Count - 1)
        {
            count++;
            theAudio.Play(Entersound);
        }
        keyActivated = false;
        StopAllCoroutines();
        StartCoroutine(StartDialogueCoroutine());
    }

    private void UpdateButtons()
    {
        if (count == listSentences.Count - 1)
        {
            CloseButton.SetActive(true);
            if (NextButton != null)
            {
                NextButton.SetActive(false);
            }
        }
        else
        {
            CloseButton.SetActive(false);
            if (NextButton != null)
            {
                NextButton.SetActive(true);
            }
        }
    }
}
