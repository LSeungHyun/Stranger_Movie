using UnityEngine;

public class ToggleMultipleWindows : MonoBehaviour
{
    public GameObject[] globalActivateOnEnable; 
    public GameObject[] globalDeactivateOnDisable; 

    public GameObject dialogueManager;
    public GameObject questTextManager;
    private PlayerManager thePlayer;

    [System.Serializable]
    public class WindowSettings
    {
        public GameObject window;
    }
    [System.Serializable]
    public class WindowChange
    {
        public GameObject window;
        public Sprite detailImage; 
    }
    public WindowSettings[] windowsSettings;
    public WindowChange[] changeWindow;


    void Start()
    {
        thePlayer = FindObjectOfType<PlayerManager>();
    }
        private void DeactivateAllWindows()
    {
        foreach (var settings in windowsSettings)
        {
            if (settings.window != null)
            {
                settings.window.SetActive(false);
            }
        }
    }

    private void InitializeWindows()
    {
        foreach (var window in globalActivateOnEnable)
        {
            if (window != null)
            {
                if (!dialogueManager.activeSelf && !questTextManager.activeSelf)
                {

                    window.SetActive(true);
                }
            }
        }
    }

    private void DeinitializeWindows()
    {
        foreach (var window in globalDeactivateOnDisable)
        {
            if (window != null)
            {
                window.SetActive(false);
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F8))
        {
            ToggleWindow(0);
        }
        if (Input.GetKeyDown(KeyCode.F9))
        {
            ToggleWindow(1);
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            DeactivateAllWindows();
            DeinitializeWindows();
        }
    }

    public void ToggleWindow(int index)
    {

        if (index >= 0 && index < windowsSettings.Length && windowsSettings[index].window != null)
        {
            bool isActive = windowsSettings[index].window.activeSelf;
            DeactivateAllWindows();
            windowsSettings[index].window.SetActive(!isActive);
            if(isActive)
            {
                thePlayer.notMove = false;
            }
            else
            {

                thePlayer.notMove = true;
            }
            InitializeWindows();
            DeinitializeWindows();
        }
        else
        {
            Debug.LogWarning("창없음");
        }
    }

    public void ChangeWindow(int index)
    {
        if (index >= 0 && index < changeWindow.Length && changeWindow[index].window != null)
        {
            SpriteRenderer renderer = changeWindow[index].window.GetComponent<SpriteRenderer>();
            if (renderer != null)
            {
                renderer.sprite = changeWindow[index].detailImage;
            }
            else
            {
                Debug.LogWarning("SpriteRenderer없");
            }
        }
        else
        {
            Debug.LogWarning("창없음");
        }
    }
}
