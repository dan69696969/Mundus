using UnityEngine;
using UnityEngine.SceneManagement;
public class TutorialButton : MonoBehaviour
{
    public void LoadScene()
    {
        SceneManager.LoadScene("Tutorial");
    }
}