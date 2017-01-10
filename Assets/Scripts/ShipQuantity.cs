using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ShipQuantity : MonoBehaviour {

    public InputField ShipField;

    string Desinaiton;
    LobbyManager Manager;

    void Start() {
        Manager = FindObjectOfType<LobbyManager>();
        //Make sure name of game object is the same as desination
        Desinaiton = gameObject.name;
    }

    public void QuantityChange(string Quantity) {
        int Ships;
        if (int.TryParse(Quantity, out Ships)) {
            if (Ships >= 0) {
                Manager.ChangeShipQuantity(Ship.Desninations[Desinaiton], Ships);
            } else {
                ShipField.text = "0";
                Manager.ChangeShipQuantity(Ship.Desninations[Desinaiton], 0);
            }
        } else {
            ShipField.text = "0";
            Manager.ChangeShipQuantity(Ship.Desninations[Desinaiton], 0);
        }
    }
}
