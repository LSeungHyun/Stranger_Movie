using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetrics;

public class ObjectSpinner : MonoBehaviour
{
    private SpinnerManager theSM;
    private Material originalMaterial;

    [SerializeField]
    public Dialogue dialogue;

    public Material outlineMaterial; 
    public GameObject targetObject; 

    public bool TouchObj = false;

    public GameObject[] objectsToDisable;
    public GameObject[] objectsToEnable; 

    void Start()
    {
        theSM = FindObjectOfType<SpinnerManager>();
        if (outlineMaterial == null)
        {
            outlineMaterial = Resources.Load<Material>("Outline");
        }
        if (targetObject == null)
        {
            string targetObjectName = name + "Obj";
            targetObject = GameObject.Find(targetObjectName);
        }
    }
    private bool isColliding = false;
    private void Update()
    {
        if (isColliding)
        {
            if (TouchObj)
            {
                StartCoroutine(EventCoroutine());
                gameObject.SetActive(false);
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.F))
                {
                    StartCoroutine(EventCoroutine());
                }
            }

        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("µé");
        isColliding = true;
        ApplyOutline(true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("³«");
        isColliding = false;
        ApplyOutline(false);
    }

    IEnumerator EventCoroutine()
    {
        theSM.ShowDialogue(dialogue);

        yield return new WaitUntil(() => !theSM.talking);

        UpdateObjects();
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

    private void ApplyOutline(bool apply)
    {
        if (targetObject != null)
        {
            Renderer rend = targetObject.GetComponent<Renderer>();
            if (rend != null)
            {
                if (apply)
                {
                    if (originalMaterial == null)
                    {
                        originalMaterial = rend.material;
                    }
                    rend.material = outlineMaterial;
                }
                else
                {
                    if (originalMaterial != null)
                    {
                        rend.material = originalMaterial;
                    }
                }
            }
        }

    }
}
