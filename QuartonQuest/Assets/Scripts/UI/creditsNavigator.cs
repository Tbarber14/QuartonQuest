using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class creditsNavigator : MonoBehaviour
{
    public void MainMenuButtonClicked()
    {
        GUIController.Instance.LoadSceneWithTransition(GUIController.SceneNames.MainMenu);
    }
}
