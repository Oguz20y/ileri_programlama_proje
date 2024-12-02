using System.Collections;
using UnityEngine;

public class FadeUI : MonoBehaviour
{
    public float fadeDuration = 2f; // G�r�n�rl�k ge�i� s�resi
    private CanvasGroup canvasGroup;

    private void Start()
    {
        // Canvas Group'u al
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0; // �lk olarak tamamen saydam yap
            StartCoroutine(FadeIn());
        }
        else
        {
            Debug.LogError("Canvas Group bulunamad�! L�tfen Canvas Group bile�eni ekleyin.");
        }
    }

    private IEnumerator FadeIn()
    {
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            canvasGroup.alpha = Mathf.Clamp01(elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = 1; // Tamamen g�r�n�r yap
    }
}
