using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class InactiveTimer_Multi : MonoBehaviour
{
    public float inactivityLimit = 30f; // 비활성 시간 제한 (초)
    public float inactivityTimer = 0f;

    void Update()
    {
        if (Input.anyKeyDown) // 입력이 있을 때마다 타이머 초기화
        {
            inactivityTimer = 0f;
        }
        else
        {
            inactivityTimer += Time.deltaTime;
        }

        if (inactivityTimer >= inactivityLimit)
        {

            Debug.Log("오랜시간 활동하지않아 강퇴함");
            //PhotonNetwork.LeaveRoom(); // 비활성 시간이 초과되면 룸 나가기
            PhotonNetwork.Disconnect();
            SceneManager.LoadScene("Movie_Title");
        }
    }
}