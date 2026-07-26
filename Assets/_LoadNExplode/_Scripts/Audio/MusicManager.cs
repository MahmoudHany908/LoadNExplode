using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _LoadNExplode._Scripts.Audio
{
    public class MusicManager : MonoBehaviour
    {
        [Header("Music Settings")]
        [SerializeField] private AudioClip MainMenu;
        [SerializeField] private AudioClip GameLoop;
        [SerializeField] private AudioClip AccGameLoop;
        
        [Header("SFX Settings")]
        [SerializeField] private List<AudioClip> footsteps;
        [SerializeField] private AudioClip Explosion;
        [SerializeField] private AudioClip Death;
        [SerializeField] private AudioClip Coin;
        
        public static MusicManager Instance { get; private set; }
        
        private void Awake() {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void Start() {
            PlayMainMenuMusic();
        }

        public void PlayMainMenuMusic() {
            AudioManager.PlayMusic(MainMenu, 0.2f);
        }
        
        public void PlayGameLoopMusic() {
            AudioManager.PlayMusic(GameLoop, 0.2f);
        }
        
        public void PlayAccGameLoopMusic() {
            AudioManager.PlayMusic(AccGameLoop, 0.2f);
        }

        public void PlayExplosion(float volume = 0.2f) {
            AudioManager.PlaySound(Explosion, volume);
        }

        public void PlayFootstep() {
            int index = Random.Range(0, footsteps.Count);
            var footstepAudio = footsteps[index];
            AudioManager.PlaySound(footstepAudio, 0.5f);
        }

        public void PlayDeath() {
            AudioManager.StopMusic();
            AudioManager.PlaySound(Death);
        }

        public void PlayCoin() {
            AudioManager.PlaySound(Coin, 0.6f);
        }
    }
}