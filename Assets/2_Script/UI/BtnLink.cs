using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class BtnLink : MonoBehaviour
{
    private DialManager dialManager;
    public Button btnLink;

    public string btnName;
    public string linkText;
    public int linkIndex = 1;

    private bool isColliding = false;
    private bool isActive = false;

    private void Awake()
    {
        dialManager = FindObjectOfType<DialManager>();
        btnLink = Resources.FindObjectsOfTypeAll<Button>().FirstOrDefault(g => g.name == btnName);
    }

    private void Update()
    {
        if (isColliding && dialManager.isTalking)
        {
            if (linkIndex == dialManager.currentIndex + 1)
            {
                CallButton();
            }
            else
            {
                ClearButton();
            }
        }
        if (!dialManager.isTalking)
        {
            ClearButton();
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        isColliding = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        isColliding = false;
    }

    public void CallButton()
    {
        if (!isActive)
        {
            isActive = true;
            btnLink.gameObject.SetActive(true);
            btnLink.onClick.AddListener(CallLink);
        }
    }

    public void ClearButton()
    {
        isActive = false;
        btnLink.onClick.RemoveListener(CallLink);
        btnLink.gameObject.SetActive(false);
    }

    public void CallLink()
    {
        Application.OpenURL(linkText);
    }
}