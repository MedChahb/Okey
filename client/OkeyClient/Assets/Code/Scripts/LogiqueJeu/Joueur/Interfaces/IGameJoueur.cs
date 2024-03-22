public interface IGameJoueur
{
    EtatTourJoueurEnum EtatTour { get; }
    Chevalet Chevalet { get; set; }
    Emote CurrentEmote { get; set; }
    public void JouerTour(Timer Timer);
}
