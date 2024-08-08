using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextBoxManager : MonoBehaviour
{
    public Text text;
    public GameObject panel;
    public GameObject TextManager;

    private List<string> listSentences;

    private PlayerManager thePlayer;

    public bool talking = false;
    private bool keyActivated = false;

    void Start()
    {
        text.text = "";
        listSentences = new List<string>();
        thePlayer = FindObjectOfType<PlayerManager>();
    }

    public void ShowDialogue(TextBox textbox)
    {
        talking = true;
        TextManager.SetActive(true);
        panel.SetActive(true);
        text.text = textbox.sentences; 
        text.gameObject.SetActive(true);
    }

    public void ExitDialogue()
    {
        text.text = "";
        TextManager.SetActive(false);
        listSentences.Clear();
        talking = false;
        panel.SetActive(false);
        text.gameObject.SetActive(false);
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
}
