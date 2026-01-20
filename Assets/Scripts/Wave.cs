using UnityEngine;
using TMPro; // Nutné pro práci s TextMeshPro

public class Wave : MonoBehaviour
{
    [Header("Nastavení vlnìní")]
    public float speed = 3f;      // Jak rychle se vlna pohybuje
    public float height = 5f;     // Jak vysoko písmena skáèou
    public float spread = 0.1f;   // Jak moc jsou od sebe vlny posunuté

    private TMP_Text textComponent;

    void Start()
    {
        textComponent = GetComponent<TMP_Text>();
    }

    void Update()
    {
        // Vynutíme aktualizaci textu, abychom mìli pøístup k datùm o písmenech
        textComponent.ForceMeshUpdate();
        var textInfo = textComponent.textInfo;

        // Projdeme každý znak v textu
        for (int i = 0; i < textInfo.characterCount; i++)
        {
            var charInfo = textInfo.characterInfo[i];

            // Pøeskoèíme neviditelné znaky (mezery)
            if (!charInfo.isVisible)
                continue;

            // Získáme indexy vrcholù (každé písmeno má 4 vrcholy/rohy)
            int vertexIndex = charInfo.vertexIndex;
            var meshInfo = textInfo.meshInfo[charInfo.materialReferenceIndex];

            // Výpoèet vlnìní pomocí Sinus
            // Používáme x-ovou pozici písmene, aby každé písmeno bylo v jiné fázi vlny
            float offset = Mathf.Sin(Time.time * speed + charInfo.bottomLeft.x * spread) * height;

            // Aplikujeme posun na všechny 4 vrcholy daného písmene
            for (int j = 0; j < 4; j++)
            {
                Vector3 originalPosition = meshInfo.vertices[vertexIndex + j];
                meshInfo.vertices[vertexIndex + j] = originalPosition + new Vector3(0, offset, 0);
            }
        }

        // Aktualizujeme renderer s novými daty o vrcholech
        for (int i = 0; i < textInfo.meshInfo.Length; i++)
        {
            textInfo.meshInfo[i].mesh.vertices = textInfo.meshInfo[i].vertices;
            textComponent.UpdateGeometry(textInfo.meshInfo[i].mesh, i);
        }
    }
}