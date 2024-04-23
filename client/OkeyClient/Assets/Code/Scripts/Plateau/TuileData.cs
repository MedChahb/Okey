[System.Serializable]
public class TuileData
{
    public string couleur;
    public int num;
    public bool isJoker;

    public TuileData(CouleurTuile couleur, int num, bool isJoker)
    {
        this.couleur = couleur.ToString();
        this.num = num;
        this.isJoker = isJoker;
    }
}
