using System;

public enum EtatTourInterneEnum
{
    Attente,
    Pioche,
    Pose,
    Fin
}

public class EtatTourInterne
{
    public EtatTourInterneEnum EtatTour { get; private set; }

    public EtatTourInterne() => this.EtatTour = EtatTourInterneEnum.Attente;

    public void AvancerTour()
    {
        switch (this.EtatTour)
        {
            case EtatTourInterneEnum.Attente:
                this.EtatTour = EtatTourInterneEnum.Pioche;
                break;
            case EtatTourInterneEnum.Pioche:
                this.EtatTour = EtatTourInterneEnum.Pose;
                break;
            case EtatTourInterneEnum.Pose:
                this.EtatTour = EtatTourInterneEnum.Fin;
                break;
            case EtatTourInterneEnum.Fin:
                throw new IndexOutOfRangeException(
                    "Fin de la tour, il n'est plus possible de l'avancer."
                );
            default:
                break;
        }
    }

    public void ResetTour() => this.EtatTour = EtatTourInterneEnum.Attente;
}
