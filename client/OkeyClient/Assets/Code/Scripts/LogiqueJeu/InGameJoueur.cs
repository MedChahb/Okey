public abstract class InGameJoueur : IInGameJoueur
{
    private readonly EtatTourInterne EtatTourInterne;

    // private Chevalet Chevalet;
    // private Emote CurrentEmote;

    public EtatTourInterneEnum GetEtatTour() => this.EtatTourInterne.EtatTour;

    public abstract void JouerTour();
}
