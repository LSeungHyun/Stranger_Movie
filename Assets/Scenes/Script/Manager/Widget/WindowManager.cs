using UnityEngine;

public class WindowManager : MonoBehaviour
{
    public GameObject DialogueManager;
    public GameObject QuestTextManager;
    public GameObject TextManager;
    public GameObject Phone;
    public GameObject Bag;
    public GameObject Button;

    private void DeactivateAllWindows()
    {
        DialogueManager.SetActive(false);
        QuestTextManager.SetActive(false);
        TextManager.SetActive(false);
        Phone.SetActive(false);
        Bag.SetActive(false);
        Button.SetActive(false);
    }

    public void OpenWindow(GameObject windowToOpen)
    {
        DeactivateAllWindows();
        windowToOpen.SetActive(true);
    }

    public void CloseWindow()
    {
        Button.SetActive(true); 
        DialogueManager.SetActive(false);
        QuestTextManager.SetActive(false);
        TextManager.SetActive(false);
        Phone.SetActive(false);
        Bag.SetActive(false);
    }
}
