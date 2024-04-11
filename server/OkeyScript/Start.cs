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
    public class Start
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

            Joueur? joueurStarter = j.getJoueurActuel();
            j.AfficheChevaletActuel();

            Console.Write("choisis la tuile à jeter (donner ces coords y x) ou 'Move' : ");
            String? action = Console.ReadLine();

            while (string.Equals(action, "move", StringComparison.OrdinalIgnoreCase)) //action == "move"
            {
                MoveInLoop(joueurStarter, j);
                joueurStarter?.AfficheChevalet();
                Console.Write("choisis la tuile à jeter (donner ces coords y x) ou 'Move' : ");
                action = Console.ReadLine();
            }


            Coord coords = readCoord(action);
            joueurStarter?.JeterTuile(coords, j);


            bool doitJete = false;

            while (!j.isTermine())
            {
                Joueur? joueurActuel = j.getJoueurActuel();

                Console.WriteLine($"\nc'est le tour de {joueurActuel?.getName()}:");
                j.AfficheChevaletActuel();

                //le joueur pioche
                if (!doitJete)
                {
                    Console.Write("choisis de où piocher ('Centre' ou 'Defausse') ou 'Move': ");
                    String? ouPiocher = Console.ReadLine();


                    if (string.Equals(ouPiocher, "move", StringComparison.OrdinalIgnoreCase))
                    {
                        MoveInLoop(joueurActuel, j);
                        continue;
                    }

                    if (string.Equals(ouPiocher, "Centre", StringComparison.OrdinalIgnoreCase)
                        || string.Equals(ouPiocher, "Defausse", StringComparison.OrdinalIgnoreCase))
                    {
                        joueurActuel?.PiocherTuile(ouPiocher, j);
                        Console.WriteLine("\nmaintenant vous devez jeter une tuile.");
                        j.AfficheChevaletActuel();
                    }
                    else
                    { // s'il a pas taper centre ou defausse
                        continue;
                    }
                }

                //le joueur jete

                Console.Write("choisis la tuile à jeter (donner ces coords y x) ou taper 'Move' ou 'Gagner': ");
                String? coordStr = Console.ReadLine();
                if (string.Equals(coordStr, "move", StringComparison.OrdinalIgnoreCase))
                {
                    MoveInLoop(joueurActuel, j);
                    doitJete = true;
                    continue;
                }
                if (string.Equals(coordStr, "gagner", StringComparison.OrdinalIgnoreCase))
                {
                    Console.Write("donner les coords de la tuile que vous voulez finir avec : ");
                    String? coordsToFinish = Console.ReadLine();
                    Coord c = readCoord(coordsToFinish);
                    joueurActuel?.JeteTuilePourTerminer(c, j);
                    doitJete = true;
                    continue;
                }
                Coord coordos = readCoord(coordStr);
                joueurActuel?.JeterTuile(coordos, j);

                doitJete = false;

            }
        }

        internal static readonly char[] separator = new char[] { ' ' };
        public static Coord readCoord(String? str)
        {
            if (str == null) return new Coord(-1, -1); // renvoie un error (on doit garantir qu'on passera jamais ici)

            string[] parts = str.Split(separator, StringSplitOptions.RemoveEmptyEntries);

            return new Coord(int.Parse(parts[0], System.Globalization.CultureInfo.InvariantCulture),
                             int.Parse(parts[1], System.Globalization.CultureInfo.InvariantCulture));
        }

        public static void MoveInLoop(Joueur? pl, Jeu j)
        {
            Console.Write("Donner les coords de la tuile à deplacer (y x): ");
            Coord from = readCoord(Console.ReadLine());
            Console.Write("Donner les coords d'où la mettre (y x): ");
            Coord to = readCoord(Console.ReadLine());
            pl?.MoveTuileChevalet(from, to, j);
        }
    }


}
