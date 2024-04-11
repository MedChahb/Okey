namespace Okey.Tuiles
{
    public class TuileNormale : Tuile
    {
        public TuileNormale(CouleurTuile couleur, int num, bool dansPioche) : base(couleur, num, dansPioche)
        {
        }


        public override bool MemeCouleur(Tuile t)
        {
            return this.Couleur == t.GetCouleur();
        }

        public override bool estSuivant(Tuile t)
        {
            return this.Num + 1 == t.GetNum() || (this.Num == 13 && t.GetNum() == 1);
        }

        public override string ToString()
        {
            return String.Format(null, "({0:00}, {1}, {2})", this.Num, this.Couleur, "No");
        }
    }
}
