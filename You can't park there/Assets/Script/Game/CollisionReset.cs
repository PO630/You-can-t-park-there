using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Détecte la collision entre la voiture et un obstacle.
/// Lorsque la voiture entre en collision, relance la scène après un délai.
/// </summary>
public class CollisionReset : MonoBehaviour
{
    [Header("Paramètres de collision")]
    [Tooltip("Temps d'attente avant de relancer la scène (en secondes).")]
    public float restartDelay = 1f;

    private bool hasCollided = false; // Empêche plusieurs déclenchements

    /// <summary>
    /// Détecte la collision physique avec la voiture.
    /// </summary>
    /// <param name="collision">Informations sur la collision.</param>
    private void OnCollisionEnter(Collision collision)
    {
        // Vérifie si l'objet touché est la voiture (tag = "Player")
        if (collision.gameObject.CompareTag("Player") && !hasCollided)
        {
            hasCollided = true;
            Debug.Log("💥 Collision détectée avec la voiture !");
            StartCoroutine(RestartSceneAfterDelay());
        }
    }

    /// <summary>
    /// Coroutine qui attend un certain délai avant de recharger la scène actuelle.
    /// </summary>
    private System.Collections.IEnumerator RestartSceneAfterDelay()
    {
        yield return new WaitForSeconds(restartDelay);

        // Recharge la scène active
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }
}
