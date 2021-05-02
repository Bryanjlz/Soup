using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadUnload : MonoBehaviour
{

    public string sceneName;

    public void Load(string sceneName)
    {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
    }
    public void Unload()
    {
        SceneManager.UnloadSceneAsync(sceneName);
    }


}
