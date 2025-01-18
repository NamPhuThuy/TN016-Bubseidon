using System.Collections;
using NamPhuThuy;
using UnityEngine;
using UnityEngine.SceneManagement; // Import SceneManager

public class SceneReloader : Singleton<SceneReloader>
{
    public float bufferTime = 2f; // Delay in seconds before reloading the scene

    public void ReloadSceneWithDelay()
    {
        StartCoroutine(ReloadSceneCoroutine()); // Start the coroutine
    }

    private IEnumerator ReloadSceneCoroutine()
    {
        // Optionally, perform some action before reloading
        Debug.Log("Reloading scene after buffer...");
        
        // Wait for the specified buffer time
        yield return new WaitForSeconds(bufferTime);

        // Reload the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}