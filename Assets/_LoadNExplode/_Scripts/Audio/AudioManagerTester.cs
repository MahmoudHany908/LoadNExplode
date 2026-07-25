using UnityEngine;
using UnityEngine.InputSystem;

public class AudioManagerTester : MonoBehaviour
{
    [Header("Assign Your Clips")]
    [SerializeField] private AudioClip birdSound;
    [SerializeField] private AudioClip trumpetLoop;

    [Header("Optional 3D Sound Position")]
    [SerializeField] private Transform soundPosition;

    private void Update()
    {
        Keyboard keyboard = Keyboard.current;

        if (keyboard == null)
            return;

        // 1: Play trumpet as music.
        if (keyboard.digit1Key.wasPressedThisFrame)
        {
            AudioManager.PlayMusic(trumpetLoop, 0.5f);
            Debug.Log("Playing trumpet as music");
        }

        // 2: Stop music.
        if (keyboard.digit2Key.wasPressedThisFrame)
        {
            AudioManager.StopMusic();
            Debug.Log("Music stopped");
        }

        // 3: Play bird as a 2D sound effect.
        if (keyboard.digit3Key.wasPressedThisFrame)
        {
            AudioManager.PlaySound(birdSound);
            Debug.Log("Playing bird as a 2D sound");
        }

        // 4: Play bird as a 3D sound effect.
        if (keyboard.digit4Key.wasPressedThisFrame)
        {
            Vector3 position = soundPosition != null
                ? soundPosition.position
                : transform.position;

            AudioManager.PlaySoundAtPosition(
                birdSound,
                position
            );

            Debug.Log("Playing bird as a 3D sound");
        }

        // 5: Play trumpet as global 2D ambient.
        if (keyboard.digit5Key.wasPressedThisFrame)
        {
            AudioManager.PlayAmbient(trumpetLoop, 0.5f);
            Debug.Log("Playing trumpet as 2D ambient");
        }

        // 6: Play trumpet as positional 3D ambient.
        if (keyboard.digit6Key.wasPressedThisFrame)
        {
            Vector3 position = soundPosition != null
                ? soundPosition.position
                : transform.position;

            AudioManager.PlayAmbientAtPosition(
                trumpetLoop,
                position,
                0.5f
            );

            Debug.Log("Playing trumpet as 3D ambient");
        }

        // 7: Stop ambient.
        if (keyboard.digit7Key.wasPressedThisFrame)
        {
            AudioManager.StopAmbient();
            Debug.Log("Ambient stopped");
        }
    }
}