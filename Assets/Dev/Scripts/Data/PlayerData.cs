[System.Serializable]
public class PlayerData
{
    public int collectedCommon;
    public int collectedRare;
    public int collectedLegendary;
    public float collectedCoin;

    public PlayerData()
    {
        collectedCommon = 0;
        collectedRare = 0;
        collectedLegendary = 0;
        collectedCoin = 0;
    }
    
}
