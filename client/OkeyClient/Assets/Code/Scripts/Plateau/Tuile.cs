using System.Collections.Generic;
using UnityEngine;

public class Tuile : MonoBehaviour
{
    private string couleur;
    private int valeur;
    private bool isJoker = false;
    private SpriteRenderer sprite;
    private bool deplacable = true;
    private bool estDeplace = false;
    private GameObject placeholderActuel;
    private Vector3 offset;
    private Chevalet chevalet;

    void Start()
    {
        this.sprite = GetComponent<SpriteRenderer>();
        chevalet = GetComponentInParent<Chevalet>(); // Find Chevalet script in the parent hierarchy
        placeholderActuel = transform.parent.gameObject;
    }

    void Update()
    {
        if (estDeplace)
        {
            // Update tile position based on mouse position
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.localPosition = new Vector3(mousePosition.x - offset.x, mousePosition.y - offset.y, transform.localPosition.z);
        }
    }

    void OnMouseDown()
    {
        if (deplacable)
        {
            // Save difference between mouse position and tile position
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            offset = new Vector3(mousePosition.x - transform.localPosition.x, mousePosition.y - transform.localPosition.y, 0);
            estDeplace = true;
        }
    }

    void OnMouseUp()
    {
        if (deplacable)
        {
            estDeplace = false;
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            // Find the closest placeholder using the Chevalet's function
            GameObject closestPlaceholder = chevalet.ClosestPlaceholder(new Vector3(mousePosition.x, mousePosition.y, transform.localPosition.z));

            if (closestPlaceholder != null)
            {
                //if (closestPlaceholder.transform.childCount == 0)
                //{
                // Attach the tile to the new placeholder
                AttachToPlaceholder(closestPlaceholder);
                //chevalet.UpdateTiles(closestPlaceholder);
                /*}
                else
                {
                    // The placeholder countains already a Tile, we must update before insert
                    //chevalet.UpdateTiles(closestPlaceholder);
                }*/
            }
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
}
