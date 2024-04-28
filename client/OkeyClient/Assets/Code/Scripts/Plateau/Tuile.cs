using System.Collections.Generic;
using UnityEngine;

public class Tuile : MonoBehaviour
{
    [SerializeField]
    private string couleur;

    [SerializeField]
    private int valeur;
    private bool isJoker = false;
    private SpriteRenderer sprite;
    private bool deplacable = true;
    private bool estDeplace = false;
    private GameObject placeholderActuel;

    private GameObject PreviousPlaceHolder;
    private Vector3 offset;
    private Chevalet chevalet;
    private bool isInStack = false;
    private bool isInPioche = false;
    private Vector3 initialOffset;

    void Start()
    {
        this.sprite = GetComponent<SpriteRenderer>();
        //chevalet = GetComponentInParent<Chevalet>(); // Find Chevalet script in the parent hierarchy
        chevalet = GameObject.Find("ChevaletFront").GetComponent<Chevalet>();
        placeholderActuel = transform.parent.gameObject;
    }

    void Update()
    {
        if (estDeplace)
        {
            // Update tile position based on mouse position
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(
                mousePosition.x + initialOffset.x,
                mousePosition.y + initialOffset.y,
                -7
            );
        }
    }

    void OnMouseDown() //click
    {
        if (isInStack)
        {
            this.chevalet.draw(true);
        }
        if (isInPioche)
        {
            this.chevalet.draw(false);
        }
        if (deplacable)
        {
            // Save difference between mouse position and tile position
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            initialOffset = transform.position - mousePosition;
            estDeplace = true;
        }
    }

    void OnMouseUp() //release
    {
        if (deplacable)
        {
            estDeplace = false;
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            // Find the closest placeholder using the Chevalet's function
            GameObject closestPlaceholder = chevalet.ClosestPlaceholder(
                new Vector3(mousePosition.x, mousePosition.y, transform.localPosition.z)
            );
            //pour enregistrer le placeholder ou la tuile était avant que attach to placeholder modifie placeholderactuel
            PreviousPlaceHolder = placeholderActuel;

            if (closestPlaceholder != null)
            {
                if (closestPlaceholder.transform.childCount == 0)
                {
                    // Attach the tile to the new placeholder
                    AttachToPlaceholder(closestPlaceholder);
                }
                else
                {
                    AttachToPlaceholder(closestPlaceholder);
                    if (
                        closestPlaceholder != Chevalet.pileDroitePlaceHolder
                        && closestPlaceholder != Chevalet.jokerPlaceHolder
                    )
                    {
                        // The placeholder countains already a Tile, we must update before insert
                        chevalet.UpdateTiles(closestPlaceholder);
                    }
                    else
                    {
                        deplacable = false;
                        if (closestPlaceholder == Chevalet.pileDroitePlaceHolder)
                        {
                            this.chevalet.throwTile(this);
                        }
                    }
                }
            }

            chevalet.UpdateMatrixAfterMovement(PreviousPlaceHolder, closestPlaceholder); //placeholder ou la piece était avant le deplacement et le placeholder ou elle a été deplacé
            Debug.Log(
                "Tile changed position from : "
                    + PreviousPlaceHolder.name
                    + " to "
                    + closestPlaceholder.name
            );
        }
    }

    public void AttachToPlaceholder(GameObject placeholder)
    {
        // Detach tile from current parent (if any)
        transform.parent = null;

        // Set tile position to placeholder position
        transform.localPosition = placeholder.transform.position;

        // Set tile rotation to placeholder rotation
        transform.rotation = placeholder.transform.rotation;

        // Attach tile to the placeholder
        transform.parent = placeholder.transform;
        placeholderActuel = placeholder;
    }

    /*
    public void SetBlockSprite(Sprite sprite)
    {
        this.sprite.sprite = sprite;
        this.couleur = sprite.name.Split('_')[0];
        this.valeur = int.Parse(sprite.name.Split('_')[1]);
    }*/

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

    public void SetTileData(TuileData t)
    {
        this.SetCouleur(t.couleur);
        this.SetIsJoker(t.isJoker);
        this.SetValeur(t.num);
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

    public void SetIsDeplacable(bool isDeplacable)
    {
        this.deplacable = isDeplacable;
    }

    public void SetIsInStack(bool isInStack)
    {
        this.isInStack = isInStack;
    }

    public void SetIsInPioche(bool isInPioche)
    {
        this.isInPioche = isInPioche;
    }
}
