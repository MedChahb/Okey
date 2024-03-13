using System;
using System.Collections;
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
        //private CircularLinkedList<Joueur> Joueurs = new CircularLinkedList<Joueur>(); //taille 4
        private Joueur[] Joueurs = new Joueur[4];
        //le joueur jette dans sa defausse et prend de la defausse du joueur precedent
        
        
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

        public Jeu(int id, Joueur[] joueurs)
        {
            this.id = id;

            /*foreach(var pl in joueurs)
            {
                this.Joueurs.Add(pl);
            }*/

            this.Joueurs = joueurs;
            this.MMR = CalculMMR();
            this.etat = false;
            (this.PacketTuile, this.TuileCentre) = GenererPacketTuiles();
        }

        private double CalculMMR()
        {
            return 5.3; // donner la formule en fonction des 4 joueurs
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

            this.Joueurs[randT % 4].AjoutTuileChevalet(LastTuileTogive);
            this.Joueurs[randT % 4].Ajouer(); // qui recoit la 15 Tuile jouera le premier

            // ce qui reste dans PacketTuile -> this.Pioche 
            foreach (Tuile tuile in this.PacketTuile)
            {
                this.pioche.Push(tuile);
            }

            this.JoueurActuel = this.Joueurs[randT % 4];
        }

        public void AfficheChevaletJoueur()
        {
            //CircularLinkedList<Joueur> joueurs = this.GetJoueurs();
            
            foreach (Joueur pl in this.Joueurs)
            {
                pl.AfficheChevalet();
                Console.WriteLine("");
            }
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

        public void JeterTuile(Tuile tuile)
        {
            //jeter une tuile

            JeterTuileAppelee = true;
        }

        /*public void PiocherTuile(string Apiochee)
        {
            ArgumentNullException.ThrowIfNull(Apiochee);

            if (Apiochee == "defausse")
            {
                if ()//defausse du joueur a gauche en argument.Count==0
                {
                    Console.WriteLine("La pile de défaisse du joueur à gauche est vide.Piocher au centre");
                }
                else
                {
                    JoueurActuel.PiocherTuile();//defausse du joueur a gauche en argument;
                }
            }
            if (Apiochee == "centre")
            {
                JoueurActuel.PiocherTuile(pioche);
            }
        }*/



        /*public void ChangerTour()
        {
            // Le joueur actuel n'a plus le tour
            JoueurActuel.EstPlusTour();

            // Trouver l'index du joueur actuel dans la liste
            int indexJoueurActuel = Array.IndexOf(Joueurs, JoueurActuel);

            // Choisir le joueur suivant
            int indexJoueurSuivant = (indexJoueurActuel + 1) % joueurs.Length;
            Joueur joueurSuivant = joueurs[indexJoueurSuivant];

            // Le joueur suivant a maintenant le tour
            joueurSuivant.EstTour();
            this.JoueurActuel = joueurSuivant;

            // Après avoir changé le tour, signalez le changement aux joueurs
            SignalChangementTour(joueurSuivant);
        }*/

        public void SignalChangementTour(Joueur joueurTour)
        {
            foreach (var joueur in Joueurs)
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

        /*public CircularLinkedList<Joueur> GetJoueurs()
        {
            return this.Joueurs;
        }*/
        public Joueur[] GetJoueurs()
        {
            return Joueurs;
        }


        public Joueur getJoueurActuel()
        {
            return this.JoueurActuel;
        }

        public void setJoueurActuel(Joueur j)
        {
            this.JoueurActuel = j;
        }

        public Joueur getNextJoueur(Joueur j)
        {
            int indexOfNextPlayer = (Array.IndexOf(this.Joueurs, j)+1)%4;
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

        public Tuile PopPiocheCentre()
        {
            return this.pioche.Pop();
        }

    }

    public class CircularLinkedList<T> : IEnumerable<T>
    {
        private LinkedList<T> list;
        private LinkedListNode<T> current;

        public CircularLinkedList()
        {
            list = new LinkedList<T>();
            current = null;
        }

        public void Add(T item)
        {
            list.AddLast(item);
            if (current == null)
                current = list.First;
        }

        public T GetCurrent()
        {
            if (current == null)
                throw new InvalidOperationException("List is empty");
            return current.Value;
        }

        public void Next()
        {
            if (current == null)
                throw new InvalidOperationException("List is empty");
            current = current.Next ?? list.First;
        }

        public void Previous()
        {
            if (current == null)
                throw new InvalidOperationException("List is empty");
            current = current.Previous ?? list.Last;
        }

        public T this[int index]
        {
            get
            {
                if (index < 0 || index >= list.Count)
                    throw new IndexOutOfRangeException("Index is out of range.");

                LinkedListNode<T> node = list.First;
                for (int i = 0; i < index; i++)
                {
                    node = node.Next;
                }
                return node.Value;
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            LinkedListNode<T> startingNode = current;
            if (startingNode != null)
            {
                do
                {
                    yield return startingNode.Value;
                    startingNode = startingNode.Next ?? list.First;
                } while (startingNode != current);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

}
