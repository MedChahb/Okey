namespace LogiqueJeu.Joueur
{
    using UnityEngine;

    [CreateAssetMenu(menuName = "ScriptableObjects/GameJoueur")]
    public sealed class GameJoueur : Joueur, IGameJoueur
    {
        public EtatTour.Etats EtatTour => throw new System.NotImplementedException();

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
    }
}
