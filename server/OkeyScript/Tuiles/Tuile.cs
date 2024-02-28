using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public enum CouleurTuile
{
    Jaune, 
    Noir, 
    Rouge, 
    Bleu, 
    Multi
}

namespace Okey.Tuiles
{
    public abstract class Tuile
    {
        protected CouleurTuile Couleur;
        protected int valeur;
        protected bool defausse;
        protected bool DansPioche;
    
        public Tuile(CouleurTuile couleur, int valeur, bool dansPioche)
        {
            this.Couleur = couleur;
            this.valeur = valeur;
            this.defausse = false;
            this.DansPioche = dansPioche;
        }

        public abstract void Move();
        
        public void SetDefause()
        {
            this.defausse = true;
        }

        public Boolean isDefausse()
        {
            return this.defausse;
        }

        public Boolean inPioche()
        {
            return this.DansPioche;
        }

        public CouleurTuile GetCouleur() {  return this.Couleur; }
        public int GetValeur() { return this.valeur; }



        public override abstract String ToString();
    }
}
