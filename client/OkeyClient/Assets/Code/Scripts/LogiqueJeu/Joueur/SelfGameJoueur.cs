namespace LogiqueJeu.Joueur
{
    using UnityEngine;

    [CreateAssetMenu(menuName = "ScriptableObjects/SelfGameJoueur")]
    public sealed class SelfGameJoueur : Joueur, ISelfJoueur, IGameJoueur
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

        public void Piocher(Pile Pile, Chevalet Chevalet) =>
            throw new System.NotImplementedException();
    }
}
