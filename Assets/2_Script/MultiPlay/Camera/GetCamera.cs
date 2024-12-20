using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

// 멀티에서 인게임으로 들어갔을 때 카메라 찾아오는 스크립트
public class GetCamera : MonoBehaviour 
{
    
    public Canvas canvas;
    public Camera renderCamera;

    public bool isCam;
    void Start()
    {
        // Render Camera에 설정된 카메라 호출
        renderCamera = canvas.worldCamera;
    }
        // Update is called once per frame
        void Update()
    {
        if (!isCam && SceneManager.GetActiveScene().name == "Movie_Start")
        {
            renderCamera = Camera.main;
            canvas.worldCamera = renderCamera;
            isCam = true;
        }
    }
}
