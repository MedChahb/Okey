using UnityEngine;
using UnityEngine.SceneManagement;

public class ParamBtn : MonoBehaviour
{
    private void OnMouseDown()
    {
        SceneManager.LoadScene(0);
    }
}
