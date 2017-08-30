using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Events;
using System.Linq;

//Handles Lobby UI
public class LobbyManager : LevelManager {
    //UI elements
    [Header("Main Screen")]
    public Text GameName;
    public Text Ip;
    public GameObject[] Maps;
    public Button ReadyButton;
    public Text ReadyText;
    [Header("Fleet Info")]
    public Text[] RemShips;
    public Text[] RemPoints;
    public Text Des;
    public Text Dds;
    public Text Cls;
    public Text Ccs;
    public Text Cas;
    public Text Bds;
    public Text Bbs;
    public Text Ces;
    public Text Cvs;
    public Text Sss;
    public Text Sas;
    public Text Ars;
    [Header("Fleet Builder")]
    public GameObject FleetBuilder;
    public GameObject[] FleetBuilderInfo;

    NetworkManager NetworkManager;
    int Points;
    int Ships;
    int MaxPoints;
    int MaxShips;
    bool Ready;
    bool GameStarting;
    //Local player
    Player Player;

    void Start() {
        NetworkManager = FindObjectOfType<NetworkManager>();
        Ip.text = NetworkManager.networkAddress;
        ReadyText.text = "";
    }
    new void Update() {
        base.Update();
        //If the game name UI element hasn't been set then set all of the options 
        if (GameName.text == "") {
            GameName.text = GameOptions.GameName;
            MaxPoints = GameOptions.FleetPoints;
            MaxShips = GameOptions.MaxShips;
            GameObject[] Map = Maps.Where(M => M.name == GameOptions.Map).ToArray();
            if (Map.Length != 0) {
                Map[0].SetActive(true);
            }
            if (Player) {
                SetShips(Player.Fleet.GetShipPlanCount());
                SetPoints(Player.Fleet.GetPlanPointCount());
            }
        }
        //If player isn't set then find the local player in the game manager, cannot be done in start because player object might not yet exsist
        if (!Player) {
            Player[] LocalPlayer = GameManager.GetPlayers().Where(p => p.isLocalPlayer).ToArray();
            if(LocalPlayer.Length > 0) {
                Player = LocalPlayer[0];
                SetShips(Player.Fleet.GetShipPlanCount());
                SetPoints(Player.Fleet.GetPlanPointCount());
                UpdateFleetInfo();
            }
        }
    }
    public void LeaveGame() {
        NetworkManager.StopHost();
        LoadLevel("Menu");
    }
    //Set points on UI and check if the limit has been reached
    bool SetPoints(int _Points) {
        Points = _Points;
        foreach (Text PointText in RemPoints) {
            PointText.text = "Remaining Points: " +(MaxPoints - Points)+ "/" + MaxPoints;
        }
        if(Points <= MaxPoints) {
            return true;
        } else {
            return false;
        }
    }
    bool SetShips(int _Ships) {
        Ships = _Ships;
        foreach (Text ShipText in RemShips) {
            ShipText.text = "Remaining Ships: " +(MaxShips - Ships)+ "/" + MaxShips;
        }
        if(Ships <= MaxShips) {
            return true;
        } else {
            return false;
        }
    }
    public void ToggleFleetBuilder() {
        if (!Ready) {
            if (MainUI.activeSelf) {
                UpdateFleetInfo();
            }
            MainUIToggle();
            FleetBuilder.SetActive(!FleetBuilder.activeSelf);
            if (MainUI.activeSelf) {
                UpdateFleetInfo();
            }
        }
    }
    //Shows info about ship selected in the Fleet Builder UI
    public void ToggleShipInfo(string ShipDes) {
        foreach(GameObject Info in FleetBuilderInfo) {
            Info.SetActive(false);
        }
        FleetBuilderInfo.Where(I => I.name == ShipDes).ToArray()[0].SetActive(true);
    }
    public void ChangeShipQuantity(ShipDesination _Desination, int _Ships) {
        Player.Fleet.AddShipsToPlan(_Desination, _Ships);
        //If done in one line and the first statement is false the seconde will not exicute
        bool Readyable = SetShips(Player.Fleet.GetShipPlanCount());
        ReadyButton.interactable = SetPoints(Player.Fleet.GetPlanPointCount()) && Readyable;
    }
    public void ReadyUp() {
        if (ReadyButton.IsInteractable()) {
            if (Ready) {
                Ready = false;
                ReadyText.text = "";
            } else {
                Ready = true;
                ReadyText.text = "Waiting for other players";
            }
            //Sends ready state over network
            Player.SetReady(Ready);
        }
    }
    public bool GetReady() {
        return Ready;
    }
    //Updates the list of ships the player is currently bringing into battle
    void UpdateFleetInfo() {
        Des.text = "Destoryer Escorts: " + Player.Fleet.ShipPlan[ShipDesination.DE];
        Dds.text = "Destoryers: " + Player.Fleet.ShipPlan[ShipDesination.DD];
        Cls.text = "Light Crusiers: " + Player.Fleet.ShipPlan[ShipDesination.CL];
        Ccs.text = "Crusiers: " + Player.Fleet.ShipPlan[ShipDesination.CC];
        Cas.text = "Battle Crusiers: " + Player.Fleet.ShipPlan[ShipDesination.CA];
        Bds.text = "Dreadnoughts: " + Player.Fleet.ShipPlan[ShipDesination.BD];
        Bbs.text = "Battleships: " + Player.Fleet.ShipPlan[ShipDesination.BB];
        Ces.text = "Escort Carriers: " + Player.Fleet.ShipPlan[ShipDesination.CE];
        Cvs.text = "Carriers: " + Player.Fleet.ShipPlan[ShipDesination.CV];
        Sss.text = "Submarines: " + Player.Fleet.ShipPlan[ShipDesination.SS];
        Sas.text = "Aviation Submarines: " + Player.Fleet.ShipPlan[ShipDesination.SA];
        Ars.text = "Repair ships: " + Player.Fleet.ShipPlan[ShipDesination.AR];
    }
}
