using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class TeamManager : MonoBehaviour {
    static List<Slot> Slots;
    public static Dictionary<string, Team> TeamDic = new Dictionary<string, Team>(4) {
        {"Red",Team.Red},{"Blue",Team.Blue },{"Green",Team.Green},{"Yellow",Team.Yellow}
    };
    public static Dictionary<Team, Color> TeamColors = new Dictionary<Team, Color>(4) {
            { Team.Red, Color.red },
            { Team.Blue, Color.blue },
            { Team.Green, Color.green },
            { Team.Yellow, Color.yellow }
    };

    public static void AddPlayerSlot(Player _Player) {
        if(Slots == null) {
            Slots = FindObjectsOfType<Slot>().OrderBy(s => int.Parse(s.name)).ToList();
        }
        if (Slots.Count > 0) {
            Slots[0].Player = _Player;
            Slots.RemoveAt(0);
        }
    }
    public static void UnbindPlayerSlot(Slot _Slot) {
        _Slot.Player = null;
        if (Slots != null) {
            Slots.Insert(0, _Slot);
        }
    }
    /// <summary>
    /// Only use if local player is exiting game
    /// </summary>
    public static void ResetLocalPlayer() {
        Slots = null;
    }
}
public enum Team {
    Red,
    Blue,
    Green,
    Yellow
}
