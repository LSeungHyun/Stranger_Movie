using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class QuestEvent : MonoBehaviour
{
    public string questName;
    public string itemName;
    public string[] useditemName;
    public QuestData dialogue;
    public bool triggerOnCollision; 
    private Material originalMaterial;

    private QuestManager theQM;

    private GameObject isDone;

    [SerializeField]
    private List<GameObject> objdisabled;
    [SerializeField]
    private List<GameObject> objenabled;


    [System.Serializable]
    public struct GameObjectCondition
    {
        public GameObject obj;
        public bool mustBePresent;
    }

    public GameObjectCondition[] gameObjectConditions;

    public Material outlineMaterial; 
    public GameObject targetObject;

    void Start()
    {
        theQM = FindObjectOfType<QuestManager>();
        isDone = Resources.FindObjectsOfTypeAll<GameObject>().FirstOrDefault(g => g.name == "DialQuestDone");
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
        if(isDone != null && !isDone.activeInHierarchy){
            //방금만든거
            if (isColliding && Input.GetKeyDown(KeyCode.F))
            {
                if (CheckGameObjectConditions())
                {
                    UpdateQuestStatusInDatabase(questName, QuestStatus.Seen);
                    theQM.ShowDialogue(dialogue);
                }
            }
        }
        else
        {
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        isColliding = true;
        ApplyOutline(true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        isColliding = false;
        ApplyOutline(false);
    }

    private bool CheckGameObjectConditions()
    {
        foreach (GameObjectCondition condition in gameObjectConditions)
        {
            if (condition.mustBePresent)
            {
                if (condition.obj == null || !condition.obj.activeInHierarchy)
                {
                    return false;
                }
            }
            else
            {
                if (condition.obj != null && condition.obj.activeInHierarchy)
                {
                    return false;
                }
            }
        }
        return true;
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
