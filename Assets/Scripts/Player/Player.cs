using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Timers;

public class Player : NetworkBehaviour {

    public static Player LocalPlayer;
    public string UserName;
    public Team Team { get; private set; }
    public GameObject[] DisableObjects;
    public Slot Slot;
    public bool Ready { get; private set; }
    public Fleet Fleet;
    //TODO Keep track of these stats
    public int DamageDelt { get; set; }
    public int ShipsSunk { get; set; }
    public int PointsSunk { get; set; }
    public int PointsLost { get; set; }

    public GameMode GameMode { get; private set; }
    CamMovement Movement;
    PlayerSpawnPoint SpawnPoint;
    ChatController Chat;
    bool GameRunning;
    //When useing OnDisable it removes isLocalPlayer from NetworkBehaviour, so I make another one
    bool IsLocalPlayerDisable;
    #region Behaviors
    void Start() {
        DontDestroyOnLoad(gameObject);
        gameObject.name = "Player_" + GetComponent<NetworkIdentity>().netId.ToString();
        GameManager.RegisterPlayer(netId.ToString(), this);
        Fleet = GetComponent<Fleet>();
        Movement = GetComponent<CamMovement>();
        Movement.enabled = false;
        IsLocalPlayerDisable = isLocalPlayer;
        if (isLocalPlayer) {
            LocalPlayer = this;
            PlayerSettings settings = FindObjectOfType<PlayerSettings>();
            CmdName(settings.UserName);
            Destroy(settings.gameObject);
            Chat = GetComponent<ChatController>();
            if (isServer) {
                GameOptions.ServerConf();
            }
        } else {
            foreach (GameObject thing in DisableObjects) {
                thing.SetActive(false);
            }
            GetComponent<ChatController>().enabled = false;
        }
        if (isClient) {
            TeamManager.AddPlayerSlot(this);
            if (LocalPlayer) {
                LocalPlayer.CmdGetInfo();
            }
        }
    }
    void OnDisable() {
        GameManager.UnregisterPlayer(GetComponent<NetworkIdentity>().netId.ToString());
        TeamManager.UnbindPlayerSlot(Slot);
        if (IsLocalPlayerDisable) {
            TeamManager.ResetLocalPlayer();
            GameManager.LocalPlayerReset();
        }
    }
    void OnPlayerDisconnected(NetworkPlayer _Player) {
        Debug.Log(UserName + " : " + _Player);
    }
    void OnClientDisconnect() {
        Debug.Log("OnClientDisconnect");
    }
    void OnServerDisconnect() {
        Debug.Log("OnServerDisconnect");
    }
    #endregion
    #region Lobby
    public void SetTeam(Team _Team) {
        if (isLocalPlayer) {
            CmdSetTeam(_Team);
        }
    }
    public void SetReady(bool _Ready) {
        if (isLocalPlayer) {
            CmdSetFleet(Fleet.BuildFleetPlan());
            CmdSetReady(_Ready);
        }
    }
    public void SetSlot(Slot _Slot) {
        Slot = _Slot;
    }

