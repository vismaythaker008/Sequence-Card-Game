using SequenceCardGame;
[System.Serializable]
public class Data
{
    public int HighScore { set; get; }
    public Data(int highScore)
    {
        HighScore = highScore;
    }
}