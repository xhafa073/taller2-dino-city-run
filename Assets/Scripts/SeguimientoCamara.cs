using UnityEngine;

// Controla el seguimiento de la camara principal detras del jugador manteniendo fijos los ejes X e Y
public class SeguimientoCamara : MonoBehaviour
{
    // Referencia al Transform del jugador al que seguira la camara
    public Transform jugador;

    // Distancia inicial calculada entre la camara y el jugador al comenzar la escena
    private Vector3 offset; 

    // Calcula y almacena la distancia relativa inicial entre la camara y el jugador
    void Start()
    {
        // Guardamos la distancia exacta en la que pusiste la camara al inicio
        offset = transform.position - jugador.position;
    }

    // Actualiza la posicion Z de la camara despues del movimiento del jugador sin desplazarse lateralmente
    void LateUpdate()
    {
        if (jugador != null)
        {
            // Mantenemos la X e Y fijas de la camara, y solo actualizamos la Z (hacia adelante)
            Vector3 nuevaPosicion = new Vector3(transform.position.x, transform.position.y, jugador.position.z + offset.z);
            transform.position = nuevaPosicion;
        }
    }
}