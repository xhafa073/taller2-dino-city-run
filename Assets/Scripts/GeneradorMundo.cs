using System.Collections.Generic;
using UnityEngine;

// Administra la generación infinita y destrucción de las plataformas de suelo a medida que el jugador avanza
public class GeneradorMundo : MonoBehaviour
{
    // Prefab de la plataforma de suelo que se instanciará en serie
    public GameObject sueloPrefab;

    // Referencia al Transform del jugador para rastrear su avance en el eje Z
    public Transform jugador;

    // Largo de cada plataforma de suelo en el eje Z (debe coincidir con la escala y collider del Prefab)
    public float longitudSuelo = 50f;

    // Cantidad de plataformas activas simultáneamente al iniciar y durante la partida
    public int cantidadSuelosIniciales = 30;

    // Posición en el eje Z donde se instanciará la siguiente plataforma
    private float posicionSpawnZ = 0f;

    // Lista que almacena las plataformas actualmente activas en la escena
    private List<GameObject> suelosActivos = new List<GameObject>();

    // Genera la fila inicial de plataformas al comenzar la partida
    void Start()
    {
        for (int i = 0; i < cantidadSuelosIniciales; i++)
        {
            GenerarSuelo();
        }
    }

    // Vigila la posición Z del jugador para generar una nueva plataforma adelante y eliminar la más antigua
    void Update()
    {
        if (jugador == null) return;

        // Mantiene un margen de 2 suelos detrás del jugador antes de destruir el más antiguo
        float limiteDestruccion = (posicionSpawnZ - (cantidadSuelosIniciales * longitudSuelo)) + (longitudSuelo * 2);

        if (jugador.position.z > limiteDestruccion)
        {
            GenerarSuelo();
            BorrarSueloViejo();
        }
    }

    // Instancia una nueva plataforma en la siguiente posición Z disponible y actualiza el puntero
    private void GenerarSuelo()
    {
        // Respeta la altura Y configurada en el Prefab del suelo
        Vector3 posicion = new Vector3(0, sueloPrefab.transform.position.y, posicionSpawnZ);
        GameObject nuevoSuelo = Instantiate(sueloPrefab, posicion, Quaternion.identity);
        suelosActivos.Add(nuevoSuelo);

        // Avanza la posición Z para el siguiente suelo según su longitud
        posicionSpawnZ += longitudSuelo;
    }

    // Destruye la plataforma más antigua de la lista para liberar memoria
    private void BorrarSueloViejo()
    {
        if (suelosActivos.Count == 0) return;

        Destroy(suelosActivos[0]);
        suelosActivos.RemoveAt(0);
    }
}