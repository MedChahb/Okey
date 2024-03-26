namespace LogiqueJeu.Joueur
{
    using System;
    using System.Collections;
    using LogiqueJeu.Constants;
    using UnityEngine;
    using UnityEngine.Networking;

    public class API
    {
        public const string API_URL_DEV = "https://blablabla:5001/api";

        public static string FetchUser(string Username)
        {
            var Response = "";

            var www = UnityWebRequest.Get(Constants.API_URL_DEV + "/compte/watch/" + Username);
            www.certificateHandler = new BypassCertificate();
            www.SendWebRequest();

            while (!www.isDone) { }

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                Response = www.downloadHandler.text;
            }

            return Response;
        }

        public static IEnumerator FetchUser(string Username, Action<string> CallbackJSON)
        {
            var Response = "";

            var www = UnityWebRequest.Get(Constants.API_URL_DEV + "/compte/watch/" + Username);
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
}
