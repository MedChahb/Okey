using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneChanger : MonoBehaviour
{
    // public string sceneName = "Formulaire de configuration";

    public void ChangeScene(int SceneId)
    {
        SceneManager.LoadScene(SceneId);
        Debug.Log("Scene changed to " + SceneId);
    }
}
