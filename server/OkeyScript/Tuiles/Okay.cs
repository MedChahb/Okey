namespace Okey.Tuiles
{
    public class Okay : Tuile
    {
        public Okay(bool dansPioche)
            : base(CouleurTuile.M, 0, dansPioche)
        {
            //calculate value
        }

        public override bool MemeCouleur(Tuile t)
        {
            return true;
        }

        public override bool estSuivant(Tuile t)
        {
            return true;
        }

        public override String ToString()
        {
            return String.Format(null, "({0:00}, {1}, {2})", this.Num, this.Couleur, "Ok");
        }

        public override bool TuileEquals(Tuile t)
        {
            return true;
        }

        public override string GetName() => "Ok";

        
    }
}
