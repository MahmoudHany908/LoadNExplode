using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Image progressBar;
    [SerializeField] private TextMeshProUGUI progressText;

    [Header("Settings")]
    [SerializeField] private float fadeDuration = 0.5f;

    private void Awake()
    {

        if (canvasGroup != null) canvasGroup.alpha = 0;
        if (progressBar != null) progressBar.fillAmount = 0;
        if (progressText != null) progressText.text = "0%";
    }

    public IEnumerator ShowAsync()
    {
        if (canvasGroup != null) canvasGroup.gameObject.SetActive(true);
        yield return Fade(0f, 1f);
    }

    public IEnumerator HideAsync()
    {
        yield return Fade(1f, 0f);
        if (canvasGroup != null) canvasGroup.gameObject.SetActive(false);
    }

    public void UpdateProgress(float normalizedProgress)
    {
        if (progressBar != null) progressBar.fillAmount = normalizedProgress;
        if (progressText != null) progressText.text = $"{Mathf.RoundToInt(normalizedProgress * 100)}%";
    }

    private IEnumerator Fade(float startAlpha, float endAlpha)
    {
        float timeElapsed = 0f;

        while (timeElapsed < fadeDuration)
        {
            timeElapsed += Time.unscaledDeltaTime;

            if (canvasGroup != null)
                canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, timeElapsed / fadeDuration);

            yield return null;
        }

        if (canvasGroup != null) canvasGroup.alpha = endAlpha;
    }
}