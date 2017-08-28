using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class Slot : MonoBehaviour {
    public Image ReadyMark;
    private Player player;
    private Team team;
    [SerializeField]
    private Button TeamButton;
    [SerializeField]
    private Text Name;
    bool Ready;

    public Player Player {
        get { return player; }
        set {
            player = value;
            if (value) {
                Player.SetSlot(this);
                Name.text = player.UserName;
                Player.SetTeam(Team);
            }
        }
    }
    public Team Team {
        get { return team; }
        set {
            if (Player) {
                Player.SetTeam(value);
            }
        }
    }
    public void UpdateUsername() {
        Name.text = Player.UserName;
    }
    public void ToggleTeam() {
        if (!Ready) {
            if((int)Team >= GameOptions.Teams - 1) {
                Team = Team.Red;
            } else {
                Team++;
            }
        }
    }
    public void SetTeam(Team _Team) {
        if (TeamManager.TeamColors.ContainsKey(_Team)) {
            team = _Team;
            TeamButton.image.color = TeamManager.TeamColors[Team];
        }
    }
    public void SetReady(bool Ready) {
        this.Ready = Ready;
        if (Ready) {
            ReadyMark.gameObject.SetActive(true);
        }else {
            ReadyMark.gameObject.SetActive(false);
        }
    }
    void Update() {
        if (Player) {
            if (Name.text == "") {
                UpdateUsername();
            }
        } else if(Name.text != "Player") {
            Name.text = "Player";
            team = Team.Red;
            TeamButton.image.color = Color.white;
        }
        
    }
    void Start() {
        team = Team.Red;
    }

}
