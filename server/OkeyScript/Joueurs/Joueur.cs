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

        public Joueur(int id, String Name)
        {
            this.id = id;
            this.Name = Name;
            this.Gagnant = false;
            this.Tour = false;

            //initialisation du chevalet (ready to get Tuiles)
            for (int i = 0; i < 2; i++)
            {
                this.chevalet.Add(new List<Tuile>());

                for (int j = 0; j < 8; j++)
                {
                    this.chevalet[i].Add(null);
                }
            }
        }

        public abstract void Gagne();

        public void VerifChevalet()
        {
            //chech for series then
            this.Gagne();
        }

        public void PassTour()
        {
            this.Tour = false;
        }

        public void EstTour()
        {
            this.Tour = true;
        }

        public void EstPlusTour()
        {
            this.Tour = false;
        }

        public void AjoutToChevalet(Tuile t) { }

        public List<List<Tuile>> GetChevalet()
        {
            return this.chevalet;
        }

        public void Ajouer()
        {
            this.Tour = true;
        }

        //returns a boolean if exists and the index of the list where it does [0-1]
        private (int, int) FindTuileInChevalet(Tuile t)
        {
            for (int i = 0; i < 2; i++)
            {
                for(int j=0; j < 8; j++)
                {
                    if (this.chevalet[i][j] == t)
                    {
                        return (i, j);
                    }
                }
            }
            return (-1,-1);
        }

        public void AjoutTuileChevalet(Tuile t)
        {
            for (int j = 0; j < 2; j++)
            {
                for (int i = 0; i < 8; i++)
                {
                    if (this.chevalet[j][i] == null)
                    {
                        this.chevalet[j][i] = t;
                        return;
                    }
                }
            }
        }

        public void JeterTuile(Coord c, Jeu j) // a discuter les parametres
        {
            //bloquer la pioche (condition lors de l'appel en Jeu.cs)
            //gerer le timer
            
            int x = c.getX();
            int y = c.getY();
            if ((x>=0 && x<=7) && (y==0 || y==1))
            {
                Tuile t = this.chevalet[y][x];
                this.chevalet[y][x] = null; // enlever la tuile du chevalet
                t.SetDefause(); // devient defausse
                this.JeteTuileDefausse(t); // la poser dans la defausse
                this.EstPlusTour(); // plus son tour
                j.setJoueurActuel(j.getNextJoueur(this)); // passer le tour an next jouer

                Console.WriteLine($"le joueur a jeté la tuile {t}\n");
            }
            else
            {
                Console.WriteLine("error in JeterTuile. (Tuile pas dans le chevalet)");
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

        public void MoveTuileChevalet(Coord from, Coord to)
        {
            (int yFrom, int xFrom) = (from.getY(), from.getX());
            (int yTo, int xTo) = (to.getY(), to.getX());

            Tuile tmpTuileTo = this.chevalet[to.getY()][to.getX()];

            this.chevalet[yTo][xTo] = this.chevalet[yFrom][xFrom];
            this.chevalet[yFrom][xFrom] = tmpTuileTo;
        }

        public int CountTuileDansChevalet()
        {
            int res = 0;
            for (int j = 0; j < 2; j++)
            {
                for (int i = 0; i < 8; i++)
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
            Console.WriteLine($"Chevalet du {this} : ");
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Tuile t = this.chevalet[i][j];
                    if (t != null)
                        Console.Write("|" + t);
                    else
                        Console.Write("|(        )");
                }
                Console.Write("|\n");
            }
        }
    }
}
