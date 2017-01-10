using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

public class Fleet : MonoBehaviour {
    public static List<ShipMask> AllShips = new List<ShipMask>();
    int ShipID = 0;
    public List<ShipMask> Ships = new List<ShipMask>();
    public Dictionary<ShipDesination, int> ShipPlan = new Dictionary<ShipDesination, int>(12) {
        {ShipDesination.DE, 0}, {ShipDesination.DD, 0}, {ShipDesination.CL, 0}, {ShipDesination.CC, 0}, {ShipDesination.CA, 0}, {ShipDesination.BD, 0},
        { ShipDesination.BB, 0}, {ShipDesination.CE, 0}, {ShipDesination.CV, 0}, {ShipDesination.SS, 0}, {ShipDesination.SA, 0}, {ShipDesination.AR, 0}
    };
    public FleetPlan Plan { private get; set; }
    public void AddShipsToPlan(ShipDesination Desination, int Ships) {
        if (ShipPlan.ContainsKey(Desination)) {
            ShipPlan[Desination] = Ships;
        }
    }
    public int GetShipCount() {
        return Ships.Count;
    }
    public int GetShipPlanCount() {
        return ShipPlan.Values.Sum();
    }
    public int GetPointCount() {
        int Points = 0;
        foreach (ShipMask Ship in Ships) {
            Points += Ship.Ship.Cost;
        }
        return Points;
    }
    public int GetPlanPointCount() {
        int Total = 0;
        foreach (ShipDesination key in ShipPlan.Keys) {
            Total += Ship.ShipClass[key].Cost * ShipPlan[key];
        }
        return Total;
    }
    public FleetPlan BuildFleetPlan() {
        Plan = new FleetPlan(
            ShipPlan[ShipDesination.DE],
            ShipPlan[ShipDesination.DD],
            ShipPlan[ShipDesination.CL],
            ShipPlan[ShipDesination.CC],
            ShipPlan[ShipDesination.CA],
            ShipPlan[ShipDesination.BD],
            ShipPlan[ShipDesination.BB],
            ShipPlan[ShipDesination.CE],
            ShipPlan[ShipDesination.CV],
            ShipPlan[ShipDesination.SS],
            ShipPlan[ShipDesination.SA],
            ShipPlan[ShipDesination.AR]
        );
        return Plan;
    }
    public void BuildFleet(PlayerSpawnPoint _SpawnPoint, Team _Team, Player _Owner) {
        GameLevelManager Manager = FindObjectOfType<GameLevelManager>();
        for (byte i = 0; i < Plan.DE; i++) {
            Ships.Add(Instantiate(Manager.DEPrefab));
        }
        for (byte i = 0; i < Plan.DD; i++) {
            Ships.Add(Instantiate(Manager.DDPrefab));
        }
        for (byte i = 0; i < Plan.CL; i++) {
            Ships.Add(Instantiate(Manager.CLPrefab));
        }
        for (byte i = 0; i < Plan.CC; i++) {
            Ships.Add(Instantiate(Manager.CCPrefab));
        }
        for (byte i = 0; i < Plan.CA; i++) {
            Ships.Add(Instantiate(Manager.CAPrefab));
        }
        for (byte i = 0; i < Plan.BD; i++) {
            Ships.Add(Instantiate(Manager.BDPrefab));
        }
        for (byte i = 0; i < Plan.BB; i++) {
            Ships.Add(Instantiate(Manager.BBPrefab));
        }
        for (byte i = 0; i < Plan.CE; i++) {
            Ships.Add(Instantiate(Manager.CEPrefab));
        }
        for (byte i = 0; i < Plan.CV; i++) {
            Ships.Add(Instantiate(Manager.CVPrefab));
        }
        for (byte i = 0; i < Plan.SS; i++) {
            Ships.Add(Instantiate(Manager.SSPrefab));
        }
        for (byte i = 0; i < Plan.SA; i++) {
            Ships.Add(Instantiate(Manager.SAPrefab));
        }
        for (byte i = 0; i < Plan.AR; i++) {
            Ships.Add(Instantiate(Manager.ARPrefab));
        }
        foreach(ShipMask Ship in Ships) {
            AllShips.Add(Ship);
            Ship.transform.SetParent(_SpawnPoint.transform);
            Ship.transform.localPosition = _SpawnPoint.GetSpawnLocation();
            Ship.SetUpShip(_Owner,_Team);
            Ship.name = Ship.Ship.ShipType + _Owner.gameObject.name.ToString() + ShipID.ToString();
            ShipID++;
        }
    }
    public static int AdveragePointValue(Team _Team) {
        int Cost = AllShips.Where(s => s.Team == _Team).Sum(b => b.Ship.Cost);
        int Players = GameManager.GetPlayers().Where(p => p.Team == _Team).Count();
        if (Players != 0) {
            return Cost / Players;
        } else {
            return 0;
        }
    }
}
public struct FleetPlan {
    public int DE;
    public int DD;
    public int CL;
    public int CC;
    public int CA;
    public int BD;
    public int BB;
    public int CE;
    public int CV;
    public int SS;
    public int SA;
    public int AR;
    public FleetPlan(int _DE, int _DD, int _CL, int _CC, int _CA, 
        int _BD, int _BB, int _CE, int _CV, int _SS, int _SA, int _AR) {
        DE = _DE;
        DD = _DD;
        CL = _CL;
        CC = _CC;
        CA = _CA;
        BD = _BD;
        BB = _BB;
        CE = _CE;
        CV = _CV;
        SS = _SS;
        SA = _SA;
        AR = _AR;
    }
}
