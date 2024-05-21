using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// Méthodes d'animations à appeler au moment voulu dans le code
public class Animation : MonoBehaviour
{
    private Animator animator;

    // Method called when the script instance is being loaded
    void Start()
    {
        // Initialiser l'Animator en récupérant le composant sur le même GameObject
        animator = GetComponent<Animator>();

        // Vérifier si l'Animator est trouvé
        if (animator == null)
        {
            Debug.LogError("Animator component not found on the GameObject.");
        }
    }

    // Animation distribution des tuiles et pose du joker
    public void DistributionTuiles()
    {
        if (animator != null)
        {
            animator.SetTrigger("distribution");
        }
    }

    // Joueur à droite :
    // Animation pioche joueur_droite (de la pioche centrale ou defaussedroite)
    public void PiocheJoueurDroite()
    {
        if (animator != null)
        {
            animator.SetTrigger("droitepioche");
        }
    }

    // Animation jet joueur_droite (pioche WIN ou defaussehautdroite)
    public void JetJoueurDroite()
    {
        if (animator != null)
        {
            animator.SetTrigger("droitejet");
        }
    }

    // Joueur en haut :
    // Animation pioche joueur_haut (de la pioche centrale ou defaussehautdroite)
    public void PiocheJoueurHaut()
    {
        if (animator != null)
        {
            animator.SetTrigger("hautpioche");
        }
    }

    // Animation jet joueur_haut (pioche WIN ou defaussehautgauche)
    public void JetJoueurHaut()
    {
        if (animator != null)
        {
            animator.SetTrigger("hautjet");
        }
    }

    // Joueur à gauche :
    // Animation pioche joueur_gauche (de la pioche centrale ou defaussehautgauche)
    public void PiocheJoueurGauche()
    {
        if (animator != null)
        {
            animator.SetTrigger("gauchepioche");
        }
    }

    // Animation jet joueur_gauche (pioche WIN ou defaussegauche)
    public void JetJoueurGauche()
    {
        if (animator != null)
        {
            animator.SetTrigger("gauchejet");
        }
    }

    // Animation victoire pour joueur qui a gagné
    public void Victoire()
    {
        if (animator != null)
        {
            animator.SetTrigger("victoire");
        }
    }
}
