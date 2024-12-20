using Photon.Pun;
using System;
using System.Collections;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class WindowButton_Multi : MonoBehaviour
{
    public GameObject[] globalActivateOnEnable;
    public GameObject[] globalDeactivateOnDisable;

    public GameObject dialogueManager;
    public GameObject questTextManager;

    public PlayerManager_Multi thePlayer;
    //public AudioManager theAudio;
    public Animator anim;
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
        StartCoroutine(FindPlayerCoroutine());
    }

    public IEnumerator FindPlayerCoroutine()
    {
        while (thePlayer == null)
        {
            // 모든 PlayerManager 객체를 찾음
            PlayerManager_Multi[] players = FindObjectsOfType<PlayerManager_Multi>();

            // 로컬 플레이어를 찾음 (PhotonView.IsMine이 true인 플레이어)
            foreach (PlayerManager_Multi player in players)
            {
                PhotonView playerPV = player.GetComponent<PhotonView>();
                if (playerPV != null && playerPV.IsMine) // 나 자신의 플레이어인지 확인
                {
                    thePlayer = player;
                    break;
                }
            }

            yield return null; // 다음 프레임까지 대기
        }
    }
    private void Update()
    {
        HandleKeyCode(KeyCode.F8, 0);
        HandleKeyCode(KeyCode.F9, 1);

        //if (Input.GetKeyDown(KeyCode.F8))// && !isUp)
        //{
        //    ToggleWindow(0);
        //}
        //if (Input.GetKeyDown(KeyCode.F9))
        //{
        //    ToggleWindow(1);
        //}
        //인벤토리 창에서만 적용하는 스크립트라면 인벤토리를 열었을때만 작동되도록 조건 추가해야함
        //팝업창 관련 스크립트4개를 통일한 뒤에 tallking bool값으로 조절하면 될듯
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (anim != null)
            {
                anim.SetTrigger("Phone_Close");
            }

            DeactivateAllWindows();
            DeinitializeWindows();
        }
    }
    private void DeactivateAllWindows()
    {
        foreach (var settings in windowsSettings)
        {
            if (settings.window != null)
            {
                thePlayer.canMove = true;
                Debug.Log("가방인벤토리 끄기");
                settings.window.SetActive(false);
            }
        }
    }
    /// <summary>
    /// KeyCode와 index값을 받아 ToggleWindow 호출하는 메서드
    /// </summary>
    /// <param name="key"></param>
    /// <param name="windowIndex"></param>
    public void HandleKeyCode(KeyCode key,int windowIndex)
    {
        if (Input.GetKeyDown(key))
        {
            ToggleWindow(windowIndex);
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


    public void OnWindow()
    {
        foreach (var settings in windowsSettings)
        {
            if (settings.window != null)
            {
                settings.window.SetActive(true);
            }
        }
    }

    public void OffWindow()
    {
        foreach (var settings in windowsSettings)
        {
            if (settings.window != null)
            {
                settings.window.SetActive(false);
            }
        }
    }

    public void ToggleWindow(int index)
    {
        //isUp = !isUp;
        if (index >= 0 && index < windowsSettings.Length && windowsSettings[index].window != null)
        {
            bool isActive = windowsSettings[index].window.activeSelf;

            if (this.gameObject.tag == "CloseBtn")// || (index == 0 && !isUp))
            {
                Debug.Log("꾸물꾸물서윤성");
                //버튼을 누를때 fasle로 초기화
                //F8_False();
                anim.SetTrigger("Phone_Close");
                //anim.SetTrigger("Bag_Close");
                //버튼 누르고 0.5초뒤에 꺼지도록 딜레이주는 조건
                Invoke("DeactivateAllWindows", 0.4f);
                if (thePlayer.canMove)
                {
                    windowsSettings[index].window.SetActive(!isActive);
                }
            }

            else
            {
                DeactivateAllWindows();
                windowsSettings[index].window.SetActive(!isActive);
            }


            if (isActive)
            {
                thePlayer.canMove = true;
            }
            else
            {
                thePlayer.canMove = false;
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
            Image Image = changeWindow[index].window.GetComponent<Image>();
            if (Image != null)
            {
                Debug.Log("디테일 이미지 출력");
                Image.sprite = changeWindow[index].detailImage;
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
