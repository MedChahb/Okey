using System;
using System.Collections.Generic;
using System.Text;
using Code.Scripts.SignalR.Packets;
using UnityEngine;

public class Chevalet : MonoBehaviour
{
    public static GameObject[] placeholders = new GameObject[28]; // Tableau des 28 Placeholders (la grille)

    public TuileData[,] tuiles2D = new TuileData[2, 14];
    public TuileData[,] tuilesPack = new TuileData[2, 14];
    private Stack<Tuile> pileGauche = new Stack<Tuile>();
    private Stack<Tuile> pileDroite = new Stack<Tuile>();
    private Stack<Tuile> pilePioche = new Stack<Tuile>();
    public static GameObject pileGauchePlaceHolder;
    public static GameObject pilePiochePlaceHolder;

    public static GameObject pileDroitePlaceHolder;
    public static GameObject jokerPlaceHolder;

    public bool isJete { get; set; }
    public TuilePacket tuileJete { get; set; }

    public TuilePacket tuilePiochee = null;

    public static bool neverReceivedChevalet = true;

    public static Chevalet Instance { get; set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.Log("Destroying duplicate instance of Chevalet.");
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
            Debug.Log("Chevalet instance set.");
        }
    }

    public void Init()
    {
        this.InitPlaceholders();
        this.InitializeBoardFromTuiles();
        tuileJete = null;
        isJete = false;
    }

    void Start()
    {
        this.InitPlaceholders();
        //this.InitializeBoardFromPlaceholders();
        //this.InitializeBoardFromTuiles();

        //PrintTuilesArray();
    }

    void InitPlaceholders()
    {
        // Remplir le tableau avec les placeholders dans la scene
        for (int i = 1; i <= 28; i++) //i commence a 1 car le premier placeholder est "PlaceHolder1"
        {
            GameObject placeholder = GameObject.Find("PlaceHolder" + i);
            if (placeholder != null)
            {
                placeholders[i - 1] = placeholder; // Mettre le placeholder dans le tableau
            }
            else
            {
                Debug.LogError("PlaceHolder" + i + " not found!");
            }
        }
        //ToDo : Instancier les piles en fonction de ce qu'on reçoit du Back
        pileGauchePlaceHolder = GameObject.Find("PileGauchePlaceHolder");
        pileDroitePlaceHolder = GameObject.Find("PileDroitePlaceHolder");
        pilePiochePlaceHolder = GameObject.Find("PiochePlaceHolder");
        jokerPlaceHolder = GameObject.Find("Okey");
        pileGauche.Push(
            pileGauchePlaceHolder.transform.GetChild(0).gameObject.GetComponent<Tuile>()
        );
        pileGauche.Push(
            pileGauchePlaceHolder.transform.GetChild(0).gameObject.GetComponent<Tuile>()
        );
        pilePioche.Push(
            pilePiochePlaceHolder.transform.GetChild(0).gameObject.GetComponent<Tuile>()
        );
        pileGauchePlaceHolder
            .transform.GetChild(0)
            .gameObject.GetComponent<Tuile>()
            .SetIsDeplacable(false);
        pileGauchePlaceHolder
            .transform.GetChild(0)
            .gameObject.GetComponent<Tuile>()
            .SetIsInStack(true);
        pileDroitePlaceHolder
            .transform.GetChild(0)
            .gameObject.GetComponent<Tuile>()
            .SetIsDeplacable(false);
        jokerPlaceHolder
            .transform.GetChild(0)
            .gameObject.GetComponent<Tuile>()
            .SetIsDeplacable(false);
        pilePiochePlaceHolder
            .transform.GetChild(0)
            .gameObject.GetComponent<Tuile>()
            .SetIsDeplacable(false);
        pilePiochePlaceHolder
            .transform.GetChild(0)
            .gameObject.GetComponent<Tuile>()
            .SetIsInPioche(true);
    }

    public GameObject ClosestPlaceholder(Vector3 position)
    {
        GameObject closestPlaceholder = null;
        float closestDistance = Mathf.Infinity;

        // Loop through placeholders using absolute positions
        foreach (GameObject placeholder in placeholders)
        {
            if (placeholder != null)
            {
                float distance = Vector3.Distance(position, placeholder.transform.position); // Absolute position
                if (distance < closestDistance)
                {
                    closestPlaceholder = placeholder;
                    closestDistance = distance;
                }
            }
        }

        // Check specific piles using absolute positions
        float distancePileDroite = Vector3.Distance(
            position,
            pileDroitePlaceHolder.transform.position
        ); // Absolute position

        // Determine closest placeholder based on distance and tile number
        if (distancePileDroite < closestDistance && getTilesNumber() == 15)
        {
            closestDistance = distancePileDroite;
            closestPlaceholder = pileDroitePlaceHolder;
        }
        else if (
            position.y > 0
            && getTilesNumber() == 15 /* et peut gagner*/
        )
        {
            closestPlaceholder = pilePiochePlaceHolder;
        }
        return closestPlaceholder;
    }

    public void UpdateTiles(GameObject placeholder)
    {
        // Vérifier si le placeholder a des enfants (des tuiles)
        if (placeholder.transform.childCount > 1)
        {
            // Déterminer le numéro du placeholder actuel
            int currentPlaceholderNumber = GetPlaceholderNumber(placeholder.name);
            int indexTab = currentPlaceholderNumber - 1;

            // Vérifier s'il y a un placeholder à droite et s'il n'est pas plein
            if (
                currentPlaceholderNumber < 29
                && placeholders[(indexTab + 1) % 28].transform.childCount <= 1
            )
            {
                // Déplacer la première tuile du placeholder actuel vers le placeholder à droite
                GameObject tile = placeholder.transform.GetChild(0).gameObject;
                tile.GetComponent<Tuile>().AttachToPlaceholder(placeholders[(indexTab + 1) % 28]);

                // Appeler récursivement la fonction UpdateTiles sur le placeholder à droite
                UpdateTiles(placeholders[(indexTab + 1) % 28]);
            }
        }
    }

    public void draw(bool piocheDroite)
    {
        if (
            getTilesNumber() == 14 /* et c'est mon tour*/
        )
        {
            if (piocheDroite)
            {
                pileGauchePlaceHolder
                    .transform.GetChild(0)
                    .gameObject.GetComponent<Tuile>()
                    .SetIsDeplacable(true);
                pileGauchePlaceHolder
                    .transform.GetChild(0)
                    .gameObject.GetComponent<Tuile>()
                    .SetIsInStack(false);
                pileGauche.Pop();
                if (pileGauche.Count > 0)
                {
                    GameObject newChild = Instantiate(
                        pileGauche.Peek().gameObject,
                        pileGauchePlaceHolder.transform
                    );
                    newChild.transform.localPosition = Vector3.zero;
                    newChild.GetComponent<Tuile>().SetIsInStack(true);
                    newChild.GetComponent<Tuile>().SetIsDeplacable(false);
                }
                //ToDo : Envoyer "Pioche à Droite"
                //
            }
            else
            {
                pilePiochePlaceHolder
                    .transform.GetChild(0)
                    .gameObject.GetComponent<Tuile>()
                    .SetIsDeplacable(true);
                pilePiochePlaceHolder
                    .transform.GetChild(0)
                    .gameObject.GetComponent<Tuile>()
                    .SetIsInPioche(false);
                pilePioche.Pop();
                if (pilePioche.Count > 0)
                {
                    GameObject newChild = Instantiate(
                        pilePioche.Peek().gameObject,
                        pilePiochePlaceHolder.transform
                    );
                    newChild.transform.localPosition = Vector3.zero;
                    newChild.GetComponent<Tuile>().SetIsInStack(true);
                    newChild.GetComponent<Tuile>().SetIsDeplacable(false);
                }
                //ToDo : Envoyer "Pioche au centre"
            }
        }
    }

    public void throwTile(Tuile tuile)
    {
        pileDroite.Push(tuile);

        Debug.Log($"Tuile recue {tuile.GetCouleur()}");

        /*
        TuileData tuileData = new TuileData(
            ConvertToFrontendColorToBackendEnumName(tuile.GetCouleur()),
            tuile.GetValeur(),
            tuile.GetIsJoker()
        );*/
        var tuileData = this.getTuilePacketFromChevalet(tuile);
        Debug.Log($"{tuileData.X}, {tuileData.Y}");

        // ON ENVOIE ICI

        //ToDo : Envoyer TuileData + Défausse droite
        isJete = true;
        tuileJete = tuileData;
    }

    public void throwTileToWin(Tuile tuile)
    {
        TuileData tuileData = new TuileData(
            ConvertToFrontendColorToBackendEnumName(tuile.GetCouleur()),
            tuile.GetValeur(),
            tuile.GetIsJoker()
        );
        //ToDo : Envoyer TuileData + Pile Pioche + tuiles2D
        //Attendre Vérif
        //Et communiquer le résultat
        //pilePioche.Push(tuile);
    }

    public TuilePacket getTuilePacketFromChevalet(Tuile t)
    {
        if (t != null)
        {
            for (var x = 0; x < 2; x++)
            {
                for (var y = 0; y < 14; y++)
                {
                    if (
                        t.GetCouleur().Equals("V", StringComparison.Ordinal)
                        || t.GetCouleur().Equals("J", StringComparison.Ordinal)
                    )
                    {
                        if (
                            this.tuilesPack[x, y].couleur.Equals("V", StringComparison.Ordinal)
                            || this.tuilesPack[x, y].couleur.Equals("J", StringComparison.Ordinal)
                        )
                        {
                            if (
                                t.GetValeur() == this.tuilesPack[x, y].num
                                && t.GetIsJoker() == this.tuilesPack[x, y].isJoker
                            )
                            {
                                return new TuilePacket
                                {
                                    X = "" + y,
                                    Y = "" + x,
                                    gagner = false
                                };
                            }
                        }
                    }
                    else
                    {
                        if (
                            t.GetCouleur()
                                .Equals(this.tuilesPack[x, y].couleur, StringComparison.Ordinal)
                            && t.GetValeur() == this.tuilesPack[x, y].num
                        )
                        {
                            return new TuilePacket
                            {
                                X = "" + y,
                                Y = "" + x,
                                gagner = false
                            };
                        }
                    }
                }
            }
        }

        return new TuilePacket();
    }

    // Fonction auxiliaire pour extraire le numéro du placeholder à partir de son nom
    public static int GetPlaceholderNumber(string name)
    {
        string numberString = name.Substring(11);
        int number;
        int.TryParse(numberString, out number);
        return number;
    }

    private void InitializeBoardFromPlaceholders()
    {
        for (var i = 0; i < placeholders.Length; i++)
        {
            var x = i / 14; // Calculate the row based on index.
            var y = i % 14; // Calculate the column based on index.

            var placeholder = placeholders[i];

            //Debug.Log($"Checking placeholder {i}, Child count: {placeholder.transform.childCount}");

            if (placeholder.transform.childCount > 0)
            {
                // Get the first child of the placeholder
                var child = placeholder.transform.GetChild(0).gameObject;

                if (child.TryGetComponent<SpriteRenderer>(out var childSpriteRenderer))
                {
                    // note: "color_value" should be used for naming the sprites
                    var properties = childSpriteRenderer.sprite.name.Split('_');
                    //Debug.Log(properties[0] + properties[1]);

                    if (properties.Length == 2)
                    {
                        var couleur = this.ConvertToFrontendColorToBackendEnumName(properties[0]);
                        //                    Debug.Log(couleur);
                        var num = int.Parse(properties[1]);
                        var isJoker = properties[0] == "FakeJoker";
                        this.tuiles2D[x, y] = new TuileData(couleur, num, isJoker);
                        //this.tuilesPack[x, y] = new TuileData(couleur, num, isJoker);
                        //Debug.Log("["+x+"]"+"["+y+"]"+" "+this.tuiles2D[x, y].couleur+ " " +this.tuiles2D[x, y].num+" "+this.tuiles2D[x, y].isJoker);
                    }
                    else
                    {
                        // If the name does not contain both color and value, log an error or set as null
                        Debug.LogError(
                            $"Child sprite of placeholder at index {i} does not have a properly formatted name."
                        );
                        this.tuiles2D[x, y] = null;
                        //this.tuilesPack[x, y] = null;
                    }
                }
                else
                {
                    // If there is no SpriteRenderer component, set the corresponding array position to null
                    this.tuiles2D[x, y] = null;
                    //this.tuilesPack[x, y] = null;
                }
            }
            else //empty placeholder
            {
                // If the placeholder is empty, set the corresponding array position to null
                this.tuiles2D[x, y] = null;
            }
        }
    }

    private static string fromTuileToSpriteName(TuileData tuile)
    {
        if (
            tuile.isJoker
            || tuile.couleur.Equals("M", StringComparison.Ordinal)
            || tuile.couleur.Equals("X", StringComparison.Ordinal)
        )
        {
            return "Fake Joker_1";
        }
        var name = "";
        switch (tuile.couleur)
        {
            case "B":
                name = "Blue_";
                break;
            case "R":
                name = "Red_";
                break;
            case "N":
                name = "Black_";
                break;
            case "J":
                name = "Green_";
                break;
        }

        name += tuile.num;
        return name;
    }

    private void InitializeBoardFromTuiles()
    {
        var sprites = Resources.LoadAll<Sprite>("Tiles");

        var spritesDic = new Dictionary<string, Sprite>();

        for (var i = 0; i < 13; i++)
        {
            spritesDic.Add($"Black_{i + 1}", sprites[i]);
        }

        for (var i = 13; i < 26; i++)
        {
            spritesDic.Add($"Blue_{(i + 1) - 13}", sprites[i]);
        }

        spritesDic.Add("Fake Joker_1", sprites[26]);
        spritesDic.Add("Fake Joker_2", sprites[27]);

        for (var i = 28; i < 41; i++)
        {
            spritesDic.Add($"Green_{(i + 1) - 28}", sprites[i]);
        }

        spritesDic.Add($"Pioche", sprites[41]);

        for (var i = 42; i < 55; i++)
        {
            spritesDic.Add($"Red_{(i + 1) - 42}", sprites[i]);
        }

        for (var i = 0; i < placeholders.Length; i++)
        {
            var x = i / 14;
            var y = i % 14;
            var placeholder = placeholders[i];
            if (this.tuiles2D[x, y] != null)
            {
                var childObject = new GameObject("SpriteChild");
                childObject.transform.SetParent(placeholder.transform);
                var spriteRen = childObject.AddComponent<SpriteRenderer>();
                var mat = new Material(Shader.Find("Sprites/Default"));
                mat.color = new Color(
                    0.9529411764705882f,
                    0.9411764705882353f,
                    0.8156862745098039f
                );
                spriteRen.material = mat;
                spriteRen.sprite = spritesDic[fromTuileToSpriteName(this.tuiles2D[x, y])];
                spriteRen.sortingOrder = 3;
                spriteRen.transform.localPosition = new Vector3(0, 0, 0);
                spriteRen.transform.localScale = new Vector3(1, 1, 1);
                childObject.AddComponent<Tuile>();
                var collider2D = childObject.AddComponent<BoxCollider2D>();
                collider2D.size = new Vector2((float)0.875, (float)1.25);

                placeholder.GetComponent<Tuile>().SetValeur(this.tuiles2D[x, y].num);
                placeholder.GetComponent<Tuile>().SetCouleur(this.tuiles2D[x, y].couleur);
                placeholder.GetComponent<Tuile>().SetIsJoker(this.tuiles2D[x, y].isJoker);
            }
        }
    }

    private CouleurTuile ConvertToFrontendColorToBackendEnumName(string FrontendColor)
    {
        //Debug.Log(FrontendColor);
        return FrontendColor.ToLower() switch
        {
            "yellow" => CouleurTuile.J,
            "black" => CouleurTuile.N,
            "red" => CouleurTuile.R,
            "blue" => CouleurTuile.B,
            "green" => CouleurTuile.V,
            // other cases still needed
            "fakejoker" => CouleurTuile.X,
            _ => CouleurTuile.M, // the okey
        };
    }

    private (int, int) ConvertPlaceHolderNumberToMatrixCoordinates(int PlaceholderNumber)
    {
        //conversion du numero du place holder 1->28 en coordonnées dans la matrice [2][14]
        int x = (PlaceholderNumber - 1) / 14;
        int y = (PlaceholderNumber - 1) % 14;
        return (x, y);
    }

    public void UpdateMatrixAfterMovement(
        GameObject PreviousPlaceHolder,
        GameObject NextPlaceholder
    )
    {
        //cas de deplacement a l'interieur du chevalet : Chevalet -> Chevalet
        GameObject ChevaletFront = GameObject.Find("ChevaletFront");
        if (
            PreviousPlaceHolder.transform.IsChildOf(ChevaletFront.transform)
            && NextPlaceholder.transform.IsChildOf(ChevaletFront.transform)
        )
        {
            this.InitializeBoardFromPlaceholders(); // il faut re parcourir le chevalets pour recuperer les nouvelles position car il y'a le decalage des tuiles

            var tuile = PreviousPlaceHolder.GetComponent<Tuile>();
            var nt = NextPlaceholder.GetComponent<Tuile>();
            nt.SetCouleur(tuile.GetCouleur());
            nt.SetValeur(tuile.GetValeur());
            nt.SetIsJoker(tuile.GetIsJoker());

            tuile.SetCouleur(null);
            tuile.SetValeur(0);
        }
        //cas de tirage : pioche ou pile gauche -> Chevalet
        else if (
            PreviousPlaceHolder == pileGauchePlaceHolder
            || PreviousPlaceHolder == pilePiochePlaceHolder
                && NextPlaceholder.transform.IsChildOf(ChevaletFront.transform)
        )
        {
            (int, int) nxt_ph_pos = ConvertPlaceHolderNumberToMatrixCoordinates(
                GetPlaceholderNumber(NextPlaceholder.name)
            );

            //ajouter la piece pioché au chevalet
            InitializeBoardFromPlaceholders(); // il faut re parcourir le chevalets car quand on pioche on peut la mettre entre 2 tuiles et ca crée un decalage

            //Faudra parler a lequipe du backend pour savoir si ca leur suffit la matrice mis a jour et le contenu des defausses ou ils veulent exactement la piece pioché
        }
        //cas de jet : Chevalet -> pile droite ou joker(gagné)
        else if (
            PreviousPlaceHolder.transform.IsChildOf(ChevaletFront.transform)
                && NextPlaceholder == pileDroitePlaceHolder
            || NextPlaceholder == jokerPlaceHolder
        )
        {
            //enlever la piece jeté du chevalet
            (int, int) prv_ph_pos = ConvertPlaceHolderNumberToMatrixCoordinates(
                GetPlaceholderNumber(PreviousPlaceHolder.name)
            );

            //Debug.Log($"tuile[{prv_ph_pos.Item1}][{prv_ph_pos.Item1}] a été jeté");
            Debug.Log(
                ""
                    + this.tuiles2D[prv_ph_pos.Item1, prv_ph_pos.Item2].couleur
                    + this.tuiles2D[prv_ph_pos.Item1, prv_ph_pos.Item2].num
                    + this.tuiles2D[prv_ph_pos.Item1, prv_ph_pos.Item2].isJoker
            );

            this.tuiles2D[prv_ph_pos.Item1, prv_ph_pos.Item2] = null;

            //Faudra parler a lequipe du backend pour savoir si ca leur suffit la matrice mis a jour et le contenu des defausses ou ils veulent exactement la piece jeté
        }
        else //cas derreur
        {
            Debug.Log("error updating matrix after movement");
        }

        Print2DMatrix();
    }

    public void Print2DMatrix() //Really useful to visualize tiles placement matrix
    {
        // Initialize a string to store the table
        string table = "";

        for (int x = 0; x < 2; x++) // Rows
        {
            for (int y = 0; y < 14; y++) // Columns
            {
                if (this.tuiles2D[x, y] != null)
                {
                    table +=
                        $"| {this.tuiles2D[x, y].couleur} {this.tuiles2D[x, y].num} {this.tuiles2D[x, y].isJoker} ";
                }
                else
                {
                    table += "| Vide ";
                }
            }
            table += "|\n                    "; // End of the row
        }

        // Print the table
        Debug.Log(table);
    }

    public void SetTuiles(TuileData[,] tuiles)
    {
        this.tuiles2D = tuiles;
    }

    //public void SetTuile(int x, int y, TuileData tuile)
    //{
    //    tuiles2D[x, y] = tuile;
    //}

    //// get Tuile de l'index donnee
    //public TuileData GetTuile(int x, int y)
    //{
    //    if (x >= 0 && x < 2 && y >= 0 && y < 14)
    //    {
    //        return tuiles2D[x, y];
    //    }
    //    else
    //    {
    //        Debug.LogError("GetTuile: Index out of bounds");
    //        return null;
    //    }
    //}

    public TuileData[,] GetTuilesArray()
    {
        return this.tuiles2D;
    }

    private void PrintTuilesArray()
    {
        var sb = new StringBuilder();
        sb.AppendLine("Board State:");

        for (int x = 0; x < this.tuiles2D.GetLength(0); x++)
        {
            for (int y = 0; y < this.tuiles2D.GetLength(1); y++)
            {
                TuileData tile = this.tuiles2D[x, y];
                if (tile != null)
                {
                    sb.AppendLine(
                        $"Tile at [{x},{y}]: Color = {tile.couleur}, Number = {tile.num}, IsJoker = {tile.isJoker}"
                    );
                }
                else
                {
                    sb.AppendLine($"Tile at [{x},{y}]: None");
                }
            }
        }

        Debug.Log(sb.ToString());
    }

    public int getTilesNumber()
    {
        int num = 0;
        foreach (GameObject placeholder in placeholders)
        {
            if (placeholder.transform.childCount == 1)
            {
                num++;
            }
        }
        return num;
    }
}








