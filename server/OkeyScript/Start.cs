using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Okey.Joueurs;
using Okey.Tuiles;
using Okey.Game;

namespace Okey
{
    internal class Start
    {
        public static void Main(string[] args)
        {
            Joueur[] Joueurs = { new Humain(1, "mohammed", 800), new Humain(2, "Emin", 100), new Bot(100), new Bot(50) };
            Jeu j = new Jeu(1, Joueurs, new Stack<Tuile>());

            /*Console.WriteLine("la tuile du centre : " + j.GetTuileCentre());

            Console.WriteLine(j.GetJokers()[0]);
            Console.WriteLine(j.GetJokers()[1]);
            // cbien aleatoire

            Console.WriteLine(j.GetOkays()[0]);
            Console.WriteLine(j.GetOkays()[1]);
            //
            Console.WriteLine("");
            for (int i = 0; i < 105; i++)
            {
                Console.WriteLine(i + 1 + " : " + j.GetPacketTuile()[i]);
            }*/
            //105 tuile dans le packet(avec joker et okay) + la Tuile au centre -> (pas de redondance)

            

            j.AfficheChevaletJoueur(); Console.WriteLine(j.GetPacketTuile().Count); // 105 tuiles apres la distribution
            j.DistibuerTuile(); // on commence

            Console.WriteLine(j.getJoueurActuel());
            j.AfficheChevaletJoueur(); Console.WriteLine(j.GetPacketTuile().Count); // 48 tuiles avant la disibution
            // jouer qui a 15tuile est bien c'est tour à jouer



            // ajouter une classe Joueur pour les defausse + ordonance des tours
        }
    }
}
