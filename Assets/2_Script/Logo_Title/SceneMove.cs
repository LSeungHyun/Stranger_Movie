using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMove : MonoBehaviour
{
    public string transferMapName;

    public void CallSceneMove()
    {
        SceneManager.LoadScene(transferMapName);
    }
}
