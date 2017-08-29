using UnityEngine;
using System.Collections;

public class SplachScreenLevelManager : LevelManager {

    //Set to time that splash screen will be displayed
    public float SplashScreenDelay;

    void Start() {
        if (SplashScreenDelay != 0) {
            Invoke("LoadNextLevel", SplashScreenDelay);
        }
    }
}
