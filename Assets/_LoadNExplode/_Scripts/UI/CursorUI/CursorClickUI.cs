using UnityEngine;
using UnityEngine.InputSystem;
using PrimeTween;

namespace _LoadNExplode._Scripts.UI.CursorUI
{
    public class CursorClickUI : MonoBehaviour
    {
        [SerializeField] private ParticleSystem clickVfxPrefab;
        [SerializeField] private float maxVfxLifetime = 1.2f;

        private void Update() {
           HandleCursorClick();
        }

        private void HandleCursorClick() {
            if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame) {
                // Vector2 screenPos = Mouse.current.position.ReadValue();
                // Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, 0));
                // worldPos.z = 0f; 
                SpawnClickVFX(transform.position);
                Shake();
                //Audio 
                //Screenshake
            }
        }
        
        private void SpawnClickVFX(Vector3 position) {
            var vfxInstance = Instantiate(clickVfxPrefab, position, Quaternion.identity);
            vfxInstance.Play();
            Destroy(vfxInstance.gameObject, maxVfxLifetime);
        }
        
        //TODO: move this to camera script or something
        private void Shake(float strength = 0.3f, float duration = 0.3f, float frequency = 10f) {
            Tween.ShakeCamera(Camera.main, strengthFactor: strength, duration: duration, frequency: frequency);
        }

    }
}