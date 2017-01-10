using UnityEngine;
using System.Collections;

public class PlayerSettings : MonoBehaviour {

    public string UserName;

    void Start () {
        DontDestroyOnLoad(gameObject);
    }
}
