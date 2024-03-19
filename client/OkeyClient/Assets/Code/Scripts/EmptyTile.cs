using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyTile : MonoBehaviour
{
    public string color = ""; //couleur de la tuile
    public string number = ""; // numéro sur la tuile
    public bool IsJoker = false; // tuile joker ou pas

    //private bool isPlaced=false ; //tuile bien placé dans la grille(sur un placeholder)
    /*
    isplaced va servir plus tard pour le deplacement des tuiles: isPlaced=true si la tuile est positionnée sur un placeholder
    cette partie doit etre géré par la personne qui fera le deplacement des tuiles(moi j'ai crée la grille ou elle devront être placé )
    */
    SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Set sprite pour la tuile
    public void SetTileSprite(Sprite sprite)
    {
        spriteRenderer.sprite = sprite;
        color = sprite.name.Split('_')[0];
        number = sprite.name.Split('_')[1];
    }
}
