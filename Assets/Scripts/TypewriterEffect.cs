using UnityEngine;
using System.Collections;
using TMPro;

public class TypewriterEffect : MonoBehaviour
{
    public TextMeshProUGUI textMeshPro;
    public float delay = 0.05f;

    private string fullText;
    private Coroutine typingCoroutine;

    private void Start()
    {
        fullText = textMeshPro.text; 
        textMeshPro.text = "";

        StartTyping();
    }

    public void StartTyping()
    {
        if (typingCoroutine != null) StopCoroutine(typingCoroutine);
        typingCoroutine = StartCoroutine(TypeText());
    }

    private IEnumerator TypeText()
    {
        textMeshPro.text = "";
        for (int i = 0; i < fullText.Length; i++)
        {
            textMeshPro.text += fullText[i];
            yield return new WaitForSeconds(delay);
        }
    }
}
