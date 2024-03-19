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

            j.DistibuerTuile(); // on commence
            Console.WriteLine("Tuiles distribués.\n");

            Joueur joueurStarter = j.getJoueurActuel();
            j.AfficheChevaletActuel();

            Console.Write("choisis la tuile à jeter (donner ces coords y x) : ");

            Coord coords = readCoord(Console.ReadLine());
            joueurStarter.JeterTuile(coords, j);


            bool doitJete = false;

            while (!j.isTermine())
            {
                Joueur joueurActuel = j.getJoueurActuel();

                Console.WriteLine($"\nc'est le tour de {joueurActuel.getName()}:");
                j.AfficheChevaletActuel();

                //le joueur pioche
                if (!doitJete)
                {
                    Console.Write("choisis de où piocher (Centre ou Defausse) : ");
                    String ouPiocher = Console.ReadLine();

                    if (ouPiocher == "Move")
                    {
                        MoveInLoop(joueurActuel, j);
                        continue;
                    }
                    joueurActuel.PiocherTuile(ouPiocher, j);

                    Console.WriteLine("\nmaintenant vous devez jeter une tuile.");
                    j.AfficheChevaletActuel();
                }

                //le joueur jete

                Console.Write("choisis la tuile à jeter (donner ces coords y x) : ");
                String coordStr = Console.ReadLine();
                if (coordStr == "Move")
                {
                    MoveInLoop(joueurActuel, j);
                    doitJete = true;
                    continue;
                }
                Coord coordos = readCoord(coordStr);
                joueurActuel.JeterTuile(coordos, j);

                doitJete = false;

            }


            // TODO: 
            // move for JouerStarter (not essential but can do)
            // add timer (alerts)


            /*int seconds = 0;
            Timer timer = new Timer(state =>
            {
                Console.Clear();
                Console.WriteLine("Timer: " + TimeSpan.FromSeconds(seconds));
                seconds++;
            }, null, TimeSpan.Zero, TimeSpan.FromSeconds(1));

            Console.WriteLine("Press any key to stop the timer.");
            Console.ReadKey();
            timer.Dispose();*/

        }


        public static Coord readCoord(String str)
        {
            string[] parts = str.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            return new Coord(int.Parse(parts[0]), int.Parse(parts[1]));
        }

        public static void MoveInLoop(Joueur pl, Jeu j)
        {
            Console.Write("Donner les coords de la tuile à deplacer (y x): ");
            Coord from = readCoord(Console.ReadLine());
            Console.Write("Donner les coords d'où la mettre (y x): ");
            Coord to = readCoord(Console.ReadLine());
            pl.MoveTuileChevalet(from, to, j);
        }
    }


}
