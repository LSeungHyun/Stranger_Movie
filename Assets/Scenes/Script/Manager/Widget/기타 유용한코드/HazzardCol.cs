using System.Collections;
using TMPro;
using UnityEngine;

public class HazzardCol : MonoBehaviour
{
    public Dialogue dialogue_1;
    public GameObject[] objectsToDisable; // 비활성화할 오브젝트들
    public GameObject[] objectsToEnable; // 활성화할 오브젝트들

    private DialogueManager theDM;
    private PlayerManager thePlayer;
    public CameraController theCamera;
    public GameObject targetPosition1;
    public BoxCollider2D targetBound1;// 플레이어를 옮길 목표 위치
    public GameObject targetPosition2;
    public BoxCollider2D targetBound2;


    void Start()
    {
        theDM = FindObjectOfType<DialogueManager>();
        thePlayer = FindObjectOfType<PlayerManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

            StartCoroutine(EventCoroutine());
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
        // 플레이어를 목표 위치로 옮기기
        thePlayer.transform.position = targetPosition1.transform.position;
        theCamera = FindObjectOfType<CameraController>();
        theCamera.Teleport();
        theCamera.SetBound(targetBound1);

        // 대화 시작
        theDM.ShowDialogue(dialogue_1);

        yield return new WaitUntil(() => !theDM.talking);

        // 대화 끝
        thePlayer.transform.position = targetPosition2.transform.position;
        theCamera.Teleport();
        theCamera.SetBound(targetBound2);
        UpdateObjects();
    }
}
