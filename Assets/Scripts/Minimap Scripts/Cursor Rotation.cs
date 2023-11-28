using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorRotation : MonoBehaviour
{
    [SerializeField] private Transform player;


    // Update is called once per frame
    void Update()
    {
        Quaternion rotationY = player.transform.rotation;

        float angleY = rotationY.eulerAngles.y;

        // Crea una nueva rotaci�n para obj1 con el mismo �ngulo en el eje Z
        Quaternion newRotation = Quaternion.Euler(0f, 0f, -angleY);

        // Aplica la nueva rotaci�n al objeto obj1
        transform.rotation = newRotation;
    }
}
