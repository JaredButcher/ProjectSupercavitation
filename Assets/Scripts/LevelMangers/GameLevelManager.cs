using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;

public class GameLevelManager : LevelManager {
    public GameObject ShipContainer;
    [Header("Ship Prefabs")]
    public ShipMask DEPrefab;
    public ShipMask DDPrefab;
    public ShipMask CLPrefab;
    public ShipMask CCPrefab;
    public ShipMask CAPrefab;
    public ShipMask BDPrefab;
    public ShipMask BBPrefab;
    public ShipMask CEPrefab;
    public ShipMask CVPrefab;
    public ShipMask SSPrefab;
    public ShipMask SAPrefab;
    public ShipMask ARPrefab;
    [Header("UI")]
    public GameObject Content;
    public UIStatus ShipStatus;
    public Button EndTurnButton;
    public Text SecondaryStatus;
    public Button Confirm;
    public Text QuantityText;
    public Slider QuantitySlider;
    public Slider RedAdvSlider;
    public Slider BlueAdvSlider;
    public Slider GreenAdvSlider;
    public Slider YellowAdvSlider;

    public Player LocalPlayer { get; set; }
    public float UIScaleLevel { get; private set; }
    public int MapSize { get; private set; }
    public GameMode GameMode { get; private set; }
    public bool ShipSelect { get; set; }
    int NumOfUIStatus = 0;
    bool GameEnded = false;
    string QuantiyPre;
    const float ATTRITION = .35f;
    bool DetailedActive;

    static Dictionary<string, Map> MapInfo = new Dictionary<string, Map>(1) {
        {"OceanSmall",new Map(50000,GameMode.Attrition) }
    };

