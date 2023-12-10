
using UnityEngine;

public class WinLevel : MonoBehaviour
{
    public string nextLevel;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerController _))
        {
            TransitionController.transitionController.StartTransition(nextLevel);
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
