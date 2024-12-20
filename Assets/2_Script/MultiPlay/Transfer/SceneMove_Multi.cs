using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMove_Multi : MonoBehaviour
{
    public string transferMapName;

    public void StartGameBtn()
    {
        PhotonNetwork.LoadLevel(transferMapName);
    }
}
