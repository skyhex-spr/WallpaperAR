using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingSceneController : MonoBehaviour
{
    public Slider loadingSlider;
    public float transitionDuration = 5f;

    private void Start()
    {
        // Start the scene transition process
        StartCoroutine(LoadNextScene());
    }

    private IEnumerator LoadNextScene()
    {
        yield return new WaitForSeconds(1f); // Optional delay to make sure the scene is initialized

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Camera");
        asyncLoad.allowSceneActivation = false;

        float timer = 0f;

        while (timer < transitionDuration)
        {
            loadingSlider.value = timer / transitionDuration;
            timer += Time.deltaTime;
            yield return null;
        }

        loadingSlider.value = 1f;

        // Allow the scene to be activated
        asyncLoad.allowSceneActivation = true;
    }
}
