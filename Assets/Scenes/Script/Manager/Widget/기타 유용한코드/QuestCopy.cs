using UnityEngine;
using System.Reflection;
using System;

public class QuestCopy : MonoBehaviour
{
    void Start()
    {
        GameObject questSprite = GameObject.Find("QuestSprite");

        if (questSprite != null)
        {
            CopyObject(questSprite, gameObject);
        }
        else
        {
            Debug.LogError("QuestSprite 오브젝트를 찾을 수 없습니다.");
        }
    }

    void CopyObject(GameObject source, GameObject destination)
    {
        Component[] components = source.GetComponents<Component>();

        foreach (Component component in components)
        {
            if (component is Transform)
                continue;

            Component copiedComponent = destination.AddComponent(component.GetType());

            CopyFields(component, copiedComponent);
            CopyProperties(component, copiedComponent);
        }

        foreach (Transform child in source.transform)
        {
            GameObject newChild = new GameObject(child.name);
            newChild.transform.SetParent(destination.transform);

            newChild.transform.localPosition = child.localPosition;
            newChild.transform.localRotation = child.localRotation;
            newChild.transform.localScale = child.localScale;

            CopyObject(child.gameObject, newChild);
        }
    }

    void CopyFields(Component source, Component destination)
    {
        FieldInfo[] fields = source.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        foreach (FieldInfo field in fields)
        {
            if (field.IsStatic)
                continue;

            if (field.IsPublic || field.IsDefined(typeof(SerializeField), true))
            {
                object value = field.GetValue(source);
                field.SetValue(destination, value);
            }
        }
    }

    void CopyProperties(Component source, Component destination)
    {
        PropertyInfo[] properties = source.GetType().GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        foreach (PropertyInfo property in properties)
        {
            if (property.GetMethod.IsStatic)
                continue;

            if (property.CanWrite && property.GetIndexParameters().Length == 0)
            {
                try
                {
                    object value = property.GetValue(source);
                    if (value != null)
                    {
                        property.SetValue(destination, value);
                    }
                }
                catch (TargetException ex)
                {
                    Debug.LogWarning("Failed" );
                }
                catch (Exception ex)
                {
                    Debug.LogWarning("error");
                }
            }
        }
    }
}
