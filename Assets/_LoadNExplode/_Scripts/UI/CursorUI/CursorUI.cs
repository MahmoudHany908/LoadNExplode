// using DG.Tweening;
// using Managers;

using UnityEngine;
using UnityEngine.InputSystem;

// using Utilities.Extensions;

namespace _LoadNExplode._Scripts.UI.CursorUI
{
    public class CursorUI : MonoBehaviour
    {
        [SerializeField] private bool isEnabled = true;
        private Camera canvasCamera;
        private RectTransform canvasRect;
        private RectTransform cursorRect;
        private Canvas canvas;
        // private Tween showTween;

        void Awake() {
            canvas = GetComponentInParent<Canvas>();
            canvasRect = canvas.GetComponent<RectTransform>();
            cursorRect = GetComponent<RectTransform>();
            // cursorImage = GetComponent<Image>();
            canvasCamera = canvas.worldCamera;
            EnableUICursor(); //for now...
            SetupTweens();
        }

        private void Update() {
            HandleRawCurosr();
        }

        private void HandleRawCurosr() {
            if (isEnabled && Mouse.current != null) UpdateCursorUIPos(Mouse.current.position.ReadValue());
        }

        public Vector2 ScreenToUI(Vector2 cursorPos) {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvasRect,
                cursorPos,
                canvasCamera,
                out var localUIPos
            );
            return localUIPos;
        }

        private void UpdateCursorUIPos(Vector2 cursorPos) {
            var cursorScreenPos = ScreenToUI(cursorPos);
            FollowCursor(cursorScreenPos);
        }

        private void SetupTweens() {
            // showTween = cursorImage.DOFade(1, showDuration).SetEase(Ease.InCubic).SetManual(gameObject);
        }
        
        private void FollowCursor(Vector2 cursorPos) {
            cursorRect.anchoredPosition = cursorPos;
        }

        public void EnableUICursor() {
            isEnabled = true;
            Cursor.visible = false;
            // showTween.Restart();
            // showTween.Play();
        }

        public void HideCursor() {
            isEnabled = false;
            Cursor.visible = true;
            // showTween.PlayBackwards();
        }
    }
}
