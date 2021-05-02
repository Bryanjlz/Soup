using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadUnload : MonoBehaviour
{
    bool GachaOpen = false;
    public string sceneName;

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape) && GachaOpen) {
            Unload();
        }
    }

    public void Load(string sceneName)
    {
        GachaOpen = true;
        SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
    }
    public void Unload()
    {
        GachaOpen = false;
        SceneManager.UnloadSceneAsync(sceneName);
    }



}
