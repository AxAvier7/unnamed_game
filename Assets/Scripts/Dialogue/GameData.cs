using System.Collections.Generic;

public class GameData
{
    private static GameData _instance;
    public static GameData Instance
    {
        get
        {
            if (_instance == null)
                _instance = new GameData();
            return _instance;
        }
    }

    private GameData()
    {
        PlayerNames = new List<string>();
    }

    public int Players { get; set; }
    public List<string> PlayerNames { get; set; }
    public int Chips { get; set; }
}
