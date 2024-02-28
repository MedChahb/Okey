using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Okey.Joueurs;
using Okey.Tuiles;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Okey.Game
{
    public class Jeu
    {
        private int id;
        private Joueur[] joueurs = new Joueur[4];
        private double MMR;
        private Stack<Tuile> pioche = new Stack<Tuile>();
        //Timer timer;
        private bool etat; // false : in game
        private Tuile[] Jokers = new Joker[2];
        private Tuile[] Okays = new Okay[2];
        private Tuile[] PacketTuile = new Tuile[105];
        private Tuile TuileCentre;
        public Jeu(int id, Joueur[] joueurs, Stack<Tuile> pioche)
        {
            this.id = id;
            this.joueurs = joueurs;
            this.MMR = CalculMMR();
            this.pioche = pioche;
            this.etat = false;
            (this.PacketTuile,this.TuileCentre) = GenererTableauTuiles();
        }
        private double CalculMMR()
        {
            return 5.2; // donner la formule en fonction des 4 joueurs
        }

        public (Tuile[],Tuile) GenererTableauTuiles()
        {
            Tuile[] tableauTuiles = new Tuile[106];

            int index = 0;

            // Générer 13 tuiles pour chaque numéro de 1 à 13 et chaque couleur
            for (int numero = 1; numero <= 13; numero++)
            {
                foreach (CouleurTuile couleur in Enum.GetValues(typeof(CouleurTuile)))
                {
                    if (couleur != CouleurTuile.Multi) // Éviter la couleur Multi pour les tuiles normales
                    {
                        // Créer une tuile normale
                        Tuile tuile = new TuileNormale(couleur, numero, true); // true pour dansPioche
                        Tuile tuile2 = new TuileNormale(couleur, numero, true);


                        if (index < tableauTuiles.Length)
                        {
                            tableauTuiles[index] = tuile;
                            index++;
                        }

                        if (index < tableauTuiles.Length)
                        {
                            tableauTuiles[index] = tuile2;
                            index++;
                        }
                    }
                }
            }

            // prendre La tuileCentre
            Random random = new Random();
            int RandIndex = random.Next(0, 103);
            Tuile TuileCentre = tableauTuiles[RandIndex];

            int numOkey =  (TuileCentre.GetValeur() == 13)? 1 : TuileCentre.GetValeur() + 1;
            CouleurTuile couleurOkey = TuileCentre.GetCouleur();

            int ok = 0;
            for (int i=0; i<103; i++)
            {
                if (tableauTuiles[i].GetValeur() == numOkey && tableauTuiles[i].GetCouleur() == couleurOkey)
                {
                    Okay okay = new Okay(true);
                    tableauTuiles[i] = okay; // genere deux okay ?
                    this.Okays[ok] = okay;
                    ok++;
                }     

            }


            Tuile joker1 = new Joker(couleurOkey, numOkey, true);
            tableauTuiles[RandIndex] = joker1;
            this.Jokers[0] = joker1;

            Tuile joker2 = new Joker(couleurOkey, numOkey, true);
            tableauTuiles[104] = joker2;
            this.Jokers[1] = joker2;

            return (tableauTuiles, TuileCentre);
        }


        public void DistibuerTuile() //à faire
        {

        }

        public Tuile[] GetPacketTuile() { return this.PacketTuile; }
        public Tuile GetTuileCentre() {  return this.TuileCentre; }
        public Tuile[] GetJokers() { return this.Jokers; }
        public Tuile[] GetOkays() { return this.Okays; }
    }


    
}