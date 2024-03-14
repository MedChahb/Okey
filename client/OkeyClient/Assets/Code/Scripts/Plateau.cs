using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plateau : MonoBehaviour
{
    private string theme;

    //public List<Chevalet> chevalets = new List<Chevalet>();
    private Stack<Tuile> pilePioche = new Stack<Tuile>();
    private Tuile jocker;

    //Methode pour definir le themes
    public void setTheme(string type)
    {
        theme = type;
    }

    //Methode pour obtenir la tuile jocker
    public Tuile getJocker()
    {
        return jocker;
    }

    //methode pour definir la tuile joker
    public void setJoker(Tuile JockerTile)
    {
        jocker = JockerTile;
    }

    // Methode pour depiler la pile pioche
    /*
    public List<Chevalet> getChevalet()
    {
        return chevalets;
    }
    */

    // Méthode pour initialiser le plateau de jeu
    public void InitialiserPlateau(string theme, Stack<Tuile> pioche, Tuile joker)
    {
        setTheme(theme);
        pilePioche = pioche;
        setJoker(joker);
        //this.chevalets = chevalets;
    }
}
