using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneOnAnyKey : MonoBehaviour
{
    [Tooltip("Name of the scene to load")]
    public string sceneName;

    void Update()
    {
        if (Input.anyKeyDown)
        {
            if (!string.IsNullOrEmpty(sceneName))
            {
                SceneManager.LoadScene(sceneName);
            }
            else
            {
                Debug.LogWarning("Scene name is empty! Set it in the inspector.");
            }
        }
    }
}
