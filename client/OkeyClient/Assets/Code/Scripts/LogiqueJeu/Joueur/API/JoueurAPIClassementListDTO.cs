namespace LogiqueJeu.Joueur
{
    using System;
    using System.Collections.Generic;

    [Serializable]
    public class JoueurAPIClassementListDTO
    {
        public List<JoueurAPIClassementDTO> Classements;

        public override string ToString()
        {
            return $"Classements: {string.Join(", ", this.Classements)}";
        }
    }
}
