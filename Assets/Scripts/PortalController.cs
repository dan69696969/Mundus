using UnityEngine;
using UnityEngine.SceneManagement; // D�le�it� pro pr�ci se sc�nami

public class PortalController : MonoBehaviour
{
    // Do t�to prom�nn� v Unity Editoru nap�ete n�zev sc�ny, kam m� port�l v�st.
    public string sceneNameToLoad;

    // Tato metoda se automaticky zavol�, kdy� n�jak� jin� Collider2D vstoup� do na�eho Triggeru.
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Zkontrolujeme, zda objekt, kter� vstoupil, m� tag "Player".
        if (other.CompareTag("Player"))
        {
            Debug.Log("Hr�� vstoupil do port�lu, na��t�m sc�nu: " + sceneNameToLoad);
            // Na�te sc�nu, jej� n�zev je ulo�en v prom�nn� sceneNameToLoad.
            SceneManager.LoadScene(sceneNameToLoad);
        }
    }
}