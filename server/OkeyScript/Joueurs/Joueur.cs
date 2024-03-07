using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public void Piocher() { }

        public void JeterTuile(Tuile T) { }

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

        public abstract override String ToString();
    }
}
