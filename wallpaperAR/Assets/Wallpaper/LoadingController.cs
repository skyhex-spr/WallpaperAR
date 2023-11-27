using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingController : MonoBehaviour
{
    void Start()
    {
        // Start the ChangeSceneCoroutine
        StartCoroutine(ChangeSceneCoroutine());
    }

    IEnumerator ChangeSceneCoroutine()
    {
        // Wait for 1 second
        yield return new WaitForSeconds(1f);

        // Load the desired scene by name
        SceneManager.LoadScene("Camera");
    }
}
