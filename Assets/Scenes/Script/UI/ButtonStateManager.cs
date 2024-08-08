using UnityEngine;
using UnityEngine.UI;

public class ButtonStateManager : MonoBehaviour
{
    public string personName; 
    public Sprite activeSprite;
    public Sprite inactiveSprite; 

    public Image buttonImage;

    private void Start()
    {
        buttonImage = GetComponent<Image>();
        UpdateButtonState();
    }

    private void OnEnable()
    {
        buttonImage = GetComponent<Image>();
        UpdateButtonState();
    }

    private void UpdateButtonState()
    {
        DatabaseManager databaseManager = DatabaseManager.instance;

        PersonInfo personInfo = databaseManager.personInfos.Find(p => p.name == personName);

        if (personInfo != null && personInfo.isActive)
        {
            buttonImage.sprite = activeSprite;
            GetComponent<Button>().interactable = true;
        }
        else
        {
            buttonImage.sprite = inactiveSprite;
            GetComponent<Button>().interactable = false;
        }
    }
}
