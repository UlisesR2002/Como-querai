using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class TransitionController : MonoBehaviour
{
    public static TransitionController transitionController;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject Panel;
    [SerializeField] private float TransitionTime;
    private string nextScene;
    private bool LoadingScene;

    private void Awake()
    {
        if (transitionController == null)
        {
            //Singleton inicialize
            transitionController = this;
            DontDestroyOnLoad(this);
            LoadingScene = false;
        }
        else if (transitionController != this)
        {
            Destroy(gameObject);
        }
    }

    public void StartTransition(string nextScene)
    {
        if (!LoadingScene)
        {
            this.nextScene = nextScene;
            StartCoroutine(LoadScene());
        }       
    }

    private IEnumerator LoadScene()
    {
        //Estamos cargando
        Panel.SetActive(true);
        LoadingScene = true;
        //hacemos la transicion
        animator.SetTrigger("Transition");
        //esperamos
        yield return new WaitForSecondsRealtime(TransitionTime);
        //cambiamos
        SceneManager.LoadScene(nextScene);
        Time.timeScale = 1f;
        //Llamamos a SceneLoaded cuando se termine de cargar
        SceneManager.sceneLoaded += SceneLoaded;
        
    }

    private void SceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //Por motivos, se tuvo que separar las funciones
        StartCoroutine(OnSceneLoaded());
    }

    private IEnumerator OnSceneLoaded()
    {
        //hacemos la transicion de regreso
        animator.SetTrigger("Transition");
        //esperamos
        yield return new WaitForSecondsRealtime(TransitionTime);
        //Ya no estamos cargando
        Panel.SetActive(false);
        LoadingScene = false;
    }
    

}
