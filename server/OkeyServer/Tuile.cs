using System;

public enum CouleurTuile
{
    Jaune,
    Noir,
    Rouge,
    Bleu,
    Multi
}

public abstract class Tuile
{
    public abstract void AfficherDetails();
    protected bool defausse; //pour savior si defausse est accessible
    protected bool DansPioche; //pour savoir si la tuile est dans pioche
    protected CouleurTuile Couleur;
    protected int Numero;

    public Tuile(CouleurTuile couleur, int num, bool P)
    {
        this.Couleur = couleur;
        this.Numero = num;
        this.defausse = false;
        this.DansPioche = P;
    }
}

public class TuileNormale : Tuile
{
    //public int Numero { get; }
    //public CouleurTuile Couleur { get; }

    public TuileNormale(CouleurTuile couleur, int num, bool P)
        : base(couleur, num, P) { }

    public override void AfficherDetails()
    {
        Console.WriteLine($"Tuile Normale - Numéro : {Numero}, Couleur : {Couleur}");
    }
}

public class TuileJoker : Tuile
{
    public TuileJoker(CouleurTuile couleurJoker, int num, bool P)
        : base("", "", P) { }

    public override void AfficherDetails()
    {
        Console.WriteLine($"Tuile Joker - Numéro : {NumeroJoker}, Couleur : {CouleurJoker}");
    }
}

public class TuileOkey : Tuile
{
    public TuileOkey(CouleurTuile couleurOkey, int num, bool P)
        : base(CouleurTuile.Multi, 0, P) { }

    public override void AfficherDetails()
    {
        Console.WriteLine($"Tuile Okey - Numéro : {NumeroOkey}, Couleur : {CouleurOkey}");
    }
}
