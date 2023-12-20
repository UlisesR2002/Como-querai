
using UnityEngine;

public class WinLevel : MonoBehaviour
{
    public int thisLevel;

    private void Start()
    {
        thisLevel = SaveManager.GetLevel();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerController _))
        {
            //this is final level
            if (thisLevel >= 5)
            {
                //Go to win scene
                TransitionController.transitionController.StartTransition("Main menu");
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                thisLevel++;
                SaveManager.SetLevel(thisLevel);
                TransitionController.transitionController.StartTransition("Nivel " + thisLevel.ToString());
                Cursor.lockState = CursorLockMode.None;
            }
        }
    }
}
