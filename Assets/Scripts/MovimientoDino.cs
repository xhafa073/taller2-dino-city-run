using UnityEngine;

// Controla el desplazamiento lineal continuo de los enemigos dinosaurios en direccion contraria al jugador
public class MovimientoDino : MonoBehaviour
{
    // Velocidad de desplazamiento del dinosaurio (configurable segun el diseño del enemigo)
    public float velocidad = 8f; 

    // Referencia al componente Rigidbody del dinosaurio para moverlo mediante fisicas
    private Rigidbody rb;

    // Obtiene el Rigidbody y orienta el modelo 180 grados para que mire hacia el jugador
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        transform.rotation = Quaternion.Euler(0, 180, 0);
    }

    // Mueve al dinosaurio en linea recta hacia atras (eje Z negativo) de manera constante
    void FixedUpdate()
    {
        // Vector3.back significa que corre en el eje Z negativo (hacia la cámara/jugador)
        Vector3 movimiento = Vector3.back * velocidad * Time.fixedDeltaTime;
        
        // Lo movemos a su nueva posición
        rb.MovePosition(rb.position + movimiento);
    }
}