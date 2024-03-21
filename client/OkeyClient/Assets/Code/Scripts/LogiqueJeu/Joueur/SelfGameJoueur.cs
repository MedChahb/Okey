public sealed class SelfGameJoueur : ISelfJoueur, IGameJoueur
{
    public string Login
    {
        get => throw new System.NotImplementedException();
        set => throw new System.NotImplementedException();
    }
    public string TokenConnexion
    {
        get => throw new System.NotImplementedException();
        set => throw new System.NotImplementedException();
    }

    public EtatTourJoueurEnum EtatTour => throw new System.NotImplementedException();

    public Chevalet Chevalet
    {
        get => throw new System.NotImplementedException();
        set => throw new System.NotImplementedException();
    }
    public Emote CurrentEmote
    {
        get => throw new System.NotImplementedException();
        set => throw new System.NotImplementedException();
    }

    public void JouerTour(Timer Timer) => throw new System.NotImplementedException();

    public void Piocher(Pile Pile, Chevalet Chevalet) => throw new System.NotImplementedException();
}
