using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;

public class MenuLevelManager : LevelManager {

    public InputField IpField;
    public InputField UserName;
    NetworkManager Networking;
    PlayerSettings Player;

    void Start() {
        Networking = FindObjectOfType<NetworkManager>();
        Player = FindObjectOfType<PlayerSettings>();
    }

    public void JoinGame() {
        if (CheckName()) {
            Networking.networkAddress = IpField.text;
            Networking.StartClient();
        }
    }

    public new void LoadLevel(string level) {
        if (CheckName()) {
            base.LoadLevel(level);
        }
    }
    public void CreateGame() {
        Networking.StopClient();
    }

    bool CheckName() {
        if (UserName.text == "") {
            ColorBlock colors = UserName.GetComponent<InputField>().colors;
            colors.normalColor = Color.red;
            UserName.colors = colors;
            return false;
        }
        Player.UserName = UserName.text;
        return true;
    }
}
