using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class RecapLevelManager : LevelManager {
    Recap Recap;
    float YValue = 0;
    public GameObject PlayerInfoPrefab;
    public GameObject PlayerInfoContainer;
    public Text Winner;
    public Text LocalMess;

    void Start() {
        Recap = FindObjectOfType<Recap>();
        if(Recap.GetPlayers()[0].Team == Recap.Winner) {
            LocalMess.text = "Victory";
        } else {
            LocalMess.text = "Defeat";
        }
        Winner.text = Recap.Winner + " Team Won";
        foreach(PlayerRecap Player in Recap.GetPlayers()) {
            GameObject PlayerInfo = Instantiate(PlayerInfoPrefab);
            PlayerInfo.transform.SetParent(PlayerInfoContainer.transform);
            PlayerInfo.transform.localScale = Vector3.one;
            PlayerInfo.transform.localPosition = new Vector3(0, YValue, 0);
            YValue -= 50;
            //TODO Put script on player info and use that to input all needed info
            PlayerInfo.GetComponentInChildren<Text>().text = Player.Name;
            PlayerInfo.GetComponent<Image>().color = TeamManager.TeamColors[Player.Team];
        }
    }
    public void LoadMenu() {
        Destroy(FindObjectOfType<Recap>().gameObject);
        LoadLevel("Menu");
    }
}
