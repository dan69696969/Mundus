using UnityEngine;
using UnityEngine.SceneManagement; // Dùležité pro práci se scénami

public class PortalController : MonoBehaviour
{
    // Do této promìnné v Unity Editoru napíšete název scény, kam má portál vést.
    public string sceneNameToLoad;

    // Tato metoda se automaticky zavolá, když nìjaký jiný Collider2D vstoupí do našeho Triggeru.
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Zkontrolujeme, zda objekt, který vstoupil, má tag "Player".
        if (other.CompareTag("Player"))
        {
            Debug.Log("Hráè vstoupil do portálu, naèítám scénu: " + sceneNameToLoad);
            // Naète scénu, jejíž název je uložen v promìnné sceneNameToLoad.
            SceneManager.LoadScene(sceneNameToLoad);
        }
    }
}