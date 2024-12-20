using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FadeLogo : MonoBehaviour
{
    public Image ledaLogo;
    public string transferMapName;

    float time = 0f;
    float F_time = 1f;

    void Start()
    {
        StartCoroutine(LedaOn());
    }

    IEnumerator LedaOn()
    {
        ledaLogo.gameObject.SetActive(true);

        Color alpha = ledaLogo.color;

        while (alpha.a < 5f)
        {
            time += Time.deltaTime / F_time;
            alpha.a = Mathf.Lerp(0, 5, time);
            ledaLogo.color = alpha;
            yield return null;
        }

        time = 0f;

        yield return new WaitForSeconds(time);

        while (alpha.a > 0f)
        {
            time += Time.deltaTime / F_time;
            alpha.a = Mathf.Lerp(3, 0, time);
            ledaLogo.color = alpha;
            yield return null;
        }

        ledaLogo.gameObject.SetActive(false);

        yield return new WaitForSeconds(0.3f);

        SceneManager.LoadScene(transferMapName);
    }
}
