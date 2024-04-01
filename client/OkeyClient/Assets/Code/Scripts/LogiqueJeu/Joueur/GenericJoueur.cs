namespace LogiqueJeu.Joueur
{
    using UnityEngine;

    public sealed class GenericJoueur : Joueur
    {
        public override void LoadSelf(MonoBehaviour Behaviour)
        {
            Behaviour.StartCoroutine(this.FetchUserBG(this.UnmarshalAndInit));
        }
    }
}
