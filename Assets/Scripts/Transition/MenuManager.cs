using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuManager : MonoBehaviour
{
    public void GoToScene(string sceneName)
    {
        TransitionController.transitionController.StartTransition(sceneName);
        //SceneManager.LoadScene(sceneName);
    }

    public void ReloadScene()
    {
        TransitionController.transitionController.StartTransition(SceneManager.GetActiveScene().name);
    }

    public void ChangeActiveGameObject(GameObject obj)
    {
        obj.SetActive(!obj.activeSelf);
    }


    public void QuitApp()
    {
        Application.Quit();
    }
}
