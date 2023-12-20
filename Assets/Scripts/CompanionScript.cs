using TMPro;
using UnityEngine;

public class CompanionScript : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float minDistance;
    [SerializeField] private float elevationHeight; // Nueva variable para la altura de elevación
    [SerializeField] private float speed;
    [SerializeField] private float rotationspeed;

    public TextMeshPro messageText;

    private bool mensajeMostrado = false;

    void Update()
    {
        if (player != null)
        {
            Vector3 direction = player.position - transform.position;
            float distance = direction.magnitude;

            if (distance > minDistance)
            {
                // Calcula la posición relativa al jugador con elevación en Y
                Vector3 posicionRelativa = player.position - (direction.normalized * minDistance) + Vector3.up * elevationHeight;

                // Calcula la rotación hacia la posición relativa
                Quaternion rotacion = Quaternion.LookRotation(posicionRelativa - transform.position);

                // Aplica la rotación suave
                transform.rotation = Quaternion.Slerp(transform.rotation, rotacion, Time.deltaTime * rotationspeed);

                // Mueve al compañero hacia la posición relativa
                transform.Translate(Vector3.forward * speed * Time.deltaTime);

                // Si el mensaje aún no se ha mostrado, muéstralo
                if (!mensajeMostrado)
                {
                    if (messageText != null)
                    {
                        messageText.text = "Here is a clue for you";
                        mensajeMostrado = true;
                    }
                }
            }
            // Restablece el mensaje si el compañero está cerca del jugador
            else
            {
                mensajeMostrado = false;
            }
        }
    }
}