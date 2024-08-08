using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ObjectMover : MonoBehaviour
{
    public Vector3 startPosition;
    public Vector3 endPosition;
    public float moveDuration = 5f; 
    public float waitDuration = 0.5f;

    public Vector3 lastPosition;

    public GameObject textpanel;
    public Text textComponent;

    private PlayerManager thePlayer;


    private void Start()
    {
        thePlayer = FindObjectOfType<PlayerManager>();

        transform.position = startPosition;
        StartCoroutine(MoveObject());

        textpanel = Resources.FindObjectsOfTypeAll<GameObject>().FirstOrDefault(g => g.name == "TextPanel");
        textComponent = Resources.FindObjectsOfTypeAll<Text>().FirstOrDefault(g => g.name == "TextComponent");

        textpanel.SetActive(false);
        textComponent.gameObject.SetActive(false);

    }

    IEnumerator MoveObject()
    {
        while (true)
        {
            yield return StartCoroutine(MoveFromTo(startPosition, endPosition, moveDuration));
            yield return new WaitForSeconds(waitDuration);
            transform.position = startPosition;
            yield return new WaitForSeconds(waitDuration);
        }
    }

    IEnumerator MoveFromTo(Vector3 from, Vector3 to, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            transform.position = Vector3.Lerp(from, to, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = to;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            thePlayer.canMove = false;
            collision.transform.position = lastPosition;
            StartCoroutine(ShowPanelForDuration(2f));
            thePlayer.canMove = true;
        }
    }

    IEnumerator ShowPanelForDuration(float duration)
    {
        if (textpanel != null)
        {
            textpanel.SetActive(true);
            textComponent.gameObject.SetActive(true);

            if (textComponent != null)
            {
                textComponent.text = "車にぶつかるところだった。。！\n 気をつけて渡ろう。";
            }

            yield return new WaitForSeconds(duration);

            textpanel.SetActive(false);
            textComponent.gameObject.SetActive(false);
        }
    }
}
