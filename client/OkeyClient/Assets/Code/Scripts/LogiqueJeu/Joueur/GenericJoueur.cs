namespace LogiqueJeu.Joueur
{
    using System.IO;
    using System.Xml.Serialization;
    using LogiqueJeu.Constants;
    using UnityEngine;

    public sealed class GenericJoueur : Joueur
    {
        public override void LoadSelf(MonoBehaviour Behaviour)
        {
            if (!File.Exists(Application.persistentDataPath + Constants.SELF_PLAYER_SAVE_FILE))
            {
                return;
            }
            TextReader reader = null;
            try
            {
                var serializer = new XmlSerializer(typeof(GenericJoueur));
                reader = new StreamReader(
                    Application.persistentDataPath + Constants.SELF_PLAYER_SAVE_FILE
                );
                this.CopyFrom((GenericJoueur)serializer.Deserialize(reader));
            }
            finally
            {
                reader?.Close();
            }
            Behaviour.StartCoroutine(this.FetchUserBG(this.UnmarshalAndInit));
        }
    }
}
