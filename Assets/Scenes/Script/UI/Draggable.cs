using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    private Canvas canvas;
    private CanvasGroup canvasGroup;
    private Image image;
    private bool isDraggable = true;
    private float dragSpeed = 1.5f;

    public GameObject obj1; 
    public GameObject obj2;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();
        image = GetComponent<Image>();

    }

    private void OnEnable()
    {
        ResetPosition();
    }

    private void ResetPosition()
    {
        if (rectTransform != null)
        {
            rectTransform.anchoredPosition = Vector2.zero;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        bool isValid = IsRaycastLocationValid(eventData);

        if (isValid)
        {
            GameObject selectedObject = eventData.pointerCurrentRaycast.gameObject;

            if (selectedObject == obj1)
            {
                Debug.Log("1");
                // obj1을 위로 올리고, obj2는 아래로 
                obj1.transform.SetAsLastSibling();
                obj2.transform.SetAsFirstSibling();
            }
            else if (selectedObject == obj2)
            {
                Debug.Log("2");
                // obj2을 위로 올리고, obj1은 아래로
                obj2.transform.SetAsLastSibling();
                obj1.transform.SetAsFirstSibling();
            }
            else
            {
                Debug.Log("3");
            }

            if (canvasGroup != null)
            {
                canvasGroup.alpha = 0.8f;
                isDraggable = true;
            }
        }
        else
        {
            GameObject selectedObject = eventData.pointerCurrentRaycast.gameObject;

            if (selectedObject == obj2)
            {
                Debug.Log("1");
                // obj1을 위로 올리고, obj2는 아래로
                obj1.transform.SetAsLastSibling();
                obj2.transform.SetAsFirstSibling();
            }
            else if (selectedObject == obj1)
            {
                Debug.Log("2");
                // obj2을 위로 올리고, obj1은 아래로
                obj2.transform.SetAsLastSibling();
                obj1.transform.SetAsFirstSibling();
            }
            else
            {
                Debug.Log("3");
            }

            isDraggable = false;
        }
    }



    public void OnDrag(PointerEventData eventData)
    {
        if (rectTransform != null && isDraggable)
        {
            rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor * dragSpeed;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 1.0f;
        }
    }

    private bool IsRaycastLocationValid(PointerEventData eventData)
    {
        Vector2 localCursor;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, eventData.position, eventData.pressEventCamera, out localCursor);

        Rect rect = rectTransform.rect;
        if (!rect.Contains(localCursor))
            return false;

        Sprite sprite = image.sprite;
        Texture2D tex = sprite.texture;

        if (!tex.isReadable)
        {
            Debug.LogError("Texture 오류");
            return false;
        }

        Vector2 pivot = sprite.pivot;
        Vector2 spriteSize = sprite.rect.size;
        Vector2 uv = (localCursor + pivot) / spriteSize;

        uv.x *= sprite.rect.width;
        uv.y *= sprite.rect.height;

        Color color = tex.GetPixel((int)uv.x, (int)uv.y);

        return color.a > 0.1f;
    }
}
