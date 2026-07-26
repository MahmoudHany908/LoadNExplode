using System;
using System.Collections;
using PrimeTween;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    
    
    [Header("UI Elements")]
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private RectTransform bombMask;
    // [SerializeField] private Image progressBar;
    // [SerializeField] private TextMeshProUGUI progressText;

    [Header("Settings")]
    [SerializeField] private float showDuration = 1f;
    [SerializeField] private float hideDuration = 0.8f;
    private float maskScale = 36.0f;
    

    
    private Sequence _bombSequence;

    private void Awake()
    {
        // if (canvasGroup != null) canvasGroup.gameObject.SetActive(false);
        // if (bombLoading != null) bombLoading.localScale = Vector3.zero;

        // if (progressBar != null) progressBar.fillAmount = 0;
        // if (progressText != null) progressText.text = "0%";
    }

    private void Start() {
        
    }

    public void Disable() {
        canvasGroup.gameObject.SetActive(false);
    }

    public IEnumerator ShowAsync()
    {
        if (canvasGroup != null) canvasGroup.gameObject.SetActive(true);

        Debug.Log("a");
        
        _bombSequence.Stop();
        _bombSequence = CreateBombSequence(fromScale: maskScale, toScale: 0, showDuration);

        yield return _bombSequence.ToYieldInstruction();
    }

    public IEnumerator HideAsync()
    {
        _bombSequence.Stop();
        _bombSequence = CreateBombSequence(fromScale: 0, toScale: maskScale, hideDuration);

        yield return _bombSequence.ToYieldInstruction();

        if (canvasGroup != null) canvasGroup.gameObject.SetActive(false);
    }

    public void UpdateProgress(float normalizedProgress)
    {
        // if (progressBar != null) progressBar.fillAmount = normalizedProgress;
        // if (progressText != null) progressText.text = $"{Mathf.RoundToInt(normalizedProgress * 100)}%";
    }

    private Sequence CreateBombSequence(float fromScale, float toScale, float duration)
    {
        if (bombMask != null)
            bombMask.localScale = Vector3.one * fromScale;

        return Sequence.Create()
            .Group(Tween.Scale(bombMask, endValue: Vector3.one * toScale, duration: duration, ease: Ease.OutQuad));
    }
}