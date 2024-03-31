using System.Collections.Generic;
using LogiqueJeu.Joueur;
using UnityEngine;

public class JoueurManager : MonoBehaviour
{
    private readonly List<Joueur> Joueurs = new(3);
    private SelfJoueur SoiMeme;
    public readonly List<JoueurSO> JoueursSOs = new(4);

    private void Awake()
    {
        this.SoiMeme = new();

        this.SoiMeme.NomUtilisateur = "Testeur1";
        this.SoiMeme.TokenConnexion =
            "eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJnaXZlbl9uYW1lIjoiVGVzdGV1cjEiLCJuYmYiOjE3MTE4NDc3MDksImV4cCI6MTcxMjQ1MjUwOSwiaWF0IjoxNzExODQ3NzA5LCJpc3MiOiJodHRwOi8vbG9jYWxob3N0OjUyNDYiLCJhdWQiOiJodHRwOi8vbG9jYWxob3N0OjUyNDYifQ.uf4hmpxz6MwjWD7txbxuZtf64gEMg_kxuQRYFNmWEGnw5pDLJShABigmZrFhUdODs11nBQNvQMVzV3v8VFRyfQ";

        this.SoiMeme.LoadSelf(this);

        var SoiMemeSO = ScriptableObject.CreateInstance<JoueurSO>();
        SoiMemeSO.Joueur = this.SoiMeme;
        this.JoueursSOs.Add(SoiMemeSO);
    }

    private void Update() { }
}
