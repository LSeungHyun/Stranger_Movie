using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cameratracker : MonoBehaviour
{
    static public Cameratracker instance;

    private void Awake()
    {
            SetCanvasRenderCamera();
    }

    private void SetCanvasRenderCamera()
    {
        Canvas canvas = GetComponent<Canvas>();
        if (canvas != null)
        {
            Camera mainCamera = Camera.main;
            if (mainCamera != null)
            {
                canvas.worldCamera = mainCamera;
            }
            else
            {
                Debug.LogError("Main Camera가 없습니다");
            }
        }
        else
        {
            Debug.LogError("Canvas가 없습니다.");
        }
    }
}
