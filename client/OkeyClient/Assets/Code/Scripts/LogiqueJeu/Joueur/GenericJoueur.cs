namespace LogiqueJeu.Joueur
{
    using System;
    using System.Collections;
    using System.Text.Json;
    using UnityEngine;
    using UnityEngine.Networking;

    public sealed class GenericJoueur : Joueur
    {
        protected override IEnumerator FetchUserBG()
        {
            var Response = "";

            var www = UnityWebRequest.Get(
                Constants.API_URL + "/compte/watch/" + this.NomUtilisateur
            );
            www.certificateHandler = new BypassCertificate();
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                Response = www.downloadHandler.text;
                var unmarshal = JsonSerializer.Deserialize<JoueurAPICompteDTO>(Response);
                this.NomUtilisateur = unmarshal.username;
                this.IconeProfil = unmarshal.photo;
                this.Score = unmarshal.experience;
                this.Elo = unmarshal.elo;
                this.DateInscription = unmarshal.dateInscription;
                this.NombreParties = unmarshal.nombreParties;
                this.NombrePartiesGagnees = unmarshal.nombrePartiesGagnees;
                this.OnShapeChanged(EventArgs.Empty);
            }
            else
            {
                Debug.Log(www.error);
            }
        }

        public override void LoadSelf(MonoBehaviour Behaviour)
        {
            Behaviour.StartCoroutine(this.FetchUserBG());
        }
    }
}
