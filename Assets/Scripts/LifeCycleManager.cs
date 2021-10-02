using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Manage LifeCycle of each Level.
/// </summary>
public class LifeCycleManager : MonoBehaviour
{
    // Load the next scene in the build order
    public void WinLevel()
    {
        Debug.Log("Win");
        // TODO: Show UI, then have ui button trigger new scene load.
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
