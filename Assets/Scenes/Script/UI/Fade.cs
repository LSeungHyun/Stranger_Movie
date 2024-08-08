using System.Collections;
using UnityEngine;

public class FadeEffect : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
        else
        {
            Debug.LogError("SpriteRenderer 없");
        }
    }

    void OnEnable()
    {
        if (spriteRenderer != null)
        {
            StartCoroutine(FadeInOut());
        }
        else
        {
            Debug.LogError("SpriteRenderer 없");
        }
    }

    IEnumerator FadeInOut()
    {
        // 0.5초 동안 서서히 선명해지기
        float fadeInDuration = 0.5f;
        for (float t = 0; t < fadeInDuration; t += Time.deltaTime)
        {
            Color newColor = originalColor;
            newColor.a = t / fadeInDuration;
            spriteRenderer.color = newColor;
            yield return null;
        }
        spriteRenderer.color = originalColor;

        yield return new WaitForSeconds(1.0f);

        // 0.5초 동안 서서히 투명해지기 
        float fadeOutDuration = 0.5f;
        for (float t = 0; t < fadeOutDuration; t += Time.deltaTime)
        {
            Color newColor = originalColor;
            newColor.a = 1 - (t / fadeOutDuration);
            spriteRenderer.color = newColor;
            yield return null;
        }
        Color finalColor = originalColor;
        finalColor.a = 0;
        spriteRenderer.color = finalColor;

        gameObject.SetActive(false);
    }
}
