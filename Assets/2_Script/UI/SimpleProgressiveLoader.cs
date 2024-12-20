using UnityEngine;
using System.Collections;

public class ProgressiveLoader : MonoBehaviour
{
    [SerializeField] private GameObject loadingUI;
    //[SerializeField] private UnityEngine.UI.Slider progressBar;

    private void Start()
    {
        StartCoroutine(LoadGame());
    }

    private IEnumerator LoadGame()
    {
        loadingUI.SetActive(true);

        AsyncOperation asyncLoad = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("f3");
        asyncLoad.allowSceneActivation = false;

        while (!asyncLoad.isDone)
        {
            //progressBar.value = asyncLoad.progress;

            if (asyncLoad.progress >= 0.9f)
            {
                //progressBar.value = 1f;
                asyncLoad.allowSceneActivation = true;
            }

            yield return null;
        }

        loadingUI.SetActive(false);
    }
}