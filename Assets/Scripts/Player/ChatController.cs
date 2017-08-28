using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class ChatController : MonoBehaviour {

    public InputField ChatBox;
    public Image ChatMessage;
    public RectTransform Content;
    public Text Placeholder;

    int TextSize = 15;
    float lineLenght = 25f;
    float MessageY = 0;
    Player player;
    bool TeamOnly = false;
    List<GameObject> Messages = new List<GameObject>();

    void Start()
    {
        player = GetComponentInParent<Player>();
        Placeholder.text = "Open Chat";
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Return) && ChatBox.text != "") {
            player.CmdChat(player.UserName + ":" + ChatBox.text, TeamOnly);
            ChatBox.text = "";
        } else if (Input.GetKeyDown(KeyCode.Tab)) {
            TeamOnly = !TeamOnly;
            if (TeamOnly) {
                Placeholder.text = "Team Chat";
                Placeholder.color = TeamManager.TeamColors[player.Team];
            } else {
                Placeholder.text = "Open Chat";
                Placeholder.color = Color.white;
            }
        }
    }

    public void PostChat(string _message, Team _Team)
    {
        Image MessBox = Instantiate(ChatMessage);
        Text MessText = MessBox.GetComponentInChildren<Text>();
        Messages.Add(MessBox.gameObject);
        MessText.text = _message;
        int rows = Mathf.CeilToInt(MessText.text.Length / lineLenght);
        MessBox.transform.SetParent(Content.transform);
        MessBox.transform.localScale = new Vector3(1, 1, 1);
        MessBox.rectTransform.sizeDelta = new Vector2(-5, rows * TextSize);
        MessBox.transform.localPosition = new Vector3(Content.GetComponent<RectTransform>().rect.width / 2,rows * TextSize, 0);
        MessageY += rows * TextSize;
        Content.sizeDelta = new Vector2(0, MessageY);
        Content.transform.localPosition = new Vector3(Content.rect.width / 2, MessageY - Content.GetComponentInParent<RectTransform>().rect.height, 0);
        Debug.Log(rows + " " + MessageY);
        MessText.color = TeamManager.TeamColors[_Team];
    }
}
