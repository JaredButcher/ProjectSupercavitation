using UnityEngine;
using System.Collections;

public class SplachScreenLevelManager : LevelManager {

    public float SplashScreenDelay;

    void Start() {
        if (SplashScreenDelay != 0) {
            Invoke("LoadNextLevel", SplashScreenDelay);
        }
    }
}
