using UnityEngine;

// Controla la animacion de rotacion 3D continua, el efecto de sonido y la recoleccion de cada moneda en la pista
public class Moneda : MonoBehaviour
{
    // Velocidad angular a la que gira la moneda sobre el eje vertical del mundo
    public float velocidadRotacion = 150f;

    // Clip de audio que se reproduce cuando el jugador recolecta la moneda
    public AudioClip sonidoMoneda;

    // Rota la moneda constantemente en el eje Y global en cada fotograma para lograr un giro 3D visible
    void Update()
    {
        // Usamos Space.World para que la moneda gire verticalmente aunque el modelo este inclinado
        transform.Rotate(0, velocidadRotacion * Time.deltaTime, 0, Space.World);
    }

    // Detecta cuando el jugador atraviesa el trigger de la moneda para reproducir audio, sumar al contador y destruirla
    private void OnTriggerEnter(Collider other)
    {
        // Verificamos si el que toco la moneda es el Jugador
        if (other.CompareTag("Player"))
        {
            // Reproduce el sonido de recoleccion en la posicion de la camara para evitar cortes al destruir el objeto
            if (sonidoMoneda != null)
            {
                AudioSource.PlayClipAtPoint(sonidoMoneda, Camera.main.transform.position);
            }

            // Llamamos a nuestro GameManager (Singleton) para sumar 1 a la UI
            GameManager.Instance.SumarMoneda();

            // Destruimos la moneda de la escena
            Destroy(gameObject);
        }
    }
}