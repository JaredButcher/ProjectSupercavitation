using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Linq;

public class UIStatus : MonoBehaviour {
    [SerializeField]
    Text Name;
    [SerializeField]
    Text Health;
    [SerializeField]
    Slider HealthBar;
    public ShipMask Ship;
    Image Panal;
    public void SetName(string _Name) {
        Name.text = _Name;
    }
    public void SetHealth(int _Health, int _MaxHealth) {
        Health.text = _Health + " / " + _MaxHealth;
        HealthBar.maxValue = _MaxHealth;
        HealthBar.value = _Health;
        //TODO Add animation showing health decressing
    }
    public void Active(bool _Active) {
        if (!Panal) {
            Panal = GetComponent<Image>();
        }
        float a;
        if (_Active) {
            a = 1;
        } else if (Ship.ShipButton.TacticalActive || Ship.ShipButton.MovementActive) {
            a = .3f;
        } else {
            a = 0;
        }
        Panal.color = new Color(Panal.color.r, Panal.color.g, Panal.color.b, a);
    }
    public void SelectShip() {
        if (Ship) {
            if (Ship.Active) {
                Ship.SetDetailedDisplay(true);
                foreach (ShipMask ShipM in Fleet.AllShips.Where(S => S.IsSpotted == true && S != Ship)) {
                    ShipM.SetDetailedDisplay(false);
                    ShipM.UpdateRange(Ship);
                }
                Ship.ShipButton.Enable(true);
            }
            FindObjectOfType<GameLevelManager>().LocalPlayer.GetComponent<CamMovement>().MoveTo(Ship.transform.position);
        }
    }
}
