using UnityEngine;
using System.Collections;
using System;
using UnityEngine.Networking;
using System.Linq;
using UnityEngine.UI;

public class ShipMask : MonoBehaviour {
    #region Serialized Fields
    [Header("Materials")]
    [SerializeField]
    GameObject Icon;
    [SerializeField]
    Material Red;
    [SerializeField]
    Material Blue;
    [SerializeField]
    Material Green;
    [SerializeField]
    Material Yellow;
    [Header("ShipParts")]
    [SerializeField]
    ShipButton ShipButtonPrefab;
    [SerializeField]
    GameObject MoveRadius;
    [SerializeField]
    GameObject FireRadius;
    [SerializeField]
    GameObject TorpRadius;
    [SerializeField]
    GameObject DepthRadius;
    [Header("ShipUI")]
    [SerializeField]
    GameObject UIContainer;
    [SerializeField]
    Text NameText;
    [SerializeField]
    Text PlayerText;
    [SerializeField]
    Text HealthText;
    [SerializeField]
    Slider HealthBar;
    [SerializeField]
    Image EvadeImage;
    [SerializeField]
    Canvas D_UI;
    [SerializeField]
    Canvas S_UI;
    [SerializeField]
    Text SDesText;
    [SerializeField]
    Text SRangeText;
    [SerializeField]
    Text SHealthText;
    [SerializeField]
    Slider SHealthBar;
    [SerializeField]
    GameObject Model;
    #endregion
    #region Other Properties and Fields
    public ShipButton ShipButton { get; private set; }
    public Team Team { get; private set; }
    Team LocalTeam;
    public Ship Ship { get; set; }
    public UIStatus UIStatus { get; set; }
    public bool IsSpotted { get; private set; }
    bool Moveing;
    bool Fireing;
    bool Torping;
    bool Depthing;
    int HealthDispaly;
    float HealthAculator = 0;
    float TravelDistance;
    float RotatingTime;
    float MoveingTime;
    Vector3 PrevPos;
    Quaternion RotatingTarget;
    Quaternion CurrentRotation;
    public bool Active { get; private set; }
    GameLevelManager Manager;
    Player Player;
    #endregion
    void Update() {
        if (Moveing) {
            CheckMoveing();
        } else if (Fireing) {
            CheckFireing();
        } else if (Torping) {
            CheckTorping();
        }
        if (Input.GetMouseButtonDown(1) || Input.GetKey(KeyCode.E)) {
            SetBoolsFalse();
        }
        if (RotatingTime > 0) {
            RotatingTime -= Time.deltaTime;
            Model.transform.rotation =  Quaternion.Lerp(CurrentRotation, RotatingTarget, 1 - RotatingTime * 2);
        }
        if (MoveingTime > 0 && RotatingTime < .1) {
            MoveingTime -= Time.deltaTime;
            Model.transform.position = Vector3.Lerp(PrevPos, transform.position, 1 - MoveingTime);
            UIContainer.transform.position = new Vector3(Model.transform.position.x, UIContainer.transform.position.y, Model.transform.position.z);
        }
        if (HealthAculator != 0) {
            ChangeingHealth();
        }
    }
    //Set up the ship and stuff
    public void SetUpShip(Player _Owner, Team _Team) {
        Player = _Owner;
        Team = _Team;
        switch (_Team) {
            case Team.Red:
                Icon.GetComponent<MeshRenderer>().material = Red;
                break;
            case Team.Blue:
                Icon.GetComponent<MeshRenderer>().material = Blue;
                break;
            case Team.Green:
                Icon.GetComponent<MeshRenderer>().material = Green;
                break;
            case Team.Yellow:
                Icon.GetComponent<MeshRenderer>().material = Yellow;
                break;
        }
        Manager = FindObjectOfType<GameLevelManager>();
        Ship = (Ship)Activator.CreateInstance(Ship.ShipClass[Ship.Desninations[gameObject.name.Substring(0,2)]].GetType());
        Ship.Mask = this;
        transform.SetParent(Manager.ShipContainer.transform);
        if (Manager.LocalPlayer) {
            LocalTeam = Manager.LocalPlayer.Team;
        }
        MoveRadius.transform.localScale = new Vector3(Ship.Speed / 5, Ship.Speed / 5, 1);
        FireRadius.transform.localScale = new Vector3(Ship.Range / 5, Ship.Range / 5, 1);
        TorpRadius.transform.localScale = new Vector3(Ship.TORP_RANGE / 5, Ship.TORP_RANGE / 5, 1);
        DepthRadius.transform.localScale = new Vector3(Ship.Depth * 2, Ship.Depth * 2, 1);
        NameText.text = Ship.ShipName;
        NameText.color = TeamManager.TeamColors[Team];
        PlayerText.text = Player.UserName;
        PlayerText.color = TeamManager.TeamColors[Team];
        HealthBar.fillRect.GetComponent<Image>().color = TeamManager.TeamColors[Team];
        HealthBar.maxValue = Ship.MaxHealth;
        HealthDispaly = Ship.Health;
        SDesText.text = Ship.ShipType.ToString();
        SDesText.color = TeamManager.TeamColors[Team];
        SRangeText.color = TeamManager.TeamColors[Team];
        SHealthBar.fillRect.GetComponent<Image>().color = TeamManager.TeamColors[Team];
        SHealthBar.maxValue = Ship.MaxHealth;
        UpdateHealthBars();
    }
    public void CheckSpoting() {
        ShipMask[] Enimies = Fleet.AllShips.Where(s => s.Team != Team).ToArray();
        bool Spoted = false;
        float Distance;
        foreach(ShipMask Enimie in Enimies) {
            Distance = (Enimie.transform.position - transform.position).magnitude;
            if (Distance <= Enimie.Ship.Camo) {
                Enimie.Spoted(true);
            }
            Spoted = Spoted || Distance <= Ship.Camo;
        }
        this.Spoted(Spoted);
    }
    public void SetLocalTeam(Team _LocalTeam) {
        LocalTeam = _LocalTeam;
    }
    public void Spoted(bool _Spoted) {
        if (_Spoted || Team == LocalTeam) {
            Model.layer = 0;
            Icon.layer = 8;
            //Prevents both UIs from appearing for the moving ship
            S_UI.enabled = !D_UI.enabled; 
            IsSpotted = true;
        } else {
            Model.layer = 9;
            Icon.layer = 9;
            S_UI.enabled = false;
            D_UI.enabled = false;
            IsSpotted = false;
        }
    }
    public void MakeShipButton() {
        ShipButton = Instantiate(ShipButtonPrefab);
        ShipButton.transform.SetParent(FindObjectOfType<LevelManager>().MainUI.transform);
        ShipButton.transform.localScale = new Vector3(1, 1, 1);
        ShipButton.transform.localPosition = new Vector3(0, -265, 0);
        ShipButton.SetShip(this);
        Ship.ShipButton = ShipButton;
    }
    public void MoveTo(Vector3 _Location) {
        PrevPos = transform.position;
        transform.position = _Location;
        Model.transform.position = PrevPos;
        UIContainer.transform.position = new Vector3(Model.transform.position.x, UIContainer.transform.position.y, Model.transform.position.z);
        CurrentRotation = Model.transform.rotation;
        RotatingTarget = Quaternion.FromToRotation(new Vector3(0,0,1), (PrevPos - transform.position).normalized);
        RotatingTime = .5f;
        MoveingTime = 1f;
        CheckSpoting();
        foreach (ShipMask Ship in Fleet.AllShips.Where(S => S.IsSpotted == true && S != this)) {
            Ship.UpdateRange(this);
        }
    }
    //TODO Make the targets into what is displayed and make the health and postion instantly correct
    #region UpdateMethods
    void CheckMoveing() {
        if (Input.GetMouseButtonDown(0)) {
            RaycastHit Hit;
            Ray Ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(Ray, out Hit) && Hit.collider.gameObject.tag == "ShipMove") {
                Moveing = false;
                ShipButton.MovementActive = false;
                Vector3 Position = Hit.point;
                //Clamp within map borders
                Position = new Vector3(Mathf.Clamp(Position.x, -.5f * Manager.MapSize, .5f * Manager.MapSize), 0, Mathf.Clamp(Position.z, -.5f * Manager.MapSize, .5f * Manager.MapSize));
                Manager.LocalPlayer.CmdMoveShip(name, Position);
                SetBoolsFalse();
                Manager.ShipSelect = true;
            }
        }
    }
    void CheckFireing() {
        if (Input.GetMouseButtonDown(0)) {
            RaycastHit Hit;
            Ray Ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //Check if selection was made, then if ship, then if not friendly ship
            if (Physics.Raycast(Ray, out Hit) && Hit.transform.gameObject.tag == "Ship" && Hit.transform.gameObject.GetComponentInParent<ShipMask>().Team != Team ) {
                //Calculate Range only once then check if in range
                float Range = Mathf.Abs((Hit.transform.position - transform.position).magnitude);
                if (Range <= Ship.Range) {
                    ShipMask Enemy = Hit.transform.GetComponentInParent<ShipMask>();
                    FireAtEnemy(Enemy, Range, Ship.Fire);
                    FireRadius.SetActive(false);
                    ShipButton.TacticalActive = false;
                    SetBoolsFalse();
                    Manager.ShipSelect = true;
                }
            }
        }
    }
    void CheckTorping() {
        if (Input.GetMouseButtonDown(0)) {
            RaycastHit Hit;
            Ray Ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //Check if selection was made, then if ship, then if not friendly ship
            if (Physics.Raycast(Ray, out Hit) && Hit.transform.gameObject.tag == "Ship" && Hit.transform.GetComponentInParent<ShipMask>().Team != Team) {
                //Calculate Range only once then check if in range
                float Range = Mathf.Abs((Hit.transform.position - transform.position).magnitude);
                if (Range <= Ship.TORP_RANGE) {
                    int Torps = Mathf.CeilToInt(Mathf.Clamp(Manager.QuantitySlider.value, 1, Ship.Torps));
                    ShipMask Enemy = Hit.transform.GetComponentInParent<ShipMask>();
                    float Evasion = (100 - Mathf.Min(UnityEngine.Random.Range(0, Enemy.Ship.Evasion) + 0.0f, 100f)) / 100f; //Evasion
                    int Damage = (Ship.TORP_DAMAGE * Mathf.RoundToInt(Mathf.Max(0, (Torps * Evasion / Mathf.Max(Range / 2000,1)))) //Range protect vs torps ---Magic Number here---
                        / Mathf.Max(1,(Enemy.Ship.Armor / 150))); //Armor protected vs torps ---Magic Number here---
                    Manager.LocalPlayer.CmdShipHealth(Enemy.name, Enemy.Ship.Health - Damage);
                    Ship.Torps -= Torps;
                    ShipButton.ChangeTorps(Ship.Torps);
                    TorpRadius.SetActive(false);
                    ShipButton.TacticalActive = false;
                    SetBoolsFalse();
                    Manager.ShipSelect = true;
                }
            }
        }
    }
    void ChangeingHealth() {
        HealthAculator -= Ship.MaxHealth / 180f;
        if (HealthAculator <= 0) {
            HealthAculator = 0;
            HealthDispaly = Ship.Health;
            if (Ship.Health <= 0) {
                Sink();
            }
        } else {
            HealthDispaly = (int)(Ship.Health + HealthAculator);
        }
        UpdateHealthBars();
    }
    #endregion
    #region Commands
    public void Move() {
        SetBoolsFalse();
        if (!Moveing) {
            Manager.ShipSelect = false;
            Moveing = true;
            MoveRadius.SetActive(true);
            Manager.SetupSecondary("Click on location to move or press E/RMB to cancel");
        } else {
            Moveing = false;
        }
    }
    public void Fire() {
        SetBoolsFalse();
        if (!Fireing) {
            Manager.ShipSelect = false;
            Fireing = true;
            FireRadius.SetActive(true);
            Manager.SecondaryStatus.gameObject.SetActive(true);
            Manager.SecondaryStatus.text = "Click on enemy ship to fire or press E/RMB to cancel";
        } else {
            Fireing = false;
        }
    }
    public void Evade() {
        SetBoolsFalse();
        Manager.LocalPlayer.CmdShipEvade(name, Ship.Evasion * 2); // ---Magic Number---
        ShipButton.TacticalActive = false;
        SetBoolsFalse();
    }
    public void Torps() {
        SetBoolsFalse();
        if (!Torping) {
            Manager.ShipSelect = false;
            Torping = true;
            TorpRadius.SetActive(true);
            Manager.SetupSecondary("Choose amount of torpedos and click on enemy ship to launch or press E/RMB to cancel");
            Manager.SetupQuantiy(1, Ship.Torps, "Torpedos");
        } else {
            Torping = false;
        }
    }
    public void Depth() {
        SetBoolsFalse();
        if (!Depthing) {
            Manager.ShipSelect = false;
            Depthing = true;
            DepthRadius.SetActive(true);
            Manager.SetupSecondary("Press E/RMB to cancel");
            Manager.SetupConfirm("Drop Charges", new UnityEngine.Events.UnityAction(DropCharges));
        } else {
            Depthing = false;
        }
    }
    void SetBoolsFalse() {
        Moveing = false;
        Fireing = false;
        Torping = false;
        MoveRadius.SetActive(false);
        FireRadius.SetActive(false);
        TorpRadius.SetActive(false);
        DepthRadius.SetActive(false);
        Manager.ResetExtras();
    }
    #endregion
    void DropCharges() {
        Manager.ShipSelect = true;
        SetBoolsFalse();
        DepthRadius.SetActive(false);
        ShipButton.TacticalActive = false;
        //Drop Charges
    }
    public void Sink() {
        Debug.Log("Sinking");
        Fleet.AllShips.Remove(this);
        Player.Fleet.Ships.Remove(this);
        if (UIStatus) {
            UIStatus.Active(false);
            UIStatus.Ship = null;
        }
        if (Manager.LocalPlayer.GameMode == GameMode.Attrition) {
            if (FindObjectOfType<GameLevelManager>().CalculateAttrition()) {
                if (Manager.LocalPlayer.isServer) {
                    Manager.LocalPlayer.CmdGameEnd(FindObjectOfType<GameLevelManager>().FindAttritionWinner());
                }
            }
        }
        Destroy(gameObject);
    }
    public void SetHealth(int _Health) {
        if(Player == Manager.LocalPlayer) {
            Player.GetComponent<CamMovement>().MoveTo(transform.position);
        }
        HealthDispaly = Ship.Health;
        Ship.Health = Math.Max(0, _Health);
        HealthAculator = HealthDispaly - Ship.Health;
    }
    public void SetEvade(int _Evade) {
        EvadeImage.enabled = _Evade > Ship.Evasion;
        Ship.Evasion = _Evade;
    }
    public void UpdateHealthBars() {
        HealthBar.value = HealthDispaly;
        HealthText.text = HealthDispaly + " / " + Ship.MaxHealth;
        SHealthBar.value = HealthDispaly;
        SHealthText.text = HealthDispaly.ToString();
        if (UIStatus) {
            UIStatus.SetHealth(Ship.Health, Ship.MaxHealth);
        }
    }
    public void SetUIStatus(UIStatus _UIStatus) {
        UIStatus = _UIStatus;
        UIStatus.Ship = this;
        UIStatus.SetName(Ship.ShipName);
        UIStatus.SetHealth(Ship.Health, Ship.MaxHealth);
    }
    public void SetActiveTurn(bool _Active) {
        Active = _Active;
        ShipButton.TacticalActive = _Active;
        ShipButton.MovementActive = _Active;
        UIStatus.Active(_Active);
        if (_Active) {
            Ship.UnEvade();
        } else {
            SetBoolsFalse();
        }
    }
    public void ScaleUI(float _Scale) {
        UIContainer.transform.localScale = new Vector3(_Scale,_Scale,1);
    }
    public void SetDetailedDisplay(bool Detailed) {
        D_UI.enabled = Detailed;
        S_UI.enabled = !Detailed;
    }
    public void UpdateRange(ShipMask Ship) {
        SRangeText.text = Mathf.Round(Mathf.Abs((transform.position - Ship.transform.position).magnitude) / 100) / 10 + "km";
    }
    public void FireSecondaries() {
        foreach(ShipMask Enemy in Fleet.AllShips.Where(s => s.Team != Team && s.IsSpotted && s != this)) {
            float Range = Mathf.Abs((Enemy.transform.position - transform.position).magnitude);
            if (Range <= Ship.SecondRange) {
                FireAtEnemy(Enemy, Range, Ship.Secondary);
            }
        }
    }
    void FireAtEnemy(ShipMask _Enemy, float _Range, int _Firepower) {
        float Evasion = (100 - Mathf.Min(UnityEngine.Random.Range(0, _Enemy.Ship.Evasion) + 0.0f, 100f)) / 100f; //Evasion
        int Damage = Mathf.RoundToInt(Mathf.Max(0, (_Firepower * Evasion
            / Mathf.Max(_Range / 3000, 1) //Range protection vs fire ---Magic Number here---
            / Mathf.Max((_Enemy.Ship.Armor / 100), 1)))); //Armor protection vs fire ---Magic Number here---
        Manager.LocalPlayer.CmdShipHealth(_Enemy.name, _Enemy.Ship.Health - Damage);
    }
}
