using UnityEngine;
using System.Collections;

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
