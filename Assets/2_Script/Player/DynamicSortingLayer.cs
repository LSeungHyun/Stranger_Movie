using UnityEngine;

public class DynamicSortingLayer : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private string currentSortingLayer;

    [Header("Sorting Layer Names")]
    public string underObjectLayer = "UnderObject";
    public string aboveObjectLayer = "AboveObject";

    [Header("Target Settings")]
    public PlayerManager target;

    public bool isColliding = false;

    #region Lifecycle Methods
    void Awake()
    {
        target = FindObjectOfType<PlayerManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentSortingLayer = spriteRenderer.sortingLayerName;
    }

    void Update()
    {
        if (isColliding)
        {
            UpdateSortingLayer();
        }
    }
    #endregion

    #region
    public void OnTriggerEnter2D(Collider2D collision)
    {
        isColliding = true;
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        isColliding = false;
    }
    #endregion

    #region Sorting Methods
    /// <summary>
    /// 오브젝트 위치에 따라 정렬 레이어를 업데이트
    /// </summary>
    private void UpdateSortingLayer()
    {
        if (target == null || spriteRenderer == null) return;

        string newSortingLayer = transform.position.y > target.transform.position.y ? underObjectLayer : aboveObjectLayer;

        if (newSortingLayer != currentSortingLayer)
        {
            currentSortingLayer = newSortingLayer;
            spriteRenderer.sortingLayerName = currentSortingLayer;
        }
    }
    #endregion
}