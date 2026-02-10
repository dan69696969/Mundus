using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHistoryManager : MonoBehaviour
{
    // Statická promìnná si pamatuje index scény po celou dobu bìhu hry
    private static int lastSceneIndex = 0;

    // Singleton – zajistí, že ve høe bude jen jeden tento manažer
    public static SceneHistoryManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Objekt pøežije pøechod mezi scénami
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        // Detekce stisknutí klávesy B
        if (Input.GetKeyDown(KeyCode.B))
        {
            ReturnToPrevious();
        }
    }

    // Tuto metodu používej místo SceneManager.LoadScene, aby se uložila historie
    public void LoadNewScene(int sceneIndex)
    {
        lastSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(sceneIndex);
    }

    public void ReturnToPrevious()
    {
        Debug.Log("Vracím se do scény: " + lastSceneIndex);
        SceneManager.LoadScene(lastSceneIndex);

        // Volitelné: Po návratu resetuj index na Menu (0), aby se hráè "nezacyklil"
        // lastSceneIndex = 0; 
    }
}