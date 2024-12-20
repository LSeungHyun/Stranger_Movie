using System.Linq;
using UnityEngine;

public class DialTriggerData : MonoBehaviour
{
    //현재는 사용하지 않지만 추후에 가져다 사용하게 될 cs

    //오브젝트 캐싱
    public GameObject confirmOn;
    public WebGLBtn webglBtn;

    //자기자신한테 달린 dial 스크립트
    private InteractionDialogue interactionDialogue;
    public DialManager dialManager;

    //db관련된 문자열 변수
    public string personName;
    public string questName;
    public string itemName;
    public string[] useditemName;

    //대화 시작중에 적용할것인가?
    public bool isStartTrigger = false;
    //닿기만 해도 적용할것인가?
    public bool isTouch = false;

    private void Awake()
    {
        interactionDialogue = GetComponent<InteractionDialogue>();
        dialManager = FindObjectOfType<DialManager>();
        webglBtn = Resources.FindObjectsOfTypeAll<WebGLBtn>().FirstOrDefault();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (interactionDialogue != null)
        {
            interactionDialogue.onDialogueStartedData += StartDataEdit;
            dialManager.onDialogueEndedData += EndDataEdit;
            if (isTouch)
            {
                ChangeDb();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (interactionDialogue != null)
        {
            interactionDialogue.onDialogueStartedData -= StartDataEdit;
            dialManager.onDialogueEndedData -= EndDataEdit;
        }
    }


    public void StartDataEdit()
    {
        if (isStartTrigger)
        {
            ChangeDb();
        }

    }

    public void EndDataEdit()
    {
        if (!isStartTrigger)
        {
            ChangeDb();
        }
    }

    public void ChangeDb()
    {
        if (personName != null)
        {
            UpdatePersonStatusInDatabase(personName, true);
        }
        if (questName != null)
        {
            UpdateQuestStatusInDatabase(questName, QuestStatus.Seen);
        }
        if (itemName != null)
        {
            UpdateItemStatusInDatabase(itemName, ItemStatus.Have);
        }
        if (useditemName != null && useditemName.Length > 0)
        {
            foreach (string itemName in useditemName)
            {
                UpdateItemStatusInDatabase(itemName, ItemStatus.Used);
            }
        }
    }


    private void UpdatePersonStatusInDatabase(string personName, bool newStatus)
    {
        DatabaseManager databaseManager = FindObjectOfType<DatabaseManager>();
        if (databaseManager != null)
        {
            databaseManager.UpdatePersonStatus(personName, newStatus);
        }
        else
        {
            Debug.LogWarning("DatabaseManager 없");
        }
    }

    private void UpdateQuestStatusInDatabase(string questName, QuestStatus newStatus)
    {
        DatabaseManager databaseManager = FindObjectOfType<DatabaseManager>();
        if (databaseManager != null)
        {
            databaseManager.UpdateQuestStatus(questName, newStatus);
        }
        else
        {
            Debug.LogWarning("DatabaseManager 없");
        }
    }

    private void UpdateItemStatusInDatabase(string itemName, ItemStatus newStatus)
    {
        DatabaseManager databaseManager = FindObjectOfType<DatabaseManager>();
        if (databaseManager != null)
        {
            databaseManager.UpdateItemStatus(itemName, newStatus);
        }
        else
        {
            Debug.LogWarning("DatabaseManager 없");
        }
    }
}
