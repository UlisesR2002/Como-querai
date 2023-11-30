using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuManager : MonoBehaviour
{
    public void GoToScene(string sceneName)
    {
        TransitionController.transitionController.StartTransition(sceneName);
        //SceneManager.LoadScene(sceneName);
    }
    public void Unpause()
    {
        PlayerController.instance.CheckPause();
    }
    public void QuitApp()
    {
        Application.Quit();
    }
}
