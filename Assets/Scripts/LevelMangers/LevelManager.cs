using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Events;

//Base class for level managers to inheret
//Containes functions to switch between scenes, close the game, and interact with the settings menu
public class LevelManager : MonoBehaviour {

    [Header("Basic Level Manager")]
    public GameObject SettingsMenu;
    //Everything in the UI besides the settings menu
    public GameObject MainUI;

    protected void Update() {
        //Input to toggle settings menu, children that want to use Update need to run this one as well
        if (Input.GetKeyDown(KeyCode.Escape) && SettingsMenu && MainUI) {
            SettingsToggle();
        }
    }

    public void LoadLevel(int Level) {
        SceneManager.LoadScene(Level);
    }

    public void LoadLevel(string Level) {
        SceneManager.LoadScene(Level);
    }

    public void LoadNextLevel() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Quit() {
        Application.Quit();
    }

    public void SettingsToggle() {
        MainUIToggle();
        SettingsMenu.SetActive(!SettingsMenu.activeSelf);
    }
    protected void MainUIToggle() {
        MainUI.SetActive(!MainUI.activeSelf);
    }
}
