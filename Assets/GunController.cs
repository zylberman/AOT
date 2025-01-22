using UnityEngine;

public class GunController : MonoBehaviour
{
    public LineRenderer lineRenderer; // Dibuja la cuerda visualmente entre el arma y el punto de anclaje
    public Transform player; // Referencia al objeto del jugador
    public Camera playerCamera; // Cámara del jugador para disparar la cuerda en la dirección de la mirada

    // Configuración del resorte
    public float springStrength = 150f; // Fuerza del resorte:
                                        // Valores bajos (50f): La cuerda será más flexible, permitiendo más oscilaciones.
                                        // Valores altos (300f): La cuerda será más rígida, reduciendo la elasticidad.

    public float springDamper = 10f; // Amortiguación del resorte:
                                     // Valores bajos (0f): La cuerda oscilará más tiempo, como un resorte.
                                     // Valores altos (20f): La cuerda se estabilizará rápidamente, eliminando oscilaciones.

    public float maxRopeLength = 100f; // Longitud máxima de la cuerda:
                                      // Valores bajos (10f): Cuerda corta, limita el alcance del disparo.
                                      // Valores altos (100f): Cuerda larga, permite disparar a objetivos más distantes.

    public float ropeShorteningSpeed = 18f; // Velocidad para reducir la cuerda:
                                           // Valores bajos (1f): La cuerda se acortará lentamente, simulando un tirón suave.
                                           // Valores altos (20f): La cuerda se acortará rápidamente, atrayendo al jugador con fuerza.

    public string mouseButton = "Right"; // Botón del mouse para disparar la cuerda ("Left" o "Right")

    private SpringJoint springJoint; // Referencia al SpringJoint que simula el comportamiento físico de la cuerda
    private Rigidbody playerRigidbody; // Referencia al Rigidbody del jugador para aplicar fuerzas físicas
    private bool ropeActive = false; // Indica si la cuerda está activa
    private bool isShortening = false; // Indica si la cuerda se está acortando
    private Vector3 ropeTarget; // Punto de anclaje de la cuerda
    private int mouseButtonIndex; // Índice del botón del mouse (0: izquierdo, 1: derecho)

    private float doubleClickTime = 0.3f; // Tiempo máximo entre doble clics:
                                          // Valores bajos (0.1f): Requiere clics muy rápidos.
                                          // Valores altos (0.5f): Permite más tiempo entre clics.

    private float lastClickTime = 0f; // Momento del último clic, usado para detectar doble clics

    void Start()
    {
        // Determinar el índice del botón del mouse según la selección
        mouseButtonIndex = mouseButton == "Left" ? 0 : 1;

        // Obtener el Rigidbody del jugador
        playerRigidbody = player.GetComponent<Rigidbody>();
        if (playerRigidbody == null)
        {
            Debug.LogError("El jugador necesita un componente Rigidbody.");
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(mouseButtonIndex))
        {
            HandleMouseClick();
        }

        if (Input.GetMouseButtonUp(mouseButtonIndex) && isShortening)
        {
            ResetRope();
        }

        if (ropeActive)
        {
            UpdateRopePosition(); // Actualizar la posición de la cuerda
            if (isShortening)
            {
                ReduceRopeLength();
            }
        }
    }

    void HandleMouseClick()
    {
        float timeSinceLastClick = Time.time - lastClickTime;
        lastClickTime = Time.time;

        if (timeSinceLastClick < doubleClickTime)
        {
            // Doble clic: Comienza a acortar la cuerda
            if (!ropeActive)
            {
                // Crear la cuerda si aún no existe
                ShootRope();
            }
            isShortening = true;
        }
        else
        {
            // Primer clic: Dispara la cuerda
            if (!ropeActive)
            {
                ShootRope();
            }
        }
    }

    void ShootRope()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, maxRopeLength))
        {
            if (hit.collider.CompareTag("Grappable"))
            {
                ropeActive = true;
                ropeTarget = hit.point;

                // Configurar el SpringJoint
                springJoint = player.gameObject.AddComponent<SpringJoint>();
                springJoint.autoConfigureConnectedAnchor = false;
                springJoint.connectedAnchor = ropeTarget;

                // Configuración del resorte
                springJoint.spring = springStrength;
                springJoint.damper = springDamper;
                springJoint.maxDistance = Vector3.Distance(player.position, ropeTarget);

                // Activar el LineRenderer
                lineRenderer.enabled = true;
                lineRenderer.positionCount = 2;
                lineRenderer.SetPosition(0, transform.position); // Inicio: arma
                lineRenderer.SetPosition(1, ropeTarget);         // Fin: punto de impacto
            }
        }
    }

    void UpdateRopePosition()
    {
        // Mantener la cuerda conectada entre el arma y el punto objetivo
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, ropeTarget);
    }

    void ReduceRopeLength()
    {
        if (springJoint != null)
        {
            // Reducir la longitud máxima del SpringJoint para acortar la cuerda
            springJoint.maxDistance -= ropeShorteningSpeed * Time.deltaTime;
            springJoint.maxDistance = Mathf.Max(springJoint.maxDistance, 1f);

            // Aplicar una fuerza adicional hacia el objetivo para eliminar oscilaciones
            Vector3 directionToTarget = (ropeTarget - player.position).normalized;
            Vector3 projectedDirection = Vector3.ProjectOnPlane(directionToTarget, Vector3.up); // Eliminar componente vertical
            playerRigidbody.velocity = projectedDirection * springStrength * Time.deltaTime;

            // Si el jugador está suficientemente cerca, elimina la cuerda
            if (Vector3.Distance(player.position, ropeTarget) < 1f)
            {
                ResetRope();
            }
        }
    }

    void ResetRope()
    {
        // Desactivar la cuerda
        ropeActive = false;
        isShortening = false;

        // Apagar el LineRenderer
        lineRenderer.enabled = false;

        // Eliminar el SpringJoint
        if (springJoint != null)
        {
            Destroy(springJoint);
        }
    }
}
