using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // Velocidad de movimiento normal
    public float sprintSpeed = 10f; // Velocidad de carrera
    public float mouseSensitivity = 100f; // Sensibilidad del ratón
    public float jumpForce = 5f; // Fuerza de salto

    private float xRotation = 0f;
    private bool isGrounded;
    public Transform playerCamera;
    private Rigidbody rb;
    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>(); // Obtén el componente Animator
        Cursor.lockState = CursorLockMode.Locked; // Bloquea el cursor al centro de la pantalla
    }

    void Update()
    {
        // Movimiento del mouse
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Limitar la rotación vertical

        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);

        // Movimiento del personaje
        float moveX = Input.GetAxis("Horizontal"); // A/D o flechas izquierda/derecha
        float moveZ = Input.GetAxis("Vertical");   // W/S o flechas arriba/abajo

        // Verificar si el jugador está corriendo
        float currentSpeed = Input.GetKey(KeyCode.LeftShift) && moveZ > 0 ? sprintSpeed : moveSpeed;

        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        Vector3 velocity = move * currentSpeed;
        velocity.y = rb.velocity.y; // Conserva la componente vertical del Rigidbody
        rb.velocity = velocity;

        // Actualizar parámetros del Animator
        animator.SetFloat("XSpeed", moveX); // Actualiza la velocidad horizontal
        animator.SetFloat("YSpeed", moveZ); // Actualiza la velocidad vertical

        // Salto
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}