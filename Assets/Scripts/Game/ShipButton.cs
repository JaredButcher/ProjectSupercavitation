using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class ShipButton : MonoBehaviour {
    static List<ShipButton> ShipButtons= new List<ShipButton>();
    [SerializeField]
    Button MoveButton;
    [SerializeField]
    Button[] TacticalButtons;
    [SerializeField]
    Button TorpText;
    ShipMask Ship;
    public bool TacticalActive {
        get {
            return tacticalActive;
        } set {
            tacticalActive = value;
            foreach(Button Button in TacticalButtons) {
                Button.interactable = value;
            }
            CheckActive();
        }
    }
    public bool MovementActive {
        get {
            return movementActive;
        } set {
            movementActive = value;
            MoveButton.interactable = value;
            CheckActive();
        }
    }

    bool tacticalActive = false;
    bool movementActive = false;

    void Start() {
        ShipButtons.Add(this);
        Enable(false);
    }

    public void SetShip(ShipMask _Ship) {
        Ship = _Ship;
        if (Ship.Ship.Torps > 0) {
            ChangeTorps(Ship.Ship.Torps);
        }
    }
    public void Enable(bool _Enable) {
        if (_Enable) {
            foreach(ShipButton Ship in ShipButtons) {
                Ship.gameObject.SetActive(false);
            }
            gameObject.SetActive(true);
        } else {
            gameObject.SetActive(false);
        }
    }
    public void Move() {
        Ship.Move();
    }
    public void Torps() {
        if (Ship.Ship.Torps != 0) {
            Ship.Torps();
        }
    }
    public void Fire() {
        Ship.Fire();
    }
    public void Depth() {
        Ship.Depth();
    }
    public void Evasive() {
        Ship.Evade();
    }
    void CheckActive() {
        if (!(tacticalActive || movementActive)) {
            if (Ship.Active) {
                Ship.SetActiveTurn(false);
            }
        } else if(tacticalActive ^ movementActive) {
            if (Ship.Active) {
                Ship.UIStatus.Active(false);
            }
        }
    }
    public void ChangeTorps(int _Torps) {
        TorpText.GetComponentInChildren<Text>().text = "Torpedos: " + _Torps;
    }
}
