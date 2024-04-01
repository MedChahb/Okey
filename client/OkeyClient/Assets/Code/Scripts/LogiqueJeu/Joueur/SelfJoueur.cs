namespace LogiqueJeu.Joueur
{
    using System;
    using System.Collections;
    using System.IO;
    using System.Xml.Serialization;
    using LogiqueJeu.Constants;
    using UnityEngine;
    using UnityEngine.Networking;

    public sealed class SelfJoueur : Joueur
    {
        public string Login { get; set; }

        public SelfJoueur()
            : base()
        {
            this.InGame.Pos = Position.SoiMeme;
        }

        public string TokenConnexion { get; set; }

        public override void LoadSelf(MonoBehaviour Behaviour)
        {
            if (!File.Exists(Application.persistentDataPath + Constants.SELF_PLAYER_SAVE_FILE))
            {
                return;
            }
            TextReader reader = null;
            try
            {
                var serializer = new XmlSerializer(typeof(SelfJoueur));
                reader = new StreamReader(
                    Application.persistentDataPath + Constants.SELF_PLAYER_SAVE_FILE
                );
                this.CopyFrom((SelfJoueur)serializer.Deserialize(reader));
            }
            finally
            {
                reader?.Close();
            }
            Behaviour.StartCoroutine(this.FetchUserBGSelf(this.UnmarshalAndInit));
        }

        private IEnumerator FetchUserBGSelf(Action<string> CallbackJSON = null)
        {
            var Response = "";

            var www = UnityWebRequest.Get(
                Constants.API_URL_DEV + "/compte/watch/" + this.NomUtilisateur
            );
            www.SetRequestHeader("Authorization", "Bearer " + this.TokenConnexion);
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

        private void CopyFrom(SelfJoueur SelfJoueur)
        {
            base.CopyFrom(SelfJoueur);
            this.Login = SelfJoueur.NomUtilisateur;
            this.TokenConnexion = SelfJoueur.TokenConnexion;
        }

        protected override void UnmarshalAndInit(string Json)
        {
            var unmarshal = JsonUtility.FromJson<SelfJoueurAPICompteDTO>(Json);
            this.NomUtilisateur = unmarshal.username;
            this.Elo = unmarshal.elo;
            this.Achievements = unmarshal.achievements;
            this.SaveXML();
        }
    }
}
