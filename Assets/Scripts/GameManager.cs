using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

// Clase principal que gestiona el estado global de la partida, puntajes, interfaz y audio de derrota
public class GameManager : MonoBehaviour
{
    // Patron singleton que nos permite acceder a este script desde cualquier lugar
    public static GameManager Instance;

    // Referencia al componente de texto en la UI que muestra la puntuacion acumulada
    public TextMeshProUGUI textoPuntaje;

    // Referencia al componente de texto en la UI que muestra las monedas recolectadas
    public TextMeshProUGUI textoMonedas;

    // Panel de interfaz grafica que se activa al perder la partida
    public GameObject panelDerrota;

    // Cancion o efecto musical que suena al activar el menu de derrota
    public AudioClip musicaDerrota;

    // Acumulador interno del puntaje obtenido por distancia y eliminacion de enemigos
    private float puntaje = 0;

    // Contador interno de monedas recolectadas durante la partida actual
    private int monedas = 0;

    // Bandera que indica si el jugador ya perdio para detener el conteo y evitar multiples llamadas
    private bool juegoTerminado = false;

    // Referencia al componente AudioSource del GameManager
    private AudioSource audioSource;

    // Inicializa la instancia unica del Singleton y obtiene el componente AudioSource al cargar la escena
    void Awake()
    {
        // Configuracion del singleton
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        // Obtenemos el componente AudioSource del mismo GameManager
        audioSource = GetComponent<AudioSource>();
    }

    // Actualiza continuamente el contador de puntuacion mientras la partida siga activa
    void Update()
    {
        // El puntaje sube automaticamente mientras el jugador avanza
        if (!juegoTerminado)
        {
            // Sube 10 puntos por segundo
            puntaje += Time.deltaTime * 10f;

            // ToString("D8") obliga a que el numero tenga 8 digitos (ej: 00000345)
            textoPuntaje.text = "PUNTUACIÓN: " + Mathf.FloorToInt(puntaje).ToString("D8");
        }
    }

    // Suma puntos adicionales al marcador (por ejemplo, +50 al eliminar un dinosaurio)
    public void SumarPuntosBonus(int cantidad)
    {
        if (juegoTerminado) return;

        puntaje += cantidad;
        textoPuntaje.text = "PUNTUACIÓN: " + Mathf.FloorToInt(puntaje).ToString("D8");
    }

    // Incrementa en uno el contador de monedas y actualiza el texto en pantalla
    public void SumarMoneda()
    {
        monedas++;
        textoMonedas.text = "MONEDAS: " + monedas.ToString("D2");
    }

    // Activa el menu de derrota, cambia la musica de fondo por la de derrota y pausa el juego
    public void MostrarDerrota()
    {
        // Evita que se ejecute dos veces si choca con dos colliders al mismo tiempo
        if (juegoTerminado) return;

        juegoTerminado = true;

        // Mostramos el menu
        panelDerrota.SetActive(true);

        // Cambia la musica de fondo por la musica de derrota
        if (audioSource != null && musicaDerrota != null)
        {
            audioSource.Stop();
            audioSource.clip = musicaDerrota;
            audioSource.loop = false;
            audioSource.Play();
        }

        // Pausa todo el juego
        Time.timeScale = 0f;
    }

    // Restaura el flujo del tiempo y recarga la escena actual para reiniciar la partida
    public void ReiniciarJuego()
    {
        // Restauramos el tiempo a la normalidad
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}