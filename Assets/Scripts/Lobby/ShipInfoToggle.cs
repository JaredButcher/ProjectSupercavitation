using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class ShipInfoToggle : MonoBehaviour {
    public Text Name;
    public Text Cost;
    public Text Health;
    public Text Speed;
    public Text Firepower;
    public Text Range;
    public Text Torpedos;
    public Text DepthCharges;
    public Text Secondaries;
    public Text AntiAir;
    public Text Armor;
    public Text Conceilment;

    void Start() {
        Ship ShipInfo = Ship.ShipClass[Ship.Desninations[gameObject.name]];
        Name.text = ShipInfo.ShipName;
        Cost.text = "Cost: " + ShipInfo.Cost;
        Health.text = "Health: " + ShipInfo.MaxHealth;
        Speed.text = "Speed: " + ShipInfo.Speed + "km/t";
        Firepower.text = "Primaries: " + ShipInfo.Fire;
        Range.text = "Primary Range: " + ShipInfo.Range + "km";
        Torpedos.text = "Torpedos: " + ShipInfo.Torps;
        DepthCharges.text = "Depth Charges: " + ShipInfo.Depth;
        Secondaries.text = "Secondaries: " + ShipInfo.Secondary;
        AntiAir.text = "Anti-Air: " + ShipInfo.AntiAir;
        Armor.text = "Armor: " + ShipInfo.Armor + "mm";
        Conceilment.text = "Conceiliment Range: " + ShipInfo.Camo + "km";
    }
}
