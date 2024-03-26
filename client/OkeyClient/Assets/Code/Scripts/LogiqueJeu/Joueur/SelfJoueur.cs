namespace LogiqueJeu.Joueur
{
    using UnityEngine;

    [CreateAssetMenu(menuName = "ScriptableObjects/SelfJoueur")]
    public sealed class SelfJoueur : Joueur
    {
        [field: SerializeField]
        public string Login { get; set; }

        [field: SerializeField]
        public string TokenConnexion { get; set; }
    }
}
