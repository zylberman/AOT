using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public Transform player;
    public Camera playerCamera;
    public float ropeSpeed = 10f;

    private bool ropeActive = false;
    private Vector3 ropeTarget;

    void Update()
    {
        // Disparar la cuerda
        if (Input.GetMouseButtonDown(0)) // Botón izquierdo del ratón
        {
            ShootRope();
        }

        // Mantener la cuerda entre el arma y el punto de impacto
        if (ropeActive)
        {
            UpdateRopePosition();
            MoveTowardsRope();
        }
    }

    void ShootRope()
    {
        // Raycast desde la cámara
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, 100f))
        {
            // Verificar si el objeto tiene el tag "Ground"
            if (hit.collider.CompareTag("Ground"))
            {
                ropeActive = true;
                ropeTarget = hit.point;

                // Activar y configurar el LineRenderer
                lineRenderer.enabled = true;
                lineRenderer.positionCount = 2;
                lineRenderer.SetPosition(0, transform.position); // Posición del arma
                lineRenderer.SetPosition(1, ropeTarget);         // Punto de impacto
            }
        }
    }

    void UpdateRopePosition()
    {
        // Actualizar las posiciones del LineRenderer
        lineRenderer.SetPosition(0, transform.position); // Posición del arma
        lineRenderer.SetPosition(1, ropeTarget);         // Punto de impacto
    }

    void MoveTowardsRope()
    {
        // Mover al jugador hacia el punto de la cuerda
        Vector3 direction = (ropeTarget - player.position).normalized;
        player.position = Vector3.MoveTowards(player.position, ropeTarget, ropeSpeed * Time.deltaTime);

        // Desactivar la cuerda cuando el jugador llega al destino
        if (Vector3.Distance(player.position, ropeTarget) < 1f)
        {
            ropeActive = false;
            lineRenderer.enabled = false; // Desactiva el LineRenderer
        }
    }
}
