using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PageProfile : MonoBehaviour
{

    [SerializeField]
    public GameObject Panel;

    //Modifier pour avoir l'avatar de l'utilisateur dans cette variable
    private int currentAvatarId = 0;

    [SerializeField]
    private GameObject avatar0;

    [SerializeField]
    private GameObject avatar1;

    [SerializeField]
    private GameObject avatar2;

    [SerializeField]
    private GameObject avatar3;

    private float scaleFactor = 1.3f;

    [SerializeField]
    private Button backButton;

    [SerializeField]
    private Button disconnectionButton;

    [SerializeField]
    private GameObject PanelAvatar;

    private JoueurManager manager;

    // Start is called before the first frame update
    void Start()
    {
        this.manager = JoueurManager.Instance;
        manager.SelfJoueurChangeEvent.AddListener(updateUserProfile);
        disconnectionButton.onClick.AddListener(onDisconnectionClicked);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

            if (hit.collider != null)
            {
                if (hit.collider.gameObject == avatar0.gameObject)
                {
                    OnAvatarButtonClick(0);
                }
                else if (hit.collider.gameObject == avatar1.gameObject)
                {
                    OnAvatarButtonClick(1);
                }
                else if (hit.collider.gameObject == avatar2.gameObject)
                {
                    OnAvatarButtonClick(2);
                }
                else if (hit.collider.gameObject == avatar3.gameObject)
                {
                    OnAvatarButtonClick(3);
                }
            }
        }
    }

    void OnAvatarButtonClick(int avatarId)
    {
        ResetAvatarView();
        currentAvatarId = avatarId;
        switch (currentAvatarId)
        {
            case 0:
                avatar0.transform.localScale *= scaleFactor;
                //Update avatar
                break;
            case 1:
                avatar1.transform.localScale *= scaleFactor;
                //Update avatar
                break;
            case 2:
                avatar2.transform.localScale *= scaleFactor;
                //Update avatar
                break;
            case 3:
                avatar3.transform.localScale *= scaleFactor;
                //Update avatar
                break;
        }
    }

    void ResetAvatarView()
    {
        switch (currentAvatarId)
        {
            case 0:
                avatar0.transform.localScale /= scaleFactor;
                break;
            case 1:
                avatar1.transform.localScale /= scaleFactor;
                break;
            case 2:
                avatar2.transform.localScale /= scaleFactor;
                break;
            case 3:
                avatar3.transform.localScale /= scaleFactor;
                break;
        }
    }

    // Méthode pour charger la scène précédente
    void onBackBtnClicked()
    {
        Panel.SetActive(false);
    }

    public void updateUserProfile()
    {
        PanelAvatar.GetComponentInChildren<TextMeshProUGUI>().text = manager
            .GetSelfJoueur()
            .NomUtilisateur;
        //Mettre à jour le reste du profil de l'utilisateur avec ses données
    }

    void onDisconnectionClicked()
    {
        manager.GetSelfJoueur().DeconnexionCompte();
    }
}
