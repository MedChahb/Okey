namespace LogiqueJeu.Joueur
{
    using UnityEngine;

    [CreateAssetMenu(menuName = "ScriptableObjects/SelfJoueur")]
    public sealed class SelfJoueur : Joueur, ISelfJoueur
    {
        public string Login { get; set; }
        public string TokenConnexion { get; set; }
    }
}
