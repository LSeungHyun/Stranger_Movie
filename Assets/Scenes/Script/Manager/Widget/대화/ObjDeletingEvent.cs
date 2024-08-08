using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjDeletingEvent : MonoBehaviour
{
    private DialogueManager theDM;
    public string personName;
    public string questName;
    public string itemName;
    public string[] useditemName;
    private Material originalMaterial;

    public Dialogue dialogue_1;
    public GameObject[] objectsToDisable;
    public GameObject[] objectsToEnable;

    [System.Serializable]
    public struct GameObjectCondition
    {
        public GameObject obj;
        public bool mustBePresent;
    }

    public GameObjectCondition[] gameObjectConditions;

    public Material outlineMaterial;
    public GameObject targetObject;

    public bool TouchObj = false;
    public bool OnlyDelete = false;

    [System.Serializable]
    public struct SpriteAnimation
    {
        public Sprite sprite;
        public RuntimeAnimatorController animatorController;
    }

    public SpriteAnimation[] spriteAnimations; 

    void Start()
    {
        theDM = FindObjectOfType<DialogueManager>();
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
        if (isColliding)
        {
            if (TouchObj)
            {
                if (CheckGameObjectConditions())
                {
                    if (OnlyDelete)
                    {
                        UpdateObjects();
                        if (questName != null)
                        {
                            UpdateQuestStatusInDatabase(questName, QuestStatus.Seen);
                        }
                    }
                    else
                    {
                        StartCoroutine(EventCoroutine());
                        UpdateObjects();
                    }
                }
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.F))
                {
                    if (CheckGameObjectConditions())
                    {
                        if (OnlyDelete)
                        {
                            UpdateObjects();
                        }
                        else
                        {
                            if (personName != null)
                            {
                                UpdatePersonStatusInDatabase(personName, true);
                            }
                            StartCoroutine(EventCoroutine());
                        }
                    }
                }
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
            Debug.LogWarning("DatabaseManager ¾ø");
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
            Debug.LogWarning("DatabaseManager ¾ø");
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
            Debug.LogWarning("DatabaseManager ¾ø");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("µé");
        isColliding = true;
        ApplyOutline(true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("³«");
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

    IEnumerator EventCoroutine()
    {
        theDM.ShowDialogue(dialogue_1);

        yield return new WaitUntil(() => !theDM.talking);

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
        UpdateObjects(); 
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
