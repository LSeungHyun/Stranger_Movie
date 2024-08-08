using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Outliner : MonoBehaviour
{
    private Material originalMaterial;
    public Material outlineMaterial;
    public GameObject targetObject;
    void Start()
    {
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
    private void OnTriggerEnter2D(Collider2D collision)
    {
        ApplyOutline(true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        ApplyOutline(false);
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
