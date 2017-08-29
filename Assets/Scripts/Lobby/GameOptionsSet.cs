using UnityEngine;
using System.Collections;

//A temp object for storing the settings before they are transerfered to the static GameOptions
//Server Only
public class GameOptionsSet : MonoBehaviour {
    public int FleetPoints;
    public int MaxShips;
    public int MaxPlayers;
    public bool ForceFleet;
    public string Map = "";
    public string GameName;
    public int Teams;

    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
}
