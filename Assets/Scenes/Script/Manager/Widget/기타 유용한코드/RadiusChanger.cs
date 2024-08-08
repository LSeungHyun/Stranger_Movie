using System.Collections;
using UnityEngine;

public class RadiusChanger : MonoBehaviour
{
    public FadeManager fadeManager;
    public string playerTag = "Player";
    public float targetRadius = 1.0f;
    public float fadeSpeed = 0.5f;

    private void Start()
    {
        if (fadeManager == null)
        {
            fadeManager = FindObjectOfType<FadeManager>();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            StartCoroutine(IncreaseRadius());
        }
    }

    private IEnumerator IncreaseRadius()
    {
        while (fadeManager.radius < targetRadius)
        {
            fadeManager.SetRadius(Mathf.Min(fadeManager.radius + fadeSpeed * Time.deltaTime, targetRadius));
            yield return null;
        }
    }
}
