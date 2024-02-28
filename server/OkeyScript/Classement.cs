﻿using Okey.Joueurs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Okey
{
    internal class Classement
    {
        private LinkedList<Joueur> joueurs = new LinkedList<Joueur>();

        public void afficheClassement()
        {
            foreach (Joueur joueur in joueurs)
            {
                Console.WriteLine(joueur.toString()); // should be sorted
            }
        }

        public void updateClassement()
        {

        }

        public int GetRankPlayer(Joueur j) // -1 if error
        {
            if (this.joueurs.Contains(j) && !(j is Bot))
            {
                Humain H = (Humain)j; // cast, if not bot -> is humain
                return H.GetRank();
            }

            Console.WriteLine("Le joueur n'est pas sur le classement OU c'est un BOT.");
            return -1;
        }
    }
}
