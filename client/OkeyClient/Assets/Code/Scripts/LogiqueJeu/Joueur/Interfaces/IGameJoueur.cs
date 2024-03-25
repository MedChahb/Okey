namespace LogiqueJeu.Joueur
{
    public interface IGameJoueur
    {
        EtatTour.Etats EtatTour { get; }
        Chevalet Chevalet { get; set; }
        Emote CurrentEmote { get; set; }
        public void JouerTour(Timer Timer);
    }
}
