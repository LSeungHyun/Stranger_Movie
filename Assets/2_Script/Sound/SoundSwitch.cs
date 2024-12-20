using System.Linq;
using UnityEngine;

public class SoundSwitch : MonoBehaviour
{
    public AudioManager audioManager;

    public WebGLBtn webglBtn;
    public GameObject confirmOn;

    public int audioNum;

    public bool isAwake = false;
    public bool isLoop = false;
    public bool isOnce = false;
    public bool isTouch = false;
    private bool isColliding = false;

    void Awake()
    {
        audioManager = FindObjectOfType<AudioManager>();
        webglBtn = Resources.FindObjectsOfTypeAll<WebGLBtn>().FirstOrDefault();
        confirmOn = Resources.FindObjectsOfTypeAll<GameObject>().FirstOrDefault(g => g.name == "ConfirmOn");
        if (isAwake)
        {
            PlayObjectSound();
        }
    }

    private void Update()
    {
        if (isColliding && !isAwake) //EventCollider에 닿아서 true가 돼었을때
        {
            if (isTouch) //Inspector창에서 true값으로 고정 / 스크립트에서 따로 조절하는 부분이 없음.
            {
                PlayObjectSound();
            }
            else
            {
                confirmOn.SetActive(true);
                bool isButtonClicked = Input.GetKeyDown(KeyCode.F) || (webglBtn?.isClick ?? false);
                if (isButtonClicked)
                {
                    PlayObjectSound();
                }
            }
        }
    }

    void PlayObjectSound()
    {
        audioManager.EffectSoundPlay(audioNum);
        if (isOnce)
        {
            this.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        isColliding = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        isColliding = false;
        if(confirmOn != null)
        {
            confirmOn.SetActive(false);
        }
    }
}