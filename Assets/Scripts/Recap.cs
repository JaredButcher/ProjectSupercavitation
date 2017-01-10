using UnityEngine;
using System.Collections.Generic;

public class Recap : MonoBehaviour {
    List<PlayerRecap> Players = new List<PlayerRecap>();
    public Team Winner { get; private set; }
	void Start () {
        DontDestroyOnLoad(gameObject);
	}
    public void SetWinner(Team _Winner) {
        Winner = _Winner;
    }
    public void AddPlayerRecap(PlayerRecap _PlayerRecap) {
        Players.Add(_PlayerRecap);
    }
    public List<PlayerRecap> GetPlayers() {
        return Players;
    }

}
public struct PlayerRecap {
    public int Damage;
    public int PointsSunk;
    public int PointsLost;
    public string Name;
    public Team Team;
    public PlayerRecap(string _Name, Team _Team, int _Damage, int _PointsSunk, int _PointsLost) {
        Name = _Name;
        Team = _Team;
        Damage = _Damage;
        PointsSunk = _PointsSunk;
        PointsLost = _PointsLost;
    } 
}

