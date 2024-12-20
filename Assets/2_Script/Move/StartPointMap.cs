using UnityEngine;
using UnityEngine.SceneManagement;

public class StartPointMap : MonoBehaviour
{
    private PlayerManager playerManager;

    public string fromPoint;

    private void Awake()
    {
        playerManager = FindObjectOfType<PlayerManager>();
    }

    void Start()
    {
        if (SceneManager.GetActiveScene().name == playerManager.currentMapName)
        {
            if (fromPoint == playerManager.lastMapName)
            {
                playerManager.transform.position = this.transform.position;
            }
        }
    }
}