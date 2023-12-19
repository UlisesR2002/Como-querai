using UnityEngine;

public class SeguirJugador : MonoBehaviour
{
    public Transform jugador;
    public float distanciaMinima = 2.0f;
    public float velocidad = 3.0f;
    public float velocidadRotacion = 5.0f;

    void Update()
    {
        if (jugador != null)
        {
            Vector3 direccion = jugador.position - transform.position;
            float distancia = direccion.magnitude;

            if (distancia > distanciaMinima)
            {
                // Calcula la posición relativa al jugador
                Vector3 posicionRelativa = jugador.position - (direccion.normalized * distanciaMinima);

                // Calcula la rotación hacia la posición relativa
                Quaternion rotacion = Quaternion.LookRotation(posicionRelativa - transform.position);

                // Aplica la rotación suave
                transform.rotation = Quaternion.Slerp(transform.rotation, rotacion, Time.deltaTime * velocidadRotacion);

                // Mueve al compañero hacia la posición relativa
                transform.Translate(Vector3.forward * velocidad * Time.deltaTime);
            }
        }
    }
}