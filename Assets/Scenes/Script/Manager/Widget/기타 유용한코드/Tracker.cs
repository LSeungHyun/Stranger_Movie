using System.Collections;
using UnityEngine;

public class Tracker : MonoBehaviour
{
    [SerializeField]
    public TextBox sentences;
    public GameObject[] objectsToDisable;
    public GameObject[] objectsToEnable;

    private TextBoxManager theTBM;
    private PlayerManager thePlayer;

    [System.Serializable]
    public struct GameObjectCondition
    {
        public GameObject obj;
        public bool mustBePresent;
    }

    public GameObjectCondition[] gameObjectConditions;

    private bool objectsUpdated = false;

    void Start()
    {
        theTBM = FindObjectOfType<TextBoxManager>();
        thePlayer = FindObjectOfType<PlayerManager>();
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
        objectsUpdated = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!CheckGameObjectConditions())
        {
            theTBM.ShowDialogue(sentences);
            StartCoroutine(CloseDialogueAfterDelay(2f));
        }
    }

    private IEnumerator CloseDialogueAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        theTBM.ExitDialogue();
    }

    void Update()
    {
        if (!objectsUpdated && CheckGameObjectConditions())
        {
            theTBM.ExitDialogue();
            UpdateObjects();
        }
    }
}
