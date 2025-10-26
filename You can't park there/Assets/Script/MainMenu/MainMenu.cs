using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// Gère le menu principal du jeu : lancement de niveaux, chargement asynchrone et sortie.
/// </summary>
public class MainMenu : MonoBehaviour
{
    [Header("Références UI")]
    [Tooltip("Panel contenant les boutons du menu principal.")]
    public GameObject mainMenuPanel;

    [Tooltip("Panel affichant la liste des niveaux.")]
    public GameObject levelPanel;

    [Tooltip("Panel ou image indiquant le chargement en cours.")]
    public GameObject loadingPanel;

    [Header("Paramètres")]
    [Tooltip("Nom du premier niveau à charger.")]
    public string firstLevelName = "Level_1";

    // Indique si un chargement est déjà en cours
    private bool isLoading = false;

    /// <summary>
    /// Quitte l'application (fonctionne uniquement dans un build).
    /// </summary>
    public void QuitGame()
    {
        isLoading = true ;
        Debug.Log("🛑 Quitter le jeu");
        Application.Quit();
    }

    /// <summary>
    /// Lance le premier niveau défini dans l’inspecteur.
    /// </summary>
    public void PlayFirstLevel()
    {
        if(isLoading) return;
        LoadLevel(firstLevelName);
    }

    /// <summary>
    /// Lance un niveau spécifique de façon asynchrone.
    /// Empêche toute interaction UI pendant le chargement.
    /// </summary>
    /// <param name="sceneName">Nom exact de la scène à charger.</param>
    public void LoadLevel(string sceneName)
    {
        if (isLoading) return; // évite les doublons
        StartCoroutine(LoadLevelAsync(sceneName));
    }

    /// <summary>
    /// Coroutine gérant le chargement asynchrone du niveau.
    /// </summary>
    private IEnumerator LoadLevelAsync(string sceneName)
    {
        isLoading = true;
        HideAllPanels();

        if (loadingPanel != null)
            loadingPanel.SetActive(true);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false;

        // Attend que le chargement soit presque fini
        while (asyncLoad.progress < 0.9f)
        {
            yield return null;
        }

        // Activation de la scène
        asyncLoad.allowSceneActivation = true;
    }

    /// <summary>
    /// Affiche le panel listant les niveaux disponibles.
    /// </summary>
    public void ShowLevelPanel()
    {
        if (levelPanel != null)
            levelPanel.SetActive(true);

        if (mainMenuPanel != null)
            mainMenuPanel.SetActive(false);
    }

    /// <summary>
    /// Ferme le panel des niveaux et revient au menu principal.
    /// </summary>
    public void HideLevelPanel()
    {
        if (levelPanel != null)
            levelPanel.SetActive(false);

        if (mainMenuPanel != null)
            mainMenuPanel.SetActive(true);
    }

    /// <summary>
    /// Ferme tous les panels du menu.
    /// </summary>
    public void HideAllPanels()
    {
        if (levelPanel != null)
            levelPanel.SetActive(false);

        if (mainMenuPanel != null)
            mainMenuPanel.SetActive(false);
    }


}
