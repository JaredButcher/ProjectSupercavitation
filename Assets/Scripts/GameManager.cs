using UnityEngine;
using System.Collections.Generic;
using System.Linq;

//staticly stores a list of players
public class GameManager : MonoBehaviour {
    private static Dictionary<string, Player> Players = new Dictionary<string, Player>();

    public static void RegisterPlayer(string _netID, Player _Player){
        Players.Add(_netID, _Player);
    }
    public static void UnregisterPlayer(string _netID)
    {
        Players.Remove(_netID);
    }
    public static void DeregisterPlayer(string _netID)
    {
        Players.Remove(_netID);
    }
    public static List<Player> GetPlayers()
    {
        return Players.Values.Select(p => p).ToList();
    }
    public static Player GetPlayer(string _NetId) {
        return Players[_NetId];
    }
    public static void LocalPlayerReset() {
        Players = new Dictionary<string, Player>();
    }
}
