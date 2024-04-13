using System.Text.Json;
using Okey.Joueurs;

namespace Okey.Tuiles
{
    public enum CouleurTuile
    {
        J, //Jaune
        N, //Noir
        R, //Rouge
        B, //Bleu
        M //Mult
    }

    public abstract class Tuile
    {
        private CouleurTuile couleur;
        private int num;
        private bool defausse;
        private bool dansPioche;

        public Tuile(CouleurTuile couleur, int num, bool dansPioche)
        {
            this.couleur = couleur;
            this.num = num;
            this.defausse = false;
            this.dansPioche = dansPioche;
        }

        // Protected properties to allow access to private fields from child classes
        protected CouleurTuile Couleur
        {
            get => couleur;
            set => couleur = value;
        }
        protected int Num
        {
            get => num;
            set => num = value;
        }
        protected bool Defausse
        {
            get => defausse;
            set => defausse = value;
        }
        protected bool DansPioche
        {
            get => dansPioche;
            set => dansPioche = value;
        }

        public void SetDefause()
        {
            if (this == null)
                return;
            this.defausse = true;
        }

        public Boolean isDefausse()
        {
            return this.defausse;
        }

        public Boolean inPioche()
        {
            return this.dansPioche;
        }

        private string ToJSON(Tuile tuile)
        {
            // Créer une chaîne JSON à partir de la tuile
            string jsonString = JsonSerializer.Serialize(tuile);

            // Retourner la chaîne JSON
            return jsonString;
        }

        public CouleurTuile GetCouleur()
        {
            return this.couleur;
        }

        public int GetNum()
        {
            return this.num;
        }

        public abstract bool MemeCouleur(Tuile t);

        public abstract bool estSuivant(Tuile t);

        public abstract override String ToString();
    }
}