/*
    to keep just in case :
    public static Tuile[] GetTilesPlacementInChevaletTab()
    {
        // Creation d'un tableau pour stocker le placement courant des tuiles sur le chevalet
        Tuile[] TilesArray = new Tuile[placeholders.Length];

        //loop sur les placeholders
        foreach (GameObject placeholder in placeholders)
        {
            int index = Array.IndexOf(placeholders, placeholder);

            if (placeholder.transform.childCount > 0)
            { // le placeholder a un child -> il contient une tuile -> on la met dans le tableau avec l'index du placeholder parent
                GameObject child = placeholder.transform.GetChild(0).gameObject;
                Tuile tuile = child.GetComponent<Tuile>();

                if (tuile != null)
                {
                    // Place the Tuile in the 1D array
                    TilesArray[index] = tuile;
                }
                else
                {
                    Debug.LogError("Error : placeholder has a child that isn't a Tile");
                }
            }
            else // si le placeholder n'a aucun child -> il est vide -> on met null a l'index correspondant au placeholder dans le tableau
            {
                TilesArray[index] = null;
            }
        }

        return TilesArray;
    }

    public static void PrintTilesArrayForTest()
    {
        // Get the tiles placement array using the method
        Tuile[] TilesArray = GetTilesPlacementInChevaletTab();

        // Check if TilesArray is not null
        if (TilesArray != null)
        {
            // Loop through the TilesArray to print tile number and color
            for (int i = 0; i < TilesArray.Length; i++)
            {
                if (TilesArray[i] != null)
                {
                    // Check if GetValeur() and GetCouleur() are not null
                    if (
                        TilesArray[i].GetValeur().ToString() != null
                        && TilesArray[i].GetCouleur().ToString() != null
                    ) // supposons que les valeurs sont init a 0
                    {
                        Debug.Log(
                            "Tile Number "
                                + (i + 1)
                                + " :"
                                + TilesArray[i].GetValeur().ToString()
                                + ", Tile Color: "
                                + TilesArray[i].GetCouleur().ToString()
                        );
                    }
                    else
                    {
                        Debug.Log("Tile at index " + i + " has null values.");
                    }
                }
                else
                {
                    Debug.Log("Tile at index " + i + " is null.");
                }
            }
        }
        else
        {
            Debug.Log("TilesArray is null.");
        }
    }
    */
