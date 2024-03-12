using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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
        private List<Tuile> PacketTuile = new List<Tuile>();
        private Tuile TuileCentre;
        private bool JeterTuileAppelee = false;
        private Joueur JoueurActuel;
        private Tuile TuileJete;

        public Jeu(int id, Joueur[] joueurs, Stack<Tuile> pioche)
        {
            this.id = id;
            this.joueurs = joueurs;
            this.MMR = CalculMMR();
            this.pioche = pioche;
            this.etat = false;
            (this.PacketTuile, this.TuileCentre) = GenererPacketTuiles();
        }

        private double CalculMMR()
        {
            return 5.2; // donner la formule en fonction des 4 joueurs
        }

        private (List<Tuile>, Tuile) GenererPacketTuiles()
        {
            List<Tuile> tableauTuiles = new List<Tuile>();

            // Générer 13 tuiles pour chaque numéro de 1 à 13 et chaque couleur
            for (int numero = 1; numero <= 13; numero++)
            {
                foreach (CouleurTuile couleur in Enum.GetValues(typeof(CouleurTuile)))
                {
                    if (couleur != CouleurTuile.M) // Éviter la couleur Multi pour les tuiles normales
                    {
                        // Créer une tuile normale
                        Tuile tuile = new TuileNormale(couleur, numero, true);
                        tableauTuiles.Add(tuile);

                        Tuile tuile2 = new TuileNormale(couleur, numero, true);
                        tableauTuiles.Add(tuile2);
                    }
                }
            }

            // prendre La tuileCentre
            Random random = new Random();
            int randIndex = random.Next(0, 103);
            Tuile tuileCentre = tableauTuiles[randIndex];

            int numOkey = (tuileCentre.GetNum() == 13) ? 1 : tuileCentre.GetNum() + 1;
            CouleurTuile couleurOkey = tuileCentre.GetCouleur();

            int ok = 0;
            for (int i = 0; i < 103; i++)
            {
                if (
                    tableauTuiles[i].GetNum() == numOkey
                    && tableauTuiles[i].GetCouleur() == couleurOkey
                )
                {
                    Okay okay = new Okay(true);
                    tableauTuiles[i] = okay; // Do you want to replace the existing Tuile with Okay?
                    this.Okays[ok] = okay;
                    ok++;
                }
            }

            Tuile joker1 = new Joker(couleurOkey, numOkey, true);
            tableauTuiles[randIndex] = joker1;
            this.Jokers[0] = joker1;

            Tuile joker2 = new Joker(couleurOkey, numOkey, true);
            tableauTuiles.Add(joker2);
            this.Jokers[1] = joker2;

            return (tableauTuiles, tuileCentre);
        }

        public void DistibuerTuile() //à faire
        {
            for (int i = 0; i < 14; i++)
            {
                foreach (Joueur pl in this.joueurs)
                {
                    Random random = new Random();
                    int randIndex = random.Next(0, this.PacketTuile.Count - 1); // on prend en random l'index de la tuile du packet

                    Tuile toGive = this.PacketTuile[randIndex]; // on la save dans toGive

                    this.PacketTuile.RemoveAt(randIndex); // on la supprime du packet
                    pl.AjoutTuileChevalet(toGive); // on la donne au joueur (ajout à son chevalet)

                    //faire ça 14 fois pour les 4 joueurs
                }
            }
            //on donne la 15eme Tuile au joueur à jouer
            Random randomm = new Random();
            int randT = randomm.Next(0, this.PacketTuile.Count - 1);
            Tuile LastTuileTogive = this.PacketTuile[randT];
            this.PacketTuile.RemoveAt(randT);

            this.joueurs[randT % 4].AjoutTuileChevalet(LastTuileTogive);
            this.joueurs[randT % 4].Ajouer(); // qui recoit la 15 Tuile jouera le premier

            // ce qui reste dans PacketTuile -> this.Pioche ???
            //this.pioche = this.PacketTuile;

            this.JoueurActuel = this.joueurs[randT % 4];
        }

        public void AfficheChevaletJoueur()
        {
            Joueur[] joueurs = this.GetJoueurs();

            foreach (Joueur pl in joueurs)
            {
                Console.WriteLine(System.String.Format("Chevalet du {0} : ", pl));
                for (int i = 0; i < 2; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        Tuile t = pl.GetChevalet()[i][j];
                        if (t != null)
                            Console.Write("|" + t);
                        else
                            Console.Write("|(        )");
                    }
                    Console.Write("|\n");
                }
                Console.WriteLine("");
            }
        }

        public void JeterTuile(Tuile tuile)
        {
            //jeter une tuile

            JeterTuileAppelee = true;
        }

        public void FinTour(Joueur joueur)
        {
            if ( /*(Timer == 0) ||*/
                JeterTuileAppelee == true
            )
            {
                ChangerTour();
            }
        }

        public void ChangerTour()
        {
            // Le joueur actuel n'a plus le tour
            JoueurActuel.EstPlusTour();

            // Trouver l'index du joueur actuel dans la liste
            int indexJoueurActuel = Array.IndexOf(joueurs, JoueurActuel);

            // Choisir le joueur suivant
            int indexJoueurSuivant = (indexJoueurActuel + 1) % joueurs.Length;
            Joueur joueurSuivant = joueurs[indexJoueurSuivant];

            // Le joueur suivant a maintenant le tour
            joueurSuivant.EstTour();
            this.JoueurActuel = joueurSuivant;

            // Après avoir changé le tour, signalez le changement aux joueurs
            SignalChangementTour(joueurSuivant);
        }

        public void SignalChangementTour(Joueur joueurTour)
        {
            foreach (var joueur in joueurs)
            {
                // Envoie un message au joueur indiquant si c'est son tour
                joueur.EnvoyerMessageTour(joueur == joueurTour);
            }
        }

        public List<Tuile> GetPacketTuile()
        {
            return this.PacketTuile;
        }

        public Tuile GetTuileCentre()
        {
            return this.TuileCentre;
        }

        public Tuile[] GetJokers()
        {
            return this.Jokers;
        }

        public Tuile[] GetOkays()
        {
            return this.Okays;
        }

        public Joueur[] GetJoueurs()
        {
            return this.joueurs;
        }

        public void SetTuileJete(Tuile t)
        {
            this.TuileJete = t;
        }

        public Joueur getJoueurActuel()
        {
            return this.JoueurActuel;
        }
    }
}
