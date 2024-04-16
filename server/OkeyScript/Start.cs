using Okey.Game;
using Okey.Joueurs;
using System.Timers;

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

            static Timer timer;
            static bool isRunning = false;
            timer = new Timer(30000);

            // Définir l'événement à déclencher lorsque le timer expire
            timer.Elapsed += TimerElapsed;

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

                // Démarrer le timer
                StartTimer();

                Console.WriteLine($"\nc'est le tour de {joueurActuel?.getName()}:");
                j.AfficheChevaletActuel();


                //le joueur pioche
                if (!doitJete)
                {
                    Console.Write("choisis de où piocher ('Centre' ou 'Defausse') ou 'Move': ");
                    String? ouPiocher = Console.ReadLine();


                    if(timer==0){
                        ouPiocher = "Centre";
                    }

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
                ResetTimer();

                Console.Write("choisis la tuile à jeter (donner ces coords y x) ou taper 'Move' ou 'Gagner': ");
                String? coordStr = Console.ReadLine();

                //si le timer finit on jete une tuile aleatoire
                if(timer ==0){
                    Coord JeterTuileAlea =joueurActuel?.GetRandomTuileCoords();
                    joueurActuel?.JeterTuile(JeterTuileAlea, j);
                    Console.Write("Tuile aleatoire jetee");
                    Console.Write($"Tuile jetee de coordonnees : ({JeterTuileAlea.GetY()}, {JeterTuileAlea.GetX()})");
                }

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
                else
                {
                    Coord coordos = readCoord(coordStr);
                    joueurActuel?.JeterTuile(coordos, j);
                }
                StopTimer();
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
    public static void StartTimer()
    {
        if (!isRunning)
        {
            timer.Start();
            isRunning = true;
            Console.WriteLine("Timer started.");
        }
    }

    public static void StopTimer()
    {
        if (isRunning)
        {
            timer.Stop();
            isRunning = false;
            Console.WriteLine("Timer stopped.");
        }
    }
    public static void ResetTimer()
    {
        StopTimer();
        timer.Dispose(); // Libérer les ressources du timer existant
        timer = new Timer(30000); // Recréer le timer avec la même durée
        timer.Elapsed += TimerElapsed; // Réattacher l'événement Elapsed
        StartTimer(); // Redémarrer le timer
    }

    public static void TimerElapsed(object sender, ElapsedEventArgs e)
    {
        // Action à effectuer lorsque le timer expire
        Console.WriteLine("Timer expired after 30 seconds.");
        StopTimer();
    }

}
