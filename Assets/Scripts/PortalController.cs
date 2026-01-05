using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class PortalController : MonoBehaviour
{
    [Header("Teleportace")]
    public string sceneNameToLoad;

    [Header("Co se má smazat pøi vstupu")]
    [Tooltip("Mùžeš psát více jmen oddìlených èárkou, napø: Portál1, Portál2")]
    public string objectToDestroyName;

    [Header("Podmínka pro Boss Portál")]
    public List<string> requiredObjectsToDestroy;
    public GameObject bossPortal;

    private static HashSet<string> destroyedObjectsLog = new HashSet<string>();

    private void Start()
    {
        CheckBossPortalCondition();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (!string.IsNullOrEmpty(objectToDestroyName))
            {
                // TADY JE TA ZMÌNA: Rozdìlíme text podle èárek
                string[] names = objectToDestroyName.Split(',');
                foreach (string name in names)
                {
                    string cleanName = name.Trim().ToLower();
                    if (!string.IsNullOrEmpty(cleanName))
                    {
                        destroyedObjectsLog.Add(cleanName);
                        Debug.Log("Uloženo do pamìti smazaných: " + cleanName);
                    }
                }
                CleanCurrentScene();
            }

            if (!string.IsNullOrEmpty(sceneNameToLoad))
            {
                SceneManager.LoadScene(sceneNameToLoad);
            }
        }
    }

    private void OnEnable() { SceneManager.sceneLoaded += OnSceneLoaded; }
    private void OnDisable() { SceneManager.sceneLoaded -= OnSceneLoaded; }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) { CleanCurrentScene(); }

    private void CleanCurrentScene()
    {
        GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();
        foreach (GameObject go in allObjects)
        {
            if (destroyedObjectsLog.Contains(go.name.ToLower()))
            {
                Destroy(go);
            }
        }
        CheckBossPortalCondition();
    }

    public void CheckBossPortalCondition()
    {
        if (bossPortal == null) return;
        CircleCollider2D portalCollider = bossPortal.GetComponent<CircleCollider2D>();
        if (portalCollider == null) return;
        if (requiredObjectsToDestroy.Count == 0) return;

        bool allDestroyed = true;
        foreach (string req in requiredObjectsToDestroy)
        {
            if (!destroyedObjectsLog.Contains(req.Trim().ToLower()))
            {
                allDestroyed = false;
                break;
            }
        }

        // Zapne/vypne collider podle toho, jestli je splnìno
        portalCollider.enabled = allDestroyed;

        SpriteRenderer sr = bossPortal.GetComponent<SpriteRenderer>();
        if (sr != null) sr.color = allDestroyed ? Color.white : new Color(1, 1, 1, 0.3f);

        Debug.Log("Boss Portál - Vše smazáno? " + (allDestroyed ? "ANO (Collider aktivní)" : "NE"));
    }
}