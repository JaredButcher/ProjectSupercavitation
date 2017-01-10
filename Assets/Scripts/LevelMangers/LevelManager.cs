using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Events;

public class LevelManager : MonoBehaviour {

    [Header("Basic Level Manager")]
    public GameObject SettingsMenu;
    public GameObject MainUI;

    protected void Update() {
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
