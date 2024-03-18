using UnityEngine;
using UnityEngine.SceneManagement;

public class RetourBtn : MonoBehaviour
{
    // Méthode appelée lorsqu'un clic est détecté sur le Collider de l'objet
    private void OnMouseDown()
    {
        SceneManager.LoadScene(1);
    }
}
