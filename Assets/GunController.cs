using UnityEngine;

public class GunController : MonoBehaviour
{
    public LineRenderer lineRenderer; // LineRenderer para la cuerda
    public Transform player; // Referencia al objeto Player
    public Camera playerCamera; // Cámara del jugador

    // Configuración del resorte
    public float springStrength = 150f; // Fuerza del resorte (50f a 300f)
    public float springDamper = 10f; // Amortiguación del resorte (0f a 20f)
    public float maxRopeLength = 50f; // Longitud máxima de la cuerda (10f a 100f)
    public float ropeShorteningSpeed = 5f; // Velocidad para reducir la cuerda (1f a 20f)

    public string mouseButton = "Right"; // Botón del mouse ("Left" o "Right")

    private SpringJoint springJoint; // Referencia al SpringJoint
    private Rigidbody playerRigidbody; // Referencia al Rigidbody del jugador
    private bool ropeActive = false; // Indica si la cuerda está activa
    private bool isShortening = false; // Indica si se está acortando la cuerda
    private Vector3 ropeTarget; // Punto de anclaje de la cuerda
    private int mouseButtonIndex; // Índice del botón del mouse (0: izquierdo, 1: derecho)

    private float doubleClickTime = 0.3f; // Tiempo máximo entre doble clics
    private float lastClickTime = 0f; // Momento del último clic

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
            if (hit.collider.CompareTag("Ground"))
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