    [Command]
    void CmdGetInfo() {
        foreach (Player Player in GameManager.GetPlayers()) {
            Player.TargetGetInfo(connectionToClient, Player.UserName, Player.Team, Player.Ready);
        }
        TargetSetOptions(connectionToClient, GameOptions.FleetPoints, GameOptions.MaxShips,
            GameOptions.MaxPlayers, GameOptions.Map, GameOptions.GameName, GameOptions.Teams);
    }
    [TargetRpc]
    public void TargetGetInfo(NetworkConnection target, string _Username, Team _Team, bool _Ready) {
        UserName = _Username;
        if (Slot) {
            Slot.UpdateUsername();
            Slot.Team = _Team;
        }
        SetReady(_Ready);
    }
    [Command]
    public void CmdChat(string _message, bool _TeamOnly) {
        if (_TeamOnly) {
            foreach (Player player in GameManager.GetPlayers()) {
                if (player.Team == Team) {
                    player.RpcChat("Team: " + _message, Team);
                }
            }
        } else {
            foreach (Player player in GameManager.GetPlayers()) {
                player.RpcChat(_message, Team);
            }
        }
    }
    [ClientRpc]
    private void RpcChat(string _message, Team _Team) {
        if (isLocalPlayer) {
            Chat.PostChat(_message, _Team);
        }
    }
    [Command]
    void CmdName(string _Name) {
        UserName = _Name;
        RpcName(_Name);
    }
    [ClientRpc]
    void RpcName(string _Name) {
        UserName = _Name;
    }
    [TargetRpc]
    void TargetSetOptions(NetworkConnection target, int FP, int MS, int MP, string Map, string GN, int Teams) {
        GameOptions.FleetPoints = FP;
        GameOptions.MaxShips = MS;
        GameOptions.MaxPlayers = MP;
        GameOptions.Map = Map;
        GameOptions.GameName = GN;
        GameOptions.Teams = Teams;
    }
    [Command]
    void CmdSetTeam(Team _Team) {
        Team = _Team;
        RpcSetTeam(_Team);
    }
    [ClientRpc]
    void RpcSetTeam(Team _Team) {
        Team = _Team;
        if (Slot) {
            Slot.SetTeam(_Team);
        }
    }
    [Command]
    void CmdSetReady(bool _Ready) {
        bool GameReady = false;
        if (_Ready) {
            GameReady = true;
            foreach (Player player in GameManager.GetPlayers()) {
                if (player != this) {
                    GameReady = GameReady && player.Ready;
                }
            }
        }
        RpcSetReady(_Ready, GameReady);
    }
    [ClientRpc]
    void RpcSetReady(bool _Ready, bool _AllReady) {
        Ready = _Ready;
        Slot.SetReady(_Ready);
        if (_AllReady) {
            FindObjectOfType<LevelManager>().LoadLevel(GameOptions.Map);
        }
    }
    [Command]
    void CmdSetFleet(FleetPlan _Fleet) {
        RpcSetFleet(_Fleet);
    }
    [ClientRpc]
    void RpcSetFleet(FleetPlan _Fleet) {
        Fleet.Plan = _Fleet;
    }
    #endregion
    #region Game
    Team TeamTurn = Team.Red;
    public bool TurnActive;
    public void StartGame() {
        TurnActive = false;
        SpawnPoint = PlayerSpawnPoint.GetSpawnPoint(Team);
        Fleet.BuildFleet(SpawnPoint, Team, this);
        if (isLocalPlayer) {
            GameLevelManager Manager = FindObjectOfType<GameLevelManager>();
            GameMode = Manager.GameMode;
            Manager.LocalPlayer = this;
            foreach (ShipMask Ship in Fleet.Ships) {
                Ship.SetUIStatus(Manager.GetUIStatus());
                Ship.MakeShipButton();
            }
            foreach (ShipMask Ship in Fleet.AllShips) {
                Ship.SetLocalTeam(Team);
            }
            RpcTurn(Team.Red);
            transform.position = new Vector3(SpawnPoint.transform.position.x, 1000, SpawnPoint.transform.position.y);
            Movement.enabled = true;
        }
        foreach (ShipMask Ship in Fleet.AllShips) {
            Ship.CheckSpoting();
        }
    }
    public void EndTurn() {
        TurnActive = false;
        CmdTurnEnd();
    }
    public PlayerRecap GetRecap() {
        return new PlayerRecap(UserName, Team, DamageDelt, PointsSunk, PointsLost);
    }
    [Command]
    void CmdTurnEnd() {
        TurnActive = false;
        if (GameManager.GetPlayers().Where(p => p.TurnActive == true).ToArray().Length == 0) {
            if (TeamTurn == (Team)GameOptions.Teams - 1) {
                TeamTurn = Team.Red;
            } else {
                TeamTurn++;
            }
            if (GameManager.GetPlayers().Where(p => p.Team == TeamTurn).ToArray().Length == 0) {
                CmdTurnEnd();
                return;
            }
            foreach (Player Player in GameManager.GetPlayers()) {
                Player.TeamTurn = TeamTurn;
                Player.TurnActive = Player.Team == TeamTurn;
            }
            RpcTurn(TeamTurn);
        }
    }
    [ClientRpc]
    public void RpcTurn(Team _Team) {
        FindObjectOfType<GameLevelManager>().UpdateStatus(_Team);
        foreach (ShipMask Ship in LocalPlayer.Fleet.Ships) {
            Ship.FireSecondaries();
        }
        foreach (Player Player in GameManager.GetPlayers()) {
            Player.TeamTurn = _Team;
            Player.TurnActive = Player.Team == _Team;
            if (Player.isLocalPlayer) {
                foreach (ShipMask Ship in Player.Fleet.Ships) {
                    Ship.SetActiveTurn(Player.Team == _Team);
                }
            }
        }
    }
    [Command]
    public void CmdMoveShip(string _ShipID, Vector3 _Location) {
        RpcMoveShip(_ShipID, _Location);
    }
    [ClientRpc]
    void RpcMoveShip(string _ShipID, Vector3 _Location) {
        Fleet.AllShips.Where(s => s.name == _ShipID).ToArray()[0].MoveTo(_Location);
    }
    [Command]
    public void CmdShipHealth(string _ShipID, int _Health) {
        RpcShipHealth(_ShipID, _Health);
    }
    [ClientRpc]
    void RpcShipHealth(string _ShipID, int _Health) {
        ShipMask[] Ship = Fleet.AllShips.Where(s => s.name == _ShipID).ToArray();
        if (Ship.Length > 0) {
            Ship[0].SetHealth(_Health);
        }
    }
    [Command]
    public void CmdShipEvade(string _ShipID, int _Evade) {
        RpcShipEvade(_ShipID, _Evade);
    }
    [ClientRpc]
    void RpcShipEvade(string _ShipID, int _Evade) {
        Fleet.AllShips.Where(s => s.name == _ShipID).ToArray()[0].SetEvade(_Evade);
    }
    [Command]
    public void CmdGameEnd(Team _Winner) {
        RpcGameEnd(_Winner);
    }
    [ClientRpc]
    void RpcGameEnd(Team _Winner) {
        Recap Recap = FindObjectOfType<Recap>();
        Recap.AddPlayerRecap(FindObjectOfType<GameLevelManager>().LocalPlayer.GetRecap());
        foreach(Player Player in GameManager.GetPlayers().Where(p => p != FindObjectOfType<GameLevelManager>().LocalPlayer).ToList()) {
            Recap.AddPlayerRecap(Player.GetRecap());
        }
        FindObjectOfType<NetworkManager>().StopClient();
        FindObjectOfType<NetworkManager>().StopHost();
        if (isServer) {
            FindObjectOfType<NetworkManager>().StopServer();
        }
        Destroy(FindObjectOfType<NetworkManager>().gameObject);
        FindObjectOfType<LevelManager>().LoadLevel("Recap");
        //Generate Battle Reports
        //Change to Recap Screen
    }
    #endregion
}
