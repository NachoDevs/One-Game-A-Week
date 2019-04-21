[System.Serializable]
public class GameData
{
    public bool[] charsDead;
    public bool[] charsHaveMoved;

    public int[] charactersInvolvedInCombat;
    public int[] charsHealth;

    public float[] charsDamageBoost;

    public float[,] charsPositions;

    public GameData() { }
}
