using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Okey.Game;
using Okey.Tuiles;

namespace Okey.Joueurs
{
    public abstract class Joueur
    {
        protected int id;
        protected String Name;
        protected List<List<Tuile>> chevalet = new List<List<Tuile>>(); // 15tuile + 1case vide
        public Boolean Tour;
        protected Boolean Gagnant;

        protected bool peut_piocher = false;
        protected bool peut_jeter = false;
        protected Stack<Tuile> defausse = new Stack<Tuile>();

        static int etage = 2;
        static int tuilesDansEtage = 14; //14

        public Joueur(int id, String Name)
        {
            this.id = id;
            this.Name = Name;
            this.Gagnant = false;
            this.Tour = false;

            //initialisation du chevalet (ready to get Tuiles)
            for (int i = 0; i < etage; i++)
            {
                this.chevalet.Add(new List<Tuile>());

                for (int j = 0; j < tuilesDansEtage; j++)
                {
                    this.chevalet[i].Add(null);
                }
            }
        }

        public abstract void Gagne();
        public abstract void UpdateElo();

        public void EstTour()
        {
            this.Tour = true;
        }

        public void EstPlusTour()
        {
            this.Tour = false;
        }

        public void AjoutToChevalet(Tuile t) { }

        public void Ajouer()
        {
            this.Tour = true;
        }

        public void AjoutTuileChevalet(Tuile t)
        {
            for (int j = 0; j < etage; j++)
            {
                for (int i = 0; i < tuilesDansEtage; i++)
                {
                    if (this.chevalet[j][i] == null)
                    {
                        this.chevalet[j][i] = t;
                        return;
                    }
                }
            }
        }

        public void JeterTuile(Coord c, Jeu j)
        {
            //bloquer la pioche (condition lors de l'appel en Jeu.cs)
            //gerer le timer
            
            int x = c.getX();
            int y = c.getY();
            if ((x>=0 && x< tuilesDansEtage) && (y>=0 || y<etage))
            {
                Tuile t = this.chevalet[y][x];
                this.chevalet[y][x] = null; // enlever la tuile du chevalet
                t.SetDefause(); // devient defausse
                this.JeteTuileDefausse(t); // la poser dans la defausse
                this.EstPlusTour(); // plus son tour
                j.setJoueurActuel(j.getNextJoueur(this)); // passer le tour an next joueur

                Console.WriteLine($"{this.Name} a jeté la tuile {t}\n");
            }
            else
            {
                Console.WriteLine("error in JeterTuile. (coords invalides)");
            }

            if (this.VerifSerieChevalet())
            {
                j.JeuTermine();
                this.Gagne();
            }
        }

        public void PiocherTuile(String OuPiocher, Jeu j)
        {
            if(OuPiocher == null) { Console.WriteLine("ouPiocher est null"); return; }

            if(OuPiocher == "Centre")
            {
                if (j.isPiocheCentreEmpty())
                {
                    Console.WriteLine("La pile au centre est vide, impossible a piocher");
                }
                else
                {
                    Tuile tuilePiochee = j.PopPiocheCentre();
                    this.AjoutTuileChevalet(tuilePiochee);
                }
            }
            else if (OuPiocher == "Defausse")
            {
                Joueur PreviousPlayer = j.getPreviousPlayer(this);

                if (PreviousPlayer.isDefausseEmpty())
                {
                    Console.WriteLine("La defausse du joueur est vide, impossible a piocher");
                }
                else
                {
                    Tuile tuilePiochee = PreviousPlayer.PopDefausseJoueur();
                    this.AjoutTuileChevalet(tuilePiochee);
                }
            }

        }

        public void JoueurJoue(String OuPiocher, Coord c , Jeu j)
        {
            if (this.Tour)
            {
                this.PiocherTuile(OuPiocher, j);
                this.JeterTuile(c , j);
            }

        }

        public void MoveTuileChevalet(Coord from, Coord to, Jeu j)
        {
            (int yFrom, int xFrom) = (from.getY(), from.getX());
            (int yTo, int xTo) = (to.getY(), to.getX());

            Tuile tmpTuileTo = this.chevalet[to.getY()][to.getX()];

            this.chevalet[yTo][xTo] = this.chevalet[yFrom][xFrom];
            this.chevalet[yFrom][xFrom] = tmpTuileTo;

            if (this.CountTuileDansChevalet() == 14 && this.VerifSerieChevalet())
            {
                j.JeuTermine();
                this.Gagne();
            }
        }

