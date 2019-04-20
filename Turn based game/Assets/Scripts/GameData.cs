[System.Serializable]
public class GameData
{
    public bool[] charsDead;
    public bool[] charsHaveMoved;

    public int[] charsHealth;
    public int[] charsDamageBoost;

    public float[,] charsPositions;

    public GameData() { }
}
