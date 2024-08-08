using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StepQuestTest : MonoBehaviour
{
    [SerializeField]
    public TextQuest textquest;
    public string questName;
    public string itemName;
    public string[] useditemName;

    private QuestTextManager theQTM;
    private Material originalMaterial;

    private GameObject isDone;

    [SerializeField]
    private List<GameObject> objdisabled;
    [SerializeField]
    private List<GameObject> objenabled;


    public Material outlineMaterial;
    public GameObject targetObject; 

    public bool TouchObj = false;

    void Start()
    {
        theQTM = FindObjectOfType<QuestTextManager>();

        isDone = Resources.FindObjectsOfTypeAll<GameObject>().FirstOrDefault(g => g.name == "TextQuestDone");

        if (isDone == null)
        {
            Debug.LogError("isDone 없.");
        }
        if (outlineMaterial == null)
        {
            outlineMaterial = Resources.Load<Material>("Outline");
        }
        if (targetObject == null)
        {
            string targetObjectName = name + "Obj";
            targetObject = GameObject.Find(targetObjectName);
        }
    }


    private bool isColliding = false;
    private void Update()
    {
        if (isDone != null && !isDone.activeInHierarchy)
        {
            //방금만든거
            if (isColliding)
            {
                if (TouchObj)
                {
                    UpdateQuestStatusInDatabase(questName, QuestStatus.Seen);
                    theQTM.ShowDialogue(textquest);
                }
                else
                {
                    if (Input.GetKeyDown(KeyCode.F))
                    {
                        UpdateQuestStatusInDatabase(questName, QuestStatus.Seen);
                        theQTM.ShowDialogue(textquest);
                    }
                }
            }
        }
        else
        {
            if (itemName != null)
            {
                UpdateItemStatusInDatabase(itemName, ItemStatus.Have);
            }
            if (useditemName != null && useditemName.Length > 0) // 배열이 null이 아니고 비어있지 않은 경우에만 실행
            {
                foreach (string itemName in useditemName)
                {
                    UpdateItemStatusInDatabase(itemName, ItemStatus.Used);
                }
            }
            gameObject.SetActive(false);

            foreach (GameObject objB in objdisabled)
            {
                if (objB != null)
                {
                    objB.SetActive(false);
                }
            }

            foreach (GameObject objC in objenabled)
            {
                if (objC != null)
                {
                    objC.SetActive(true);
                }
            }
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
            Debug.LogWarning("DatabaseManager not found in scene.");
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
            Debug.LogWarning("DatabaseManager not found in scene.");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("들");
        isColliding = true;
        ApplyOutline(true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("낙");
        isColliding = false;
        ApplyOutline(false);
    }

    private void ApplyOutline(bool apply)
    {
        if (targetObject != null)
        {
            Renderer rend = targetObject.GetComponent<Renderer>();
            if (rend != null)
            {
                if (apply)
                {
                    if (originalMaterial == null)
                    {
                        originalMaterial = rend.material;
                    }
                    rend.material = outlineMaterial;
                }
                else
                {
                    if (originalMaterial != null)
                    {
                        rend.material = originalMaterial;
                    }
                }
            }
        }

    }
}
