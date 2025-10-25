using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Cible à suivre")]
    public Transform target;          // La voiture (GameObject à suivre)

    [Header("Position de la caméra")]
    public Vector3 offset = new Vector3(0f, 5f, -8f); // Décalage par rapport à la voiture

    [Header("Paramètres de lissage")]
    public float smoothSpeed = 0.125f; // Vitesse du "smooth follow"

    [Header("Rotation")]
    public bool lookAtTarget = true;   // Si la caméra doit regarder la voiture

    private void LateUpdate()
    {
        if (!target) return;

        // Position souhaitée
        Vector3 desiredPosition = target.position + target.TransformDirection(offset);

        // Interpolation douce vers la position
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        // Optionnel : la caméra regarde la voiture
        if (lookAtTarget)
            transform.LookAt(target);
    }
}
