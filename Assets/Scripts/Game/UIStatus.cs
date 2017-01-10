using UnityEngine;
using System.Collections;
using UnityEngine.UI;

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
        } else {
            a = .3f;
        }
        Panal.color = new Color(Panal.color.r, Panal.color.g, Panal.color.b, a);
    }
    public void SelectShip() {
        if (Ship) {
            if (Ship.Active) {
                Ship.ShipButton.Enable(true);
            }
            FindObjectOfType<GameLevelManager>().LocalPlayer.GetComponent<CamMovement>().MoveTo(Ship.transform.position);
        }
    }
}
