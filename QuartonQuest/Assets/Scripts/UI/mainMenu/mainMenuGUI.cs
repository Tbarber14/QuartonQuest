using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class mainMenuGUI : MonoBehaviour
{
    public AudioClip storyModeButtonSound;
    public AudioClip quickPlayButtonSound;
    public AudioClip multiplayerButtonSound;
    public AudioClip settingsButtonSound;

    public GameObject helpPanel;
    public GameObject optionsPanel;


    public void ToggleHelpPanel()
    {
        if (helpPanel.activeInHierarchy)
            helpPanel.SetActive(false);
        else
            helpPanel.SetActive(true);
    }

    public void storyMode_buttonClicked()
    {
        Debug.Log("storyMode_button was clicked");
        //audio.clip = storyModeButtonSound;
        //audio.Play(0);
        GUIController.Instance.LoadSceneWithTransition(GUIController.SceneNames.Story1);
    }

    public void quickPlay_buttonClicked()
    {
        Debug.Log("quickPlay_button was clicked");
        GUIController.Instance.LoadSceneWithTransition(GUIController.SceneNames.QuickPlayMenu);
    }

    public void multiplayer_buttonClicked()
    {
        Debug.Log("multiplayer_button was clicked");
        GUIController.Instance.LoadSceneWithTransition(GUIController.SceneNames.NetworkMenu);
    }

    public void quit_buttonClicked()
    {
        Debug.Log("Quitting game");
        Application.Quit();
    }
}
