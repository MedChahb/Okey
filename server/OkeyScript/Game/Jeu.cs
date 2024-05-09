using Okey.Joueurs;
using Okey.Tuiles;

namespace Okey.Game
{
    public class Jeu
    {
        private int id;
        private Joueur[] Joueurs = new Joueur[4];

        //private double MMR;
        private Stack<Tuile> pioche = new Stack<Tuile>();
        private bool etat; // false : in game
        private Tuile[] Jokers = new Joker[2];
        private Tuile[] Okays = new Okay[2];
        private List<Tuile> PacketTuile = new List<Tuile>();
        private readonly Tuile TuileCentre;
        private Joueur? JoueurActuel;

        //Liste qui affichera toutes les tuiles jetés
        public List<Tuile> ListeDefausse { get; private set; }

        public Jeu(int id, Joueur[] joueurs)
        {
            this.id = id;
            this.Joueurs = joueurs;
            //this.MMR = CalculMMR();
            this.etat = false;
            (this.PacketTuile, this.TuileCentre) = GenererPacketTuiles();
            this.ListeDefausse = new List<Tuile>();
        }

        /*private double CalculMMR()
        {
            return 5.3; // donner la formule en fonction des 4 joueurs
        }*/

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
                    tableauTuiles[i] = okay;
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

        public void DistibuerTuile()
        {
            for (int i = 0; i < 14; i++)
            {
                foreach (Joueur pl in this.Joueurs)
                {
                    Random random = new Random();
                    int randIndex = random.Next(0, this.PacketTuile.Count - 1); // on prend en random l'index de la tuile du packet

                    Tuile toGive = this.PacketTuile[i]; // on la save dans toGive

                    this.PacketTuile.RemoveAt(i); // on la supprime du packet
                    pl.AjoutTuileChevalet(toGive); // on la donne au joueur (ajout à son chevalet)

                    //faire ça 14 fois pour les 4 joueurs
                }
            }
            //on donne la 15eme Tuile au joueur à jouer
            Random randomm = new Random();
            int randT = randomm.Next(0, this.PacketTuile.Count - 1);
            Tuile LastTuileTogive = this.PacketTuile[randT];
            this.PacketTuile.RemoveAt(randT);

            this.Joueurs[randT % 4].AjoutTuileChevalet(LastTuileTogive);
            this.Joueurs[randT % 4].Ajouer(); // qui recoit la 15 Tuile jouera le premier

            // ce qui reste dans PacketTuile -> this.Pioche
            foreach (Tuile tuile in this.PacketTuile)
            {
                this.pioche.Push(tuile);
            }

            //ici shuffle this.pioche
            ShuffleStack(this.pioche);

            this.JoueurActuel = this.Joueurs[randT % 4];
        }

        public void AfficheChevaletJoueurs()
        {
            foreach (Joueur pl in this.Joueurs)
            {
                pl.AfficheChevalet();
                Console.WriteLine("");
            }
        }

        public void AfficheChevaletActuel()
        {
            if (JoueurActuel != null)
                this.JoueurActuel.AfficheChevalet();
        }

        public string StringChevaletActuel()
        {
            if (this.JoueurActuel == null)
                return "";
            return this.JoueurActuel.StringChevalet();
        }

        public void AffichePiocheCentre()
        {
            if (this.isPiocheCentreEmpty())
            {
                Console.WriteLine("La pioche du centre est vide.");
                return;
            }

            Console.WriteLine($"La pioche du centre contient : ");
            foreach (var elem in this.pioche)
            {
                Console.WriteLine(elem);
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
            return Joueurs;
        }

        public Joueur? getJoueurActuel()
        {
            return this.JoueurActuel;
        }

        public bool isTermine()
        {
            return this.etat;
        }

        public Joueur getNextJoueur(Joueur j)
        {
            int indexOfNextPlayer = (Array.IndexOf(this.Joueurs, j) + 1) % 4;
            return this.Joueurs[indexOfNextPlayer];
        }

        public Joueur getPreviousPlayer(Joueur j)
        {
            int indexOfj = Array.IndexOf(this.Joueurs, j);
            int indexOfPreviousPlayer = indexOfj == 0 ? 3 : indexOfj - 1;
            return this.Joueurs[indexOfPreviousPlayer];
        }

        public bool isPiocheCentreEmpty()
        {
            return this.pioche.Count == 0;
        }

        public int GetPiocheTaille()
        {
            return this.pioche.Count;
        }

        public Tuile? PopPiocheCentre()
        {
            if (this.isPiocheCentreEmpty())
            {
                return null;
            }
            return this.pioche.Pop();
        }

        public Tuile? GetPiocheHead()
        {
            if (this.isPiocheCentreEmpty())
            {
                return null;
            }
            return this.pioche.Peek();
        }

        public void PushPiocheCentre(Tuile? t)
        {
            if (t == null)
                return;
            this.pioche.Push(t);
        }

        public void setJoueurActuel(Joueur j)
        {
            this.JoueurActuel = j;
        }

        public void JeuTermine()
        {
            this.etat = true;
        }

        static void ShuffleStack(Stack<Tuile> stack)
        {
            List<Tuile> list = stack.ToList();
            Random rng = new Random();
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                Tuile value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
            stack.Clear();
            foreach (Tuile item in list)
            {
                stack.Push(item);
            }
        }
    }
}