        private bool Est_serie_de_meme_chiffre(List<Tuile> tuiles)
        {
            if (tuiles.Count <= 2) return false;
            List<CouleurTuile> CouleurVues = new List<CouleurTuile>();

            foreach (Tuile tuile in tuiles)
            {
                if (tuile is Okay) continue;
                if (tuiles[0].GetNum() != tuile.GetNum()) return false; // deux tuiles pas mm num -> false

                if (CouleurVues.Contains(tuile.GetCouleur())) return false; // deux tuile de mm couleur -> false
                                                                            // renvoie false automatiquement si y a > 4 Tuiles
                                                                            // car il ya que 4 couleurs
                CouleurVues.Add(tuile.GetCouleur()); // on memorise la couleur vue

            }

            return true;
        }

        private bool EstSerieDeCouleur(List<Tuile> t) // tuile se suivent, mm couleur
        {
            if (t.Count < 3)
                return false;
            for (int i = 0; i < t.Count - 1; i++)
            {
                int j = i + 1;

                if (!t[i].MemeCouleur(t[j]) || !t[i].estSuivant(t[j]))
                    return false;
            }
            return true;
        }

        public bool Est_serie(List<Tuile> tuiles)
        {
            return Est_serie_de_meme_chiffre(tuiles) || EstSerieDeCouleur(tuiles);
        }
        
        public static List<List<Tuile>> PartitionListOnNulls(List<Tuile> etage)
        {
            List<List<Tuile>> partitions = new List<List<Tuile>>();
            List<Tuile> partition = new List<Tuile>();

            foreach (Tuile t in etage)
            {
                if(t == null)
                {
                    if(partition.Count != 0)
                        partitions.Add(partition);
                    partition = new List<Tuile>();
                }
                else
                {
                    partition.Add(t);
                }

            }
            if (partition.Count != 0)
                partitions.Add(partition);
            return partitions;
        }
        
        public bool VerifSerieChevalet()
        {
            List<List<Tuile>> ParitionEtage1 = PartitionListOnNulls(this.chevalet[0]);
            List<List<Tuile>> ParitionEtage2 = PartitionListOnNulls(this.chevalet[1]);

            foreach (List<Tuile> part in ParitionEtage1)
            {
                if (!Est_serie(part)) return false;
            }

            foreach (List<Tuile> part in ParitionEtage2)
            {
                if (!Est_serie(part)) return false;
            }

            return true;
        }

        public int CountTuileDansChevalet()
        {
            int res = 0;
            for (int j = 0; j < etage; j++)
            {
                for (int i = 0; i < tuilesDansEtage; i++)
                {
                    if (this.chevalet[j][i] != null)
                    {
                        res++;
                    }
                }
            }
            return res;
        }

        public void EnvoyerMessage(string message)
        {
            // Utilisez ici votre mécanisme réel de communication avec le client Unity (WebSocket, HTTP, etc.)
            Console.WriteLine($"Envoi du message au joueur {id}: {message}");
            // Exemple avec WebSocket :
            // webSocketConnection.Send(message);
        }

        public void EnvoyerMessageTour(bool TourActuel)
        {
            string message = TourActuel ? "C'est votre tour." : "Ce n'est pas votre tour.";
            EnvoyerMessage(message);
        }

        public void JeteTuileDefausse(Tuile t)
        {
            this.defausse.Push(t);
        }

        public Tuile PopDefausseJoueur()
        {
            return this.defausse.Pop();
        }

        public bool isDefausseEmpty()
        {
            return this.defausse.Count == 0;
        }

        public Stack<Tuile> GetDefausse()
        {
            return this.defausse;
        }
        public abstract override String ToString();
        public void AfficheDefausse()
        {
            if (this.isDefausseEmpty())
            {
                Console.WriteLine("La defausse est vide.");
                return;
            }

            Console.WriteLine($"La defausse du {this} contient : ");
            foreach (var elem in this.defausse)
            {
                Console.WriteLine(elem);
            }
        }
   
        public void AfficheChevalet()
        {
            Console.WriteLine($"\nLe chevalet de {this} : ");
            for (int i = 0; i < etage; i++)
            {
                for (int j = 0; j < tuilesDansEtage; j++)
                {
                    Tuile t = this.chevalet[i][j];
                    if (t != null)
                        Console.Write("|" + t);
                    else
                        Console.Write("|(         )");
                }
                Console.Write("|\n");
            }
        }
    
        //pour tester VerifChevalet()
        public void setChevalet(List<List<Tuile>> chev)
        {
            this.chevalet = chev;
        }

        public String getName() { return this.Name; }

        public List<List<Tuile>> GetChevalet()
        {
            return this.chevalet;
        }

    }
}
