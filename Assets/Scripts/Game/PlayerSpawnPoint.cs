using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class PlayerSpawnPoint : MonoBehaviour {
    public static List<PlayerSpawnPoint> SpawnPoints = new List<PlayerSpawnPoint>();

    public string InputTeam;
    bool Used = false;
    Vector3 NextPoint;

    Team Team;
	void Awake () {
        SpawnPoints.Add(this);
        NextPoint = new Vector3(1000, 10, 1000);
        if (InputTeam != "") {
            Team = TeamManager.TeamDic[InputTeam];
        } else {
            Debug.LogWarning("Not all Spawnpoints assinged a team");
        }
	}
    public static PlayerSpawnPoint GetSpawnPoint(Team _Team) {
        PlayerSpawnPoint SpawnPoint = SpawnPoints.Where(p => p.Team == _Team).OrderBy(t => t.name).ToArray()[0];
        SpawnPoint.UseSpawnPoint();
        return SpawnPoint;
    }
    public Vector3 GetSpawnLocation() {
        Vector3 CurrentPoint = NextPoint;
        if(NextPoint.z != -1000f) {
            NextPoint = new Vector3(NextPoint.x, NextPoint.y, NextPoint.z - 500);
        } else {
            NextPoint = new Vector3(NextPoint.x - 500, NextPoint.y ,1000);
        }
        return CurrentPoint;
    }
    void UseSpawnPoint() {
        SpawnPoints.Remove(this);
        Used = true;
    }
}
