using UnityEngine;

// Controla la trayectoria rectilinea, tiempo de vida, colisiones y puntaje bonus de las balas disparadas por el jugador
public class Proyectil : MonoBehaviour
{
    // Velocidad de avance del proyectil en el eje Z
    public float velocidad = 30f;

    // Tiempo maximo en segundos antes de que el proyectil se autodestruya si no impacta nada
    public float tiempoDeVida = 3f;

    // Puntos adicionales que recibe el jugador al eliminar a un dinosaurio
    public int puntosPorEnemigo = 50;

    // Clip de audio que se reproduce al eliminar a un dinosaurio
    public AudioClip sonidoMuerte;

    // Programa la autodestruccion del proyectil tras cumplirse su tiempo de vida
    void Start()
    {
        // Limpiamos la memoria destruyendola despues de 3 segundos
        Destroy(gameObject, tiempoDeVida);
    }

    // Desplaza el proyectil en linea recta hacia adelante sin verse afectado por la gravedad
    void Update()
    {
        // Mover la bala hacia adelante constantemente
        transform.Translate(Vector3.forward * velocidad * Time.deltaTime, Space.World);
    }

    // Detecta el impacto contra enemigos (sumando puntos bonus, destruyendo ambos y reproduciendo audio) o contra obstaculos
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemigo"))
        {
            // Reproduce el sonido de muerte en la posicion de la camara para que no se corte al destruir al enemigo
            if (sonidoMuerte != null)
            {
                AudioSource.PlayClipAtPoint(sonidoMuerte, Camera.main.transform.position);
            }

            // Suma los puntos bonus en el GameManager por eliminar al dinosaurio
            if (GameManager.Instance != null)
            {
                GameManager.Instance.SumarPuntosBonus(puntosPorEnemigo);
            }

            // Destruimos al Dinosaurio
            Destroy(other.gameObject);

            // Destruimos la bala
            Destroy(gameObject);
        }
        else if (other.CompareTag("Obstaculo"))
        {
            // Al chocar con un vehiculo, solo se destruye la bala y el auto permanece intacto
            Destroy(gameObject);
        }
    }
}