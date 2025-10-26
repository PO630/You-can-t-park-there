using UnityEngine;
using UnityEngine.Rendering; // Nécessaire pour Volume
using System.Linq;

public class VolumeFinder : MonoBehaviour
{
    void Start()
    {
        // Trouve tous les composants Volume actifs dans la scène
        Volume[] volumes = FindObjectsOfType<Volume>(true);

        if (volumes.Length == 0)
        {
            Debug.Log("❌ Aucun Volume trouvé dans la scène !");
            return;
        }

        foreach (var vol in volumes)
        {
            Debug.Log($"🎨 Volume trouvé sur : {vol.gameObject.name} | Global: {vol.isGlobal}");
        }
    }
}
