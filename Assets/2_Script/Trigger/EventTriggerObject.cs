using System.Linq;
using UnityEngine;

public class EventTriggerObject : MonoBehaviour
{
    //오브젝트 캐싱
    public GameObject ConfirmOn;
    public WebGLBtn webglBtn;

    //활성화, 비활성화할 오브젝트들
    public GameObject[] objectsToDisable;
    public GameObject[] objectsToEnable;

    //대화창 잔류 시간 나타내는 float값
    public float editCount = 0f;

    //인스펙터로 지정해주는 불값
    public bool isTouch = false;

    //게임 중 변하는 불값
    private bool isColliding = false;
    private void Awake()
    {
        ConfirmOn = Resources.FindObjectsOfTypeAll<GameObject>().FirstOrDefault(g => g.name == "ConfirmOn");
        webglBtn = Resources.FindObjectsOfTypeAll<WebGLBtn>().FirstOrDefault();
    }
    private void Update()
    {
        if (isColliding)
        {
            if (isTouch)
            {
                Invoke("UpdateObjects", editCount);
            }
            else
            {
                ConfirmOn.SetActive(true);
                bool isButtonClicked = Input.GetKeyDown(KeyCode.F) || (webglBtn?.isClick ?? false);
                if (isButtonClicked)
                {
                    Invoke("UpdateObjects", editCount);
                }
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        isColliding = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        isColliding = false;
        ConfirmOn.SetActive(false);
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
}