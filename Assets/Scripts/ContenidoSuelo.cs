using System.Collections.Generic;
using UnityEngine;


// Controla la aparición aleatoria de vehículos, dinosaurios y monedas
// sobre los carriles disponibles de cada plataforma de suelo.
public class ContenidoSuelo : MonoBehaviour
{
    // Cantidad de objetos que se generarán en cada plataforma.
    public int cantidadObjetosPorSuelo = 2;

    // Lista de prefabs posibles a instanciar (autos, dinosaurios y monedas).
    public GameObject[] posiblesObjetos;

    // Puntos de referencia que representan los carriles de la plataforma.
    public Transform[] puntosDeAparicion;

    // Al crearse la plataforma, instancia objetos aleatorios en carriles distintos.
    void Start()
    {
        if (transform.position.z < 15f)
        {
            return;
        }

        if (posiblesObjetos == null || posiblesObjetos.Length == 0 ||
            puntosDeAparicion == null || puntosDeAparicion.Length == 0)
        {
            return;
        }

        // Asegura que no se intenten generar más objetos que carriles disponibles
        int cantidad = Mathf.Min(cantidadObjetosPorSuelo, puntosDeAparicion.Length);

        // Guarda los índices de carriles disponibles para no repetir carril en el mismo suelo
        List<int> carrilesDisponibles = new List<int>();
        for (int i = 0; i < puntosDeAparicion.Length; i++)
        {
            carrilesDisponibles.Add(i);
        }

        for (int i = 0; i < cantidad; i++)
        {
            int indiceObjeto = Random.Range(0, posiblesObjetos.Length);

            // Elige un carril libre al azar y lo elimina de la lista de disponibles
            int randomIndexCarril = Random.Range(0, carrilesDisponibles.Count);
            int indicePunto = carrilesDisponibles[randomIndexCarril];
            carrilesDisponibles.RemoveAt(randomIndexCarril);

            // Instancia el objeto como hijo de este suelo para que se destruya junto con él
            Instantiate(
                posiblesObjetos[indiceObjeto],
                puntosDeAparicion[indicePunto].position,
                posiblesObjetos[indiceObjeto].transform.rotation,
                transform
            );
        }
    }
}