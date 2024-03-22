using System;

public enum EtatTourJoueurEnum
{
    Attente,
    Pioche,
    Pose,
    Fin
}

public class EtatTourJoueur
{
    public EtatTourJoueurEnum EtatTour { get; private set; }

    public EtatTourJoueur() => this.EtatTour = EtatTourJoueurEnum.Attente;

    public EtatTourJoueurEnum AvancerTour()
    {
        switch (this.EtatTour)
        {
            case EtatTourJoueurEnum.Attente:
                this.EtatTour = EtatTourJoueurEnum.Pioche;
                break;
            case EtatTourJoueurEnum.Pioche:
                this.EtatTour = EtatTourJoueurEnum.Pose;
                break;
            case EtatTourJoueurEnum.Pose:
                this.EtatTour = EtatTourJoueurEnum.Fin;
                break;
            case EtatTourJoueurEnum.Fin:
                throw new IndexOutOfRangeException(
                    "Fin de la tour, il n'est plus possible de l'avancer."
                );
            default:
                break;
        }
        return this.EtatTour;
    }

    public EtatTourJoueurEnum ResetTour()
    {
        return this.EtatTour = EtatTourJoueurEnum.Attente;
    }
}
