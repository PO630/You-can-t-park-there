using UnityEngine;

/// <summary>
/// Contrôle physique de la voiture.
/// Gère l'accélération, la direction et la friction de manière réaliste.
/// Empêche la voiture de tourner sur place et limite la vitesse maximale.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class CarController : MonoBehaviour
{
    [Header("Paramètres de conduite")]

    /// <summary>
    /// Force appliquée à la voiture lors de l'accélération (en Newtons).
    /// </summary>
    [Tooltip("Force appliquée lors de l'accélération (N).")]
    public float accelerationForce = 1500f;

    /// <summary>
    /// Angle maximum de braquage (en degrés).
    /// </summary>
    [Tooltip("Angle maximum de braquage (°).")]
    public float steeringAngle = 25f;

    /// <summary>
    /// Vitesse maximale de la voiture (en m/s).
    /// </summary>
    [Tooltip("Vitesse maximale de la voiture (m/s).")]
    public float maxSpeed = 20f;

    /// <summary>
    /// Facteur de ralentissement appliqué à chaque frame pour simuler la friction (entre 0 et 1).
    /// </summary>
    [Tooltip("Coefficient de friction générale (0.98 = légère résistance).")]
    public float dragFactor = 0.98f;

    /// <summary>
    /// Vitesse de rotation du véhicule (influence la réactivité de la direction).
    /// </summary>
    [Tooltip("Vitesse de rotation du véhicule lors des virages.")]
    public float turnSpeed = 3f;

    // --- Variables privées ---
    private Rigidbody rb;      // Référence au Rigidbody pour la physique
    private float moveInput;   // Entrée avant/arrière (Z/S)
    private float steerInput;  // Entrée gauche/droite (Q/D)

    /// <summary>
    /// Initialise les composants et ajuste le centre de gravité pour améliorer la stabilité.
    /// </summary>
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = new Vector3(0, -0.5f, 0); // Abaisse le centre de gravité
    }

    /// <summary>
    /// Récupère les entrées utilisateur à chaque frame.
    /// </summary>
    void Update()
    {
        // Entrées clavier (Z/S pour avancer/reculer, Q/D pour tourner)
        moveInput = Input.GetAxis("Vertical");
        steerInput = Input.GetAxis("Horizontal");
    }

    /// <summary>
    /// Applique les forces physiques et met à jour la direction du véhicule.
    /// </summary>
    void FixedUpdate()
    {
        // Limite la vitesse maximale
        if (rb.linearVelocity.magnitude > maxSpeed)
            rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;

        // Applique la force d'accélération dans la direction avant du véhicule
        Vector3 forward = transform.forward * moveInput * accelerationForce * Time.fixedDeltaTime;
        rb.AddForce(forward, ForceMode.Force);

        // Tourne seulement si la voiture roule (évite la rotation sur place)
        if (rb.linearVelocity.magnitude > 0.1f)
        {
            float steerAmount = steerInput * turnSpeed * (rb.linearVelocity.magnitude / maxSpeed);
            Quaternion turnOffset = Quaternion.Euler(0f, steerAmount, 0f);
            rb.MoveRotation(rb.rotation * turnOffset);
        }

        // Simule une friction progressive pour ralentir naturellement la voiture
        rb.linearVelocity *= dragFactor;
    }
}
