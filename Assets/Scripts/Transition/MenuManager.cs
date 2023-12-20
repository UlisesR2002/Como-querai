using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuManager : MonoBehaviour
{
    public AudioClip buttonClickSound;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    public void NewGame()
    {
        audioSource.PlayOneShot(buttonClickSound);
        SaveManager.SetLevel(1);
        TransitionController.transitionController.StartTransition("Nivel " + 1.ToString());
    }

    public void ResumeGame()
    {
        audioSource.PlayOneShot(buttonClickSound);
        TransitionController.transitionController.StartTransition("Nivel " + SaveManager.GetLevel().ToString());
    }


    public void GoToScene(string sceneName)
    {
        audioSource.PlayOneShot(buttonClickSound);
        TransitionController.transitionController.StartTransition(sceneName);
    }

    public void ReloadScene()
    {
        audioSource.PlayOneShot(buttonClickSound);
        TransitionController.transitionController.StartTransition(SceneManager.GetActiveScene().name);
    }

    public void ChangeActiveGameObject(GameObject obj)
    {
        audioSource.PlayOneShot(buttonClickSound);
        obj.SetActive(!obj.activeSelf);
    }


    public void QuitApp()
    {
        audioSource.PlayOneShot(buttonClickSound);
        Application.Quit();
    }
}
