using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Networking;

public class CreateGameManager : LevelManager {

    public GameObject FleetPoints;
    public GameObject ShipLimit;
    public Slider FleetSlider;
    public InputField FleetField;
    public Slider PlayerSlider;
    public InputField PlayerField;
    public Slider ShipSlider;
    public InputField ShipField;
    public Button MapButton;
    public GameObject MapSelect;
    public InputField GameName;
    public Slider TeamSlider;
    public InputField TeamField;

    GameOptionsSet Options;
    GameObject MapDisplay;
    NetworkManager NetworkLobby;

	void Start () {
        Options = FindObjectOfType<GameOptionsSet>();
        NetworkLobby = FindObjectOfType<NetworkManager>();
        
    }
    void OnDrawGizmos() {
        Gizmos.color = new Color(1, 0, 0, 0.5F);
        Gizmos.DrawCube(transform.position, new Vector3(10, 10, 10));
    }
    public void SetForceFleet(bool ForceFleet) {
        Options.ForceFleet = ForceFleet;
        ShipLimit.SetActive(!ForceFleet);
        FleetPoints.SetActive(!ForceFleet);
    }

    public void CreateGame() {  
        if(Options.Map == "") {
            ColorBlock colors = MapButton.GetComponent<Button>().colors;
            colors.normalColor = Color.red;
            MapButton.GetComponent<Button>().colors = colors;
            return;
        }
        if(GameName.text == "") {
            GameName.text = FindObjectOfType<PlayerSettings>().UserName + "'s Game";
        }
        Options.FleetPoints = (int)FleetSlider.value;
        Options.MaxShips = (int)ShipSlider.value;
        Options.MaxPlayers = (int)PlayerSlider.value;
        Options.GameName = GameName.text;
        Options.Teams = (int)TeamSlider.value;
        NetworkLobby.StartHost();
    }

    public void SetMap(GameObject Map) {
        DestroyObject(MapDisplay);
        Options.Map = Map.name;
        MapDisplay = Instantiate(Map, transform.position, new Quaternion()) as GameObject;
        MapDisplay.transform.SetParent(FindObjectOfType<LevelManager>().transform);
        MapDisplay.GetComponent<Button>().interactable = false;
        MapDisplay.transform.localScale = new Vector3(1f, 1f);
    }
    public void MapSelectToggle() {
        ColorBlock colors = MapButton.GetComponent<Button>().colors;
        colors.normalColor = Color.white;
        MapButton.GetComponent<Button>().colors = colors;
        MapSelect.SetActive(!MapSelect.activeInHierarchy);
    }
    public void SetFleetPoints(float Points) {
        SetValue(Points, FleetField);
    }
    public void SetFleetPoints(string Points) {
        SetValue(Points, FleetSlider);
    }
    public void SetMaxPlayers(float Players) {
        SetValue(Players, PlayerField);
    }
    public void SetMaxPlayers(string Players) {
        SetValue(Players, PlayerSlider);
    }
    public void SetTeams(float Teams) {
        SetValue(Teams, TeamField);
    }
    public void SetTeams(string Teams) {
        SetValue(Teams, TeamSlider);
    }
    public void SetShips(float Ships){
        SetValue(Ships, ShipField);
    }
    public void SetShips(string Ships){
        SetValue(Ships, ShipSlider);
    }

    void SetValue(float Value, InputField Field)
    {
        Field.text = Value.ToString();
    }
    void SetValue(string Value, Slider Slider)
    {
        float temp;
        if (float.TryParse(Value, out temp))
        {
            Slider.value = Mathf.Clamp(temp, Slider.minValue, Slider.maxValue);
        }
    }
}
