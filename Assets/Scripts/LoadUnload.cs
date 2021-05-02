using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadUnload : MonoBehaviour
{
    bool GachaOpen = false;
    public GameObject thePanel;

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape) && GachaOpen) {
            Unload();
        }
    }

    public void Load(string sceneName)
    {
        GachaOpen = true;
        thePanel.SetActive(true);
    }
    public void Unload()
    {
        GachaOpen = false;
        thePanel.SetActive(false);
    }



}
