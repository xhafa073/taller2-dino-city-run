using UnityEngine;

// Controla el movimiento continuo, dificultad progresiva, cambio de carriles, salto, disparo y colisiones del personaje principal
public class Jugador : MonoBehaviour
{
    // Velocidad inicial y actual de la carrera hacia adelante
    public float velocidadAvance = 10f; 

    // Velocidad maxima que puede alcanzar el jugador para mantener la jugabilidad y no superar sus propias balas
    public float velocidadMaxima = 22f;

    // Cuanto aumenta la velocidad de avance por cada segundo transcurrido
    public float incrementoVelocidad = 0.2f;

    // Que tan rapido cambia de carril
    public float velocidadLateral = 10f; 

    // Que tanto salta
    public float fuerzaSalto = 7f;

    // Distancia en metros entre cada carril
    public float distanciaCarril = 3f; 

    // Prefab del proyectil que sera instanciado al disparar
    public GameObject balaPrefab;

    // Punto exacto en el arma desde donde salen los proyectiles
    public Transform puntoDisparo;

    // Componente de audio encargado de reproducir el efecto de sonido al disparar
    private AudioSource sonidoDisparo;

    // Controla el tiempo de espera minimo de 0.5s entre disparos
    private float proximoDisparo = 0f; 

    // Referencia al componente Rigidbody para aplicar movimiento fisico y salto
    private Rigidbody rb;

    // Indice del carril actual: 0 = Izquierda, 1 = Centro, 2 = Derecha
    private int carrilActual = 1; 

    // Indica si el jugador esta tocando el suelo para habilitar el salto
    private bool enElSuelo = true;

    // Obtiene las referencias a los componentes Rigidbody y AudioSource al iniciar
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        sonidoDisparo = GetComponent<AudioSource>();
    }

    // Aumenta progresivamente la velocidad y lee las entradas de teclado para cambiar de carril, saltar y disparar
    void Update()
    {
        // Si el juego esta pausado (menu de derrota), ignora el aumento de velocidad y todas las teclas
        if (Time.timeScale == 0f) return;

        // Dificultad progresiva: incrementa la velocidad de avance gradualmente hasta el tope maximo
        velocidadAvance = Mathf.Min(velocidadAvance + incrementoVelocidad * Time.deltaTime, velocidadMaxima);

        // Detectar cambio de carril (Teclas A y D)
        if (Input.GetKeyDown(KeyCode.D))
        {
            carrilActual++;

            // Limitar al carril derecho
            if (carrilActual > 2) carrilActual = 2; 
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            carrilActual--;

            // Limitar al carril izquierdo
            if (carrilActual < 0) carrilActual = 0; 
        }

        // Detectar Salto
        if (Input.GetKeyDown(KeyCode.Space) && enElSuelo)
        {
            // Aplicamos un impulso hacia arriba
            rb.AddForce(Vector3.up * fuerzaSalto, ForceMode.Impulse);
            enElSuelo = false; 
        }

        // Detectar Disparo (Tecla F)
        if (Input.GetKeyDown(KeyCode.F) && Time.time >= proximoDisparo)
        {
            // Instanciamos (creamos) la bala en el punto de disparo
            Instantiate(balaPrefab, puntoDisparo.position, puntoDisparo.rotation);

            sonidoDisparo.Play();
            
            // Calculamos cuando podremos volver a disparar (tiempo actual + 0.5 segundos)
            proximoDisparo = Time.time + 0.5f;
        }
    }

    // Ejecuta el desplazamiento fisico constante hacia adelante y la interpolacion suave hacia el carril objetivo
    void FixedUpdate()
    {
        // Calculamos a que posicion en X debe ir segun su carril (-3, 0 o 3)
        float posicionXObjetivo = (carrilActual - 1) * distanciaCarril;

        // Calculamos la meta: X (carril), Y (su altura actual), Z (movimiento infinito hacia adelante)
        Vector3 meta = new Vector3(posicionXObjetivo, rb.position.y, rb.position.z + velocidadAvance * Time.fixedDeltaTime);

        // Movemos el Rigidbody suavemente hacia esa meta
        Vector3 nuevaPosicion = Vector3.Lerp(rb.position, meta, velocidadLateral * Time.fixedDeltaTime);
        rb.MovePosition(nuevaPosicion);
    }

    // Detecta colisiones fisicas con el suelo para permitir volver a saltar, o con enemigos/obstaculos para activar la derrota
    private void OnCollisionEnter(Collision collision)
    {
        // Verifica si el jugador aterrizo sobre una plataforma de suelo
        if (collision.gameObject.CompareTag("Suelo"))
        {
            enElSuelo = true;
        }
        
        // Verifica si el jugador impacto contra un dinosaurio o un vehiculo obstaculo
        if (collision.gameObject.CompareTag("Enemigo") || collision.gameObject.CompareTag("Obstaculo"))
        {
            // Activamos la pantalla de derrota en el GameManager
            GameManager.Instance.MostrarDerrota();
        }
    }
}