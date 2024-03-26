namespace LogiqueJeu.Joueur
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public abstract class Joueur : ScriptableObject
    {
        [field: SerializeField]
        public string NomUtilisateur { get; set; }

        [field: SerializeField]
        public int Elo { get; set; }

        [field: SerializeField]
        public int IconeProfil { get; set; }

#pragma warning disable IDE0052, IDE0044
        // A placeholder for the time being until it gets potentially implemented
        private List<Achievement> Achievements;
#pragma warning restore IDE0052, IDE0044
        [field: SerializeField]
        public int Score { get; set; }

        [field: SerializeField]
        public int Niveau { get; set; }

        [field: SerializeField]
        public bool IsInGame { get; set; }

        // [field: SerializeField]
        // public bool IsInLobby { get; set; }

        public InGameDetails InGame
        {
            get
            {
                if (this.IsInGame)
                {
                    return (InGameDetails)this.InGame.Clone();
                }
                else
                {
                    return null;
                }
            }
        }

        [Serializable]
        public class InGameDetails : ICloneable
        {
            [field: SerializeField]
            protected EtatTour _etatTour;

            [field: SerializeField]
            public Chevalet Chevalet { get; set; }

            [field: SerializeField]
            public Emote CurrentEmote { get; set; }

            public EtatTour.Etats EtatTour
            {
                get { return this._etatTour.Etat; }
            }

            public object Clone()
            {
                return this.MemberwiseClone();
            }
        }

        public override string ToString()
        {
            var Representation = "";
            if (this.IsInGame)
            {
                Representation =
                    $@"
                                NomUtilisateur: {this.NomUtilisateur},
                                Elo: {this.Elo},
                                IconeProfil: {this.IconeProfil},
                                Score: {this.Score},
                                Niveau: {this.Niveau},
                                EtatTour: {this.InGame.EtatTour},
                                Chevalet: {this.InGame.Chevalet},
                                CurrentEmote: {this.InGame.CurrentEmote}
                                ";
            }
            else
            {
                Representation =
                    $@"
                                NomUtilisateur: {this.NomUtilisateur},
                                Elo: {this.Elo},
                                IconeProfil: {this.IconeProfil},
                                Score: {this.Score},
                                Niveau: {this.Niveau}
                                ";
            }
            return Representation;
        }
    }
}
