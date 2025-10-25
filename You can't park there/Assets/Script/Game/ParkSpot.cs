using UnityEngine;

/// <summary>
/// Représente une zone de stationnement.
/// Le joueur gagne s'il garde la majorité de son véhicule dans cette zone
/// pendant un temps donné (parkTime).
/// </summary>
[RequireComponent(typeof(Collider))]
public class ParkSpot : MonoBehaviour
{
    [Header("Paramètres du stationnement")]
    [Tooltip("Durée (en secondes) pendant laquelle la voiture doit rester garée pour gagner.")]
    public float parkTime = 2f;

    [Tooltip("Pourcentage minimum de la voiture devant être dans la zone pour valider le stationnement (de 0 à 1).")]
    [Range(0.1f, 1f)]
    public float requiredCoverage = 0.7f;

    // Chronomètre pour compter combien de temps la voiture reste bien garée
    private float timer = 0f;

    /// <summary>
    /// Appelé à chaque frame physique tant qu'un collider est dans la zone (Is Trigger activé).
    /// </summary>
    /// <param name="other">Le collider qui reste dans la zone de parking.</param>
    private void OnTriggerStay(Collider other)
    {
        // On ne s'intéresse qu'à la voiture (tag "Player")
        if (!other.CompareTag("Player"))
            return;

        // Récupère le collider de la voiture
        Collider carCollider = other.GetComponent<Collider>();
        if (carCollider == null)
            return;

        // Vérifie si une bonne partie de la voiture est bien dans la zone
        if (IsMostlyInside(carCollider))
        {
            // Incrémente le temps passé dans la zone
            timer += Time.deltaTime;

            // Si le joueur est resté assez longtemps → victoire
            if (timer >= parkTime)
            {
                Debug.Log("🏆 Gagné ! La voiture est bien garée !");
                enabled = false; // désactive le script pour éviter de rejouer la victoire
            }
        }
        else
        {
            // Si la voiture ressort partiellement → on réinitialise le timer
            timer = 0f;
        }
    }

    /// <summary>
    /// Réinitialise le timer lorsque la voiture quitte complètement la zone.
    /// </summary>
    /// <param name="other">Le collider sortant.</param>
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            timer = 0f;
        }
    }

    /// <summary>
    /// Vérifie si une proportion suffisante du collider de la voiture
    /// est à l'intérieur du collider du ParkSpot.
    /// Cette méthode utilise une approximation basée sur les bounding boxes.
    /// </summary>
    /// <param name="carCollider">Le collider de la voiture.</param>
    /// <returns>True si la couverture dépasse le seuil requis, sinon False.</returns>
    private bool IsMostlyInside(Collider carCollider)
    {
        // Boîtes englobantes du parking et de la voiture
        Bounds spotBounds = GetComponent<Collider>().bounds;
        Bounds carBounds = carCollider.bounds;

        // Calcule la zone d’intersection entre les deux bounding boxes
        Bounds intersection = new Bounds();
        intersection.SetMinMax(
            Vector3.Max(spotBounds.min, carBounds.min),
            Vector3.Min(spotBounds.max, carBounds.max)
        );

        // Si pas d’intersection, la voiture est complètement dehors
        if (intersection.size.x <= 0 || intersection.size.z <= 0)
            return false;

        // Aire du véhicule et aire de l’intersection (on ignore la hauteur Y)
        float carArea = carBounds.size.x * carBounds.size.z;
        float intersectArea = intersection.size.x * intersection.size.z;

        // Pourcentage de surface du véhicule présente dans la zone
        float coverage = intersectArea / carArea;

        // True si la couverture est suffisante
        return coverage >= requiredCoverage;
    }
}
