namespace LogiqueJeu.Joueur
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Xml.Serialization;
    using LogiqueJeu.Constants;
    using UnityEngine;
    using UnityEngine.Networking;

    public abstract class Joueur
    {
        public string NomUtilisateur { get; set; }

        public int Elo { get; set; }

        public int IconeProfil { get; set; }

#pragma warning disable IDE0052, IDE0044
        // A placeholder for the time being until it gets potentially implemented
        // private List<Achievement> Achievements;
        public List<bool> Achievements;
#pragma warning restore IDE0052, IDE0044
        public int Score { get; set; }

        public int Niveau { get; set; }

        public int Classement { get; set; }

        // Not sure if this is necessary
        public bool IsInGame { get; set; }

        // public bool IsInLobby { get; set; }

        [XmlIgnore]
        public InGameDetails InGame;

        public struct InGameDetails
        {
            public int ID;
            public Position? Pos;
            public EtatTour _etatTour;
            public Chevalet Chevalet { get; set; }
            public Emote CurrentEmote { get; set; }

            public readonly EtatTour.Etats EtatTour
            {
                get { return this._etatTour.Etat; }
            }
        }

        public Joueur()
        {
            this.NomUtilisateur = "Anonyme";
            this.Elo = 0;
            this.IconeProfil = 0;
            this.Score = 0;
            this.Niveau = 0;
            this.IsInGame = false;
            this.InGame = new InGameDetails
            {
                ID = -1,
                Pos = null,
                _etatTour = new EtatTour(),
                Chevalet = null,
                CurrentEmote = null
            };
        }

        public override string ToString()
        {
            var Representation =
                $@"
                                NomUtilisateur: {this.NomUtilisateur},
                                Elo: {this.Elo},
                                IconeProfil: {this.IconeProfil},
                                Achievements: {string.Join(",", this.Achievements)},
                                Score: {this.Score},
                                Niveau: {this.Niveau}

                                ";
            if (this.IsInGame)
            {
                Representation +=
                    $@"
                                ID (SignalR): {this.InGame.ID},
                                Positon: {this.InGame.Pos},
                                EtatTour: {this.InGame.EtatTour},
                                Chevalet: {this.InGame.Chevalet},
                                CurrentEmote: {this.InGame.CurrentEmote}
                                ";
            }
            return Representation;
        }

        protected virtual void CopyFrom(Joueur Joueur)
        {
            this.NomUtilisateur = Joueur.NomUtilisateur;
            this.Elo = Joueur.Elo;
            this.IconeProfil = Joueur.IconeProfil;
            this.Score = Joueur.Score;
            this.Niveau = Joueur.Niveau;
            this.IsInGame = false;
        }

        public abstract void LoadSelf(MonoBehaviour Behaviour);

        public virtual void UpdateDetails(MonoBehaviour Behaviour)
        {
            Behaviour.StartCoroutine(this.FetchUserBG(this.UnmarshalAndInit));
        }

        protected virtual void UnmarshalAndInit(string Json)
        {
            var unmarshal = JsonUtility.FromJson<JoueurAPICompteDTO>(Json);
            this.NomUtilisateur = unmarshal.username;
            this.Elo = unmarshal.elo;
        }

        protected virtual IEnumerator FetchUserBG(Action<string> CallbackJSON = null)
        {
            var Response = "";

            var www = UnityWebRequest.Get(
                Constants.API_URL_DEV + "/compte/watch/" + this.NomUtilisateur
            );
            www.certificateHandler = new BypassCertificate();
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                Response = www.downloadHandler.text;
            }

            CallbackJSON?.Invoke(Response);
        }
    }

    public class BypassCertificate : CertificateHandler
    {
        // Cela devrait pas être nécessaire vu que la connexion passe par HTTPS
        // avec un certificat de l'Unistra bien reconnu.
        // Il faut mettre l'API en HTTP simple.
        // À mon avis il y a deux HTTPS en jeu en ce moment,
        // 1) le bastion Unistra, 2) l'API ou le reverse proxy Nginx.
        // Il faut enlever le HTTPS de l'API pour que ça marche mieux sans cette duplication.
        protected override bool ValidateCertificate(byte[] certificateData)
        {
            //Simply return true no matter what
            return true;
        }
    }
}
