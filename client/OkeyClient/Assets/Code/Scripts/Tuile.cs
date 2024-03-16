using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tuile : MonoBehaviour
{
    private string couleur; //0: blue, 1: black, 2: green, 3: red
    private int valeur; // number between 1 and 13
    private bool isJoker = false;
    private SpriteRenderer sprite;
    
     private bool isPlaced=false ; //tuile bien placé dans la grille(sur un placeholder)

    // Start is called before the first frame update
    void Start()
    {
        this.sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!this.isPlaced)
            this.transform.position = Vector3.Lerp(
                this.transform.position,
                Camera.main.ScreenPointToRay(Input.mousePosition).GetPoint(5),
                Time.deltaTime * 6
            );
    }

    //thibault il faudrait juste modifier deplacable/estDeplace 
    void OnMouseDown()
    {
        if (this.deplacable)
        {
            this.estDeplace = true;
        }
    }

    void OnMouseUp()
    {
        if (this.deplacable)
        {
            this.estDeplace = false;
        }
    }

    public void SetBlockSprite(Sprite sprite)
    {
        this.sprite.sprite = sprite;
        this.couleur = sprite.name.Split('_')[0];
        this.valeur = int.Parse(sprite.name.Split('_')[1]);
    }

    public string GetCouleur()
    {
        return this.couleur;
    }

    public void SetCouleur(string coul)
    {
        this.couleur = coul;
    }

    public int GetValeur()
    {
        return this.valeur;
    }

    public void SetValeur(int val)
    {
        this.valeur = val;
    }

    public bool GetIsJoker()
    {
        return this.isJoker;
    }

    public void SetIsJoker(bool isJoke)
    {
        this.isJoker = isJoke;
    }

    // Set sprite pour la tuile
    public void SetTileSprite(Sprite sprite)
    {
        spriteRenderer.sprite = sprite;
        color = sprite.name.Split('_')[0];
        number = sprite.name.Split('_')[1];
    }
}
