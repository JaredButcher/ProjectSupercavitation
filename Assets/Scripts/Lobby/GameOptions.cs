using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Linq;

//Static object that stores game settings
//Set by GameOptionsSet on Server and by network visa Player on client
public class GameOptions : MonoBehaviour{
    public static int FleetPoints;
    public static int MaxShips;
    public static int MaxPlayers;
    public static string Map = "";
    public static string GameName = "";
    public static int Teams;

    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public static void ServerConf() {
        GameOptionsSet GameData = FindObjectOfType<GameOptionsSet>();
        FleetPoints = GameData.FleetPoints;
        MaxShips = GameData.MaxShips;
        MaxPlayers = GameData.MaxPlayers;
        Map = GameData.Map;
        GameName = GameData.GameName;
        Teams = GameData.Teams;
        FindObjectOfType<NetworkManager>().maxConnections = MaxPlayers;
        Destroy(GameData.gameObject);
    }
 
    
}

