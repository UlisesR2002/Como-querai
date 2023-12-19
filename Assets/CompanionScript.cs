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
                // Calcula la posici�n relativa al jugador
                Vector3 posicionRelativa = jugador.position - (direccion.normalized * distanciaMinima);

                // Calcula la rotaci�n hacia la posici�n relativa
                Quaternion rotacion = Quaternion.LookRotation(posicionRelativa - transform.position);

                // Aplica la rotaci�n suave
                transform.rotation = Quaternion.Slerp(transform.rotation, rotacion, Time.deltaTime * velocidadRotacion);

                // Mueve al compa�ero hacia la posici�n relativa
                transform.Translate(Vector3.forward * velocidad * Time.deltaTime);
            }
        }
    }
}