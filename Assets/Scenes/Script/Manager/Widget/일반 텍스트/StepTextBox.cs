using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepTextBox : MonoBehaviour
{
    [SerializeField]
    public TextBox sentences;
    public GameObject[] objectsToDisable;
    public GameObject[] objectsToEnable; 

    private TextBoxManager theTBM;
    private PlayerManager thePlayer;

    void Start()
    {
        theTBM = FindObjectOfType<TextBoxManager>();
        thePlayer = FindObjectOfType<PlayerManager>();
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
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            theTBM.ShowDialogue(sentences);
            StartCoroutine(CloseDialogueAfterDelay(2f));
        }
    }


    private IEnumerator CloseDialogueAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        theTBM.ExitDialogue();
        UpdateObjects();
    }
}
