using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//just pour l'affichage du chevalet, apres je les remets
public enum CouleurTuile
{
    J, //Jaune
    N, //Noir
    R, //Rouge
    B, //Bleu
    M  //Mult
}

namespace Okey.Tuiles
{
    public abstract class Tuile
    {
        protected CouleurTuile Couleur;
        protected int num;
        protected bool defausse;
        protected bool DansPioche;
    
        public Tuile(CouleurTuile couleur, int num, bool dansPioche)
        {
            this.Couleur = couleur;
            this.num = num;
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
        public int GetNum() { return this.num; }



        public override abstract String ToString();
    }
}