    void Start() {
        Map Map = MapInfo[GameOptions.Map];
        MapSize = Map.Size;
        GameMode = Map.Mode;

        switch (GameOptions.Teams) {
            case 2:
                GreenAdvSlider.gameObject.SetActive(false);
                YellowAdvSlider.gameObject.SetActive(false);
                break;
            case 3:
                YellowAdvSlider.gameObject.SetActive(false);
                break;
        }
        foreach (Player Player in GameManager.GetPlayers()) {
            Player.StartGame();
        }
        RedAdvSlider.minValue = ATTRITION;
        BlueAdvSlider.minValue = ATTRITION;
        GreenAdvSlider.minValue = ATTRITION;
        YellowAdvSlider.minValue = ATTRITION;
        CalculateAttrition();
        ResetExtras();
        ShipSelect = true;
    }
    new void Update() {
        base.Update();
        if (ShipSelect && Input.GetMouseButtonDown(0)) {
            RaycastHit Hit;
            Ray Ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(Ray, out Hit) && Hit.transform.gameObject.tag == "Ship") {
                ShipMask Ship = Hit.transform.parent.GetComponent<ShipMask>();
                if (Ship.Team == LocalPlayer.Team) {
                    if (Ship.Active) {
                        Ship.SetDetailedDisplay(true);
                        foreach (ShipMask ShipM in Fleet.AllShips.Where(S => S.IsSpotted == true && S != Ship)) {
                            ShipM.SetDetailedDisplay(false);
                            ShipM.UpdateRange(Ship);
                        }
                        Ship.ShipButton.Enable(true);
                    }
                }
            }
        } else if (Input.GetKey(KeyCode.LeftAlt)) {
            DetailedActive = true;
            foreach (ShipMask ShipM in Fleet.AllShips.Where(S => S.IsSpotted == true)) {
                ShipM.SetDetailedDisplay(true);
            }
        } else if (DetailedActive) {
            DetailedActive = false;
            foreach (ShipMask ShipM in Fleet.AllShips.Where(S => S.IsSpotted == true)) {
                ShipM.SetDetailedDisplay(false);
            }
        }
    }

    public UIStatus GetUIStatus() {
        UIStatus UIStatus = Instantiate(ShipStatus);
        UIStatus.transform.SetParent(Content.transform);
        UIStatus.transform.localScale = new Vector3(1, 1, 1);
        UIStatus.transform.localPosition = new Vector3(Content.GetComponent<RectTransform>().rect.width / 2, NumOfUIStatus * -25f - 12.5f, 0);
        NumOfUIStatus++;
        return UIStatus;
    }
    public void EndTurn() {
        EndTurnButton.GetComponentInChildren<Text>().text = "Waiting for team";
        EndTurnButton.interactable = false;
        LocalPlayer.EndTurn();
    }
    public void UpdateStatus(Team _TeamTurn) {
        if(LocalPlayer.Team == _TeamTurn) {
            EndTurnButton.interactable = true;
            EndTurnButton.GetComponentInChildren<Text>().text = "End Turn";
        } else {
            EndTurnButton.interactable = false;
            EndTurnButton.GetComponentInChildren<Text>().text = "It is " + _TeamTurn + "'s turn";
        }
    }
    public void ResetExtras() {
        SecondaryStatus.gameObject.SetActive(false);
        Confirm.onClick.RemoveAllListeners();
        Confirm.gameObject.SetActive(false);
        QuantityText.gameObject.SetActive(false);
        QuantitySlider.onValueChanged.RemoveAllListeners();
        QuantitySlider.onValueChanged.AddListener(QuantiyUpdate);
        QuantitySlider.gameObject.SetActive(false);
    }
    public void SetupQuantiy(int _Min, int _Max, string _Text) {
        QuantitySlider.gameObject.SetActive(true);
        QuantityText.gameObject.SetActive(true);
        QuantitySlider.maxValue = _Max;
        QuantitySlider.minValue = _Min;
        QuantiyPre = _Text;
        QuantiyUpdate(_Min);
    }
    public void SetupSecondary(string _Text) {
        SecondaryStatus.gameObject.SetActive(true);
        SecondaryStatus.text = _Text;
    }
    public void SetupConfirm(string _Text, UnityEngine.Events.UnityAction _Action) {
        Confirm.gameObject.SetActive(true);
        Confirm.onClick.AddListener(_Action);
        Confirm.GetComponentInChildren<Text>().text = _Text;
    }
    void QuantiyUpdate(float _Value) {
        QuantityText.text = QuantiyPre + ": " + _Value + " / " + QuantitySlider.maxValue;
    }
    public void SetUIScale(float _Scale) {
        UIScaleLevel = _Scale;
        foreach(ShipMask Ship in Fleet.AllShips) {
            Ship.ScaleUI(UIScaleLevel);
        }
    }
    public bool CalculateAttrition() {
        float RedAdv = (float)Fleet.AdveragePointValue(Team.Red) / GameOptions.FleetPoints;
        float BlueAdv = (float)Fleet.AdveragePointValue(Team.Blue) / GameOptions.FleetPoints;
        float GreenAdv = (float)Fleet.AdveragePointValue(Team.Green) / GameOptions.FleetPoints;
        float YellowAdv = (float)Fleet.AdveragePointValue(Team.Yellow) / GameOptions.FleetPoints;
        Debug.Log(RedAdv);
        RedAdvSlider.value = RedAdv;
        BlueAdvSlider.value = BlueAdv;
        YellowAdvSlider.value = YellowAdv;
        RedAdvSlider.value = RedAdv;
        return RedAdv < ATTRITION ^ BlueAdv < ATTRITION ^ GreenAdv < ATTRITION ^ YellowAdv < ATTRITION;
    }
    public Team FindAttritionWinner() {
        float RedAdv = (float)Fleet.AdveragePointValue(Team.Red) / GameOptions.FleetPoints;
        float BlueAdv = (float)Fleet.AdveragePointValue(Team.Blue) / GameOptions.FleetPoints;
        float GreenAdv = (float)Fleet.AdveragePointValue(Team.Green) / GameOptions.FleetPoints;
        float YellowAdv = (float)Fleet.AdveragePointValue(Team.Yellow) / GameOptions.FleetPoints;
        if(RedAdv > ATTRITION) {
            return Team.Red;
        } else if (BlueAdv > ATTRITION) {
            return Team.Blue;
        } else if(GreenAdv > ATTRITION) {
            return Team.Green;
        } else if (YellowAdv > ATTRITION) {
            return Team.Yellow;
        }
        //Should never get here
        return Team.Red;
    }
}
public enum GameMode {
    Attrition,
    Standared,
    Encounter,
    Assault
}
public struct Map {
    public int Size;
    public GameMode Mode;
    public Map(int _Size, GameMode _Mode) {
        Size = _Size;
        Mode = _Mode;
    }
}
