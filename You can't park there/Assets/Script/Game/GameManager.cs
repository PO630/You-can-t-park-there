using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Diagnostics;

/// <summary>
/// Gère les états principaux du jeu : pause, victoire, défaite et retour au menu.
/// Contient une instance unique (singleton) accessible depuis tout le projet.
/// </summary>
public class GameManager : MonoBehaviour
{
    // --- Instance unique ---
    public static GameManager Instance { get; private set; }

    [Header("Panneaux UI")]
    [Tooltip("Panel du menu pause (affiché avec Échap).")]
    public GameObject pausePanel;

    [Tooltip("Panel affiché lors d'une victoire.")]
    public GameObject winPanel;

    [Tooltip("Panel affiché lors d'une défaite.")]
    public GameObject losePanel;

    [Header("Paramètres")]
    [Tooltip("Nom de la scène du menu principal.")]
    public string mainMenuScene = "MainMenu";

    [Tooltip("Nom de la scène suivante.")]
    public string nextLevelScene = "";

    // État du jeu
    private bool isPaused = false;
    private bool isGameOver = false;

    private void Awake()
    {
        // Singleton : assure qu'une seule instance existe
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Update()
    {
        // Ouvre ou ferme le menu pause avec Échap
        if (Input.GetKeyDown(KeyCode.Escape) && !isGameOver)
        {
            if (!isPaused)
                PauseGame();
            else
                ResumeGame();
        }
    }

    /// <summary>
    /// Met le jeu en pause, affiche le menu pause et arrête le temps.
    /// </summary>
    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f;

        if (pausePanel != null)
            pausePanel.SetActive(true);

    }

    /// <summary>
    /// Reprend le jeu après une pause.
    /// </summary>
    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;

        if (pausePanel != null)
            pausePanel.SetActive(false);

    }

    /// <summary>
    /// Arrête le temps et affiche le panneau de victoire.
    /// </summary>
    public void WinGame()
    {
        if (isGameOver) return;
        isGameOver = true;

        Time.timeScale = 0f;


        if (winPanel != null)
            winPanel.SetActive(true);

        UnityEngine.Debug.Log("🏆 Gagné !");

        if( nextLevelScene != null && nextLevelScene != "")
        {
            UnityEngine.Debug.Log($"➡️ Chargement du niveau suivant : {nextLevelScene}");
            StartCoroutine(LoadNextLevelAsyncAfterDelay(nextLevelScene , 2f));
        }
        else
        {
            UnityEngine.Debug.Log("🏁 Pas de niveau suivant défini — retour au menu principal.");
            StartCoroutine(LoadNextLevelAsyncAfterDelay(mainMenuScene , 2f));
        }
    }

    /// <summary>
    /// Arrête le temps et affiche le panneau de défaite, puis recharge la scène après un délai.
    /// </summary>
    public void LoseGame()
    {
        if (isGameOver) return;
        isGameOver = true;

        Time.timeScale = 0f;

        if (losePanel != null)
            losePanel.SetActive(true);

        UnityEngine.Debug.Log("💥 Perdu !");
        StartCoroutine(RestartLevelAfterDelay(2f));
    }

    /// <summary>
    /// Recharge le niveau actuel après un certain délai (pendant lequel le temps est figé).
    /// </summary>
    private IEnumerator RestartLevelAfterDelay(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);

        Time.timeScale = 1f; // remet le temps avant de recharger
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    /// <summary>
    /// Charge un niveau spécifique (nommé <paramref name="nextScene"/>) de manière asynchrone après un délai.
    /// Si la scène n’existe pas, retourne au menu principal.
    /// </summary>
    /// <param name="nextScene">Nom de la scène suivante à charger.</param>
    /// <param name="delay">Délai en secondes réelles avant le chargement.</param>
    private IEnumerator LoadNextLevelAsyncAfterDelay(string nextScene, float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        Time.timeScale = 1f; // réactive le temps avant le chargement

        if (!string.IsNullOrEmpty(nextScene))
        {
            UnityEngine.Debug.Log($"Chargement asynchrone de la scène suivante : {nextScene}");
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(nextScene);

            // On bloque toute interaction jusqu’à la fin du chargement
            asyncLoad.allowSceneActivation = true;
            yield return asyncLoad;
        }
        else
        {
            UnityEngine.Debug.Log("🏁 Nom de scène suivant invalide — retour au menu principal.");
            SceneManager.LoadScene(mainMenuScene);
        }
    }


    /// <summary>
    /// Retourne au menu principal.
    /// </summary>
    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuScene);
    }

    /// <summary>
    /// Masque tous les panneaux (utile au démarrage ou reset).
    /// </summary>
    public void HideAllPanels()
    {
        if (pausePanel != null) pausePanel.SetActive(false);
        if (winPanel != null) winPanel.SetActive(false);
        if (losePanel != null) losePanel.SetActive(false);
    }
}
