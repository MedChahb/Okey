using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Okey.Game;
using Okey.Joueurs;
using Okey.Tuiles;

namespace Okey
{
    internal class Start
    {
        public static void Main(string[] args)
        {
            Joueur[] Joueurs =
            {
                new Humain(1, "mohammed", 800),
                new Humain(2, "Emin", 100),
                new Humain(3, "toto", 2400),
                new Bot(50)
            };
            Jeu j = new Jeu(1, Joueurs);

            //Console.WriteLine("la tuile du centre : " + j.GetTuileCentre());

            j.DistibuerTuile(); // on commence

            //le joueur pioche
            if (j.getJoueurActuel().CountTuileDansChevalet() == 14)
            {
                //if(defausse[i-1 est vide])
                //acces a piocher centre true
                //piocher true
                // reset le timer

                // if(timer ==0){
                //     piocherTuile(random);
                // }
                Console.WriteLine("Choisis de piocher entre defausse et centre:");
                string TuilePiochee = Console.ReadLine();
                Console.WriteLine(TuilePiochee);
            }
            //le joueur jette
            if (j.getJoueurActuel().CountTuileDansChevalet() == 15)
            {
                //acces a jeter true
                // reset le timer
                // if(timer ==0){
                //     jeterTuile(random);
                //ChangerTour();
                // }
            }

            Joueur joueurCommence = j.getJoueurActuel();
            Tuile tuileTest = joueurCommence.GetChevalet()[0][0];
            //Tuile tuileTest2 = j.getPreviousPlayer(joueurCommence).GetChevalet()[0][0];

            Console.WriteLine("le joueur qui commence " + joueurCommence+"\n");
            j.AfficheChevaletJoueur();
            joueurCommence.AfficheDefausse();

            Console.WriteLine("------------------------------------------TEST JETER----------------------------------------------");

            //la piece jeté est plus dans son chevalet -> mais dans sa defausse
            Console.WriteLine($"le joueur {joueurCommence} jette la Tuile {tuileTest}\n");
            joueurCommence.JeterTuile(tuileTest, j);
            j.AfficheChevaletJoueur();
            joueurCommence.AfficheDefausse();

            /*Console.WriteLine("------------------------------------------TEST PIOCHER(centre)----------------------------------------------");
            j.AffichePiocheCentre();

            Joueur joueurActuel = j.getJoueurActuel();
            Console.WriteLine($"\nC'est à {joueurActuel} de joueur.\n");
            joueurActuel.PiocherTuile("Centre", j);
            j.AffichePiocheCentre();
            joueurActuel.AfficheChevalet();*/

            //!!!!!! on doit faire un SHUFFLE du pioche au centre !!!!!!!!!

            /*Console.WriteLine("------------------------------------------TEST PIOCHER(defausse)----------------------------------------------");
            Joueur joueurActuel = j.getJoueurActuel();
            Joueur joueurPrecedent = j.getPreviousPlayer(joueurActuel);

            Console.WriteLine($"\nC'est à {joueurActuel} de joueur.\n");

            Console.WriteLine($"{joueurActuel} a pioché de : ");
            joueurPrecedent.AfficheDefausse();
            joueurActuel.PiocherTuile("Defausse", j);
            joueurPrecedent.AfficheDefausse();

            joueurActuel.AfficheChevalet();*/

        }


    }


}
