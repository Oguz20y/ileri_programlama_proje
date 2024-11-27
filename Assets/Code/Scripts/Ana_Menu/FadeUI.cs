using System.Collections;
using UnityEngine;

public class FadeUI : MonoBehaviour
{
    public float fadeDuration = 2f; // Görünürlük geçiþ süresi
    private CanvasGroup canvasGroup;

    private void Start()
    {
        // Canvas Group'u al
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0; // Ýlk olarak tamamen saydam yap
            StartCoroutine(FadeIn());
        }
        else
        {
            Debug.LogError("Canvas Group bulunamadý! Lütfen Canvas Group bileþeni ekleyin.");
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

        canvasGroup.alpha = 1; // Tamamen görünür yap
    }
}
