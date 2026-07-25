using UnityEngine;

public static class AudioManager
{
    private static GameObject audioManagerObject;

    private static AudioSource musicSource;
    private static AudioSource ambientSource;

    private const float MinRandomPitch = 0.95f;
    private const float MaxRandomPitch = 1.05f;

    private const float MinRandomIntensity = 0.85f;
    private const float MaxRandomIntensity = 1f;

    private static void Initialize()
    {
        // Already initialized.
        if (audioManagerObject != null)
            return;

        audioManagerObject = new GameObject("AudioManager");

        Object.DontDestroyOnLoad(audioManagerObject);

        // Music source.
        musicSource = audioManagerObject.AddComponent<AudioSource>();
        musicSource.playOnAwake = false;
        musicSource.loop = true;
        musicSource.spatialBlend = 0f;

        // Ambient source.
        ambientSource = audioManagerObject.AddComponent<AudioSource>();
        ambientSource.playOnAwake = false;
        ambientSource.loop = true;
        ambientSource.spatialBlend = 0f;
    }

    // --------------------------------------------------
    // Music
    // Only one music track plays at a time.
    // --------------------------------------------------

    public static void PlayMusic(
        AudioClip clip,
        float volume = 1f)
    {
        if (clip == null)
            return;

        Initialize();

        // Do not restart the same music.
        if (musicSource.clip == clip && musicSource.isPlaying)
            return;

        musicSource.Stop();

        musicSource.clip = clip;
        musicSource.volume = Mathf.Clamp01(volume);
        musicSource.pitch = 1f;
        musicSource.loop = true;
        musicSource.spatialBlend = 0f;

        musicSource.Play();
    }

    public static void StopMusic()
    {
        Initialize();

        musicSource.Stop();
        musicSource.clip = null;
    }

    // --------------------------------------------------
    // 2D Sound
    // UI buttons and notifications.
    // Includes random pitch and intensity.
    // --------------------------------------------------

    public static void PlaySound(
        AudioClip clip,
        float volume = 1f)
    {
        if (clip == null)
            return;

        Initialize();

        CreateTemporarySound(
            clip,
            Vector3.zero,
            volume,
            is3D: false
        );
    }

    // --------------------------------------------------
    // 3D Sound
    // Plays from a position in the world.
    // Includes random pitch and intensity.
    // --------------------------------------------------

    public static void PlaySoundAtPosition(
        AudioClip clip,
        Vector3 position,
        float volume = 1f)
    {
        if (clip == null)
            return;

        Initialize();

        CreateTemporarySound(
            clip,
            position,
            volume,
            is3D: true
        );
    }

    // --------------------------------------------------
    // 2D Ambient
    // Only one ambient clip plays at a time.
    // --------------------------------------------------

    public static void PlayAmbient(
        AudioClip clip,
        float volume = 1f)
    {
        if (clip == null)
            return;

        Initialize();

        ambientSource.Stop();

        ambientSource.transform.localPosition = Vector3.zero;
        ambientSource.clip = clip;
        ambientSource.volume = Mathf.Clamp01(volume);
        ambientSource.pitch = 1f;
        ambientSource.loop = true;
        ambientSource.spatialBlend = 0f;

        ambientSource.Play();
    }

    // --------------------------------------------------
    // 3D Ambient
    // Only one ambient clip plays at a time.
    // --------------------------------------------------

    public static void PlayAmbientAtPosition(
        AudioClip clip,
        Vector3 position,
        float volume = 1f)
    {
        if (clip == null)
            return;

        Initialize();

        ambientSource.Stop();

        ambientSource.transform.position = position;
        ambientSource.clip = clip;
        ambientSource.volume = Mathf.Clamp01(volume);
        ambientSource.pitch = 1f;
        ambientSource.loop = true;
        ambientSource.spatialBlend = 1f;

        ambientSource.minDistance = 1f;
        ambientSource.maxDistance = 20f;
        ambientSource.rolloffMode =
            AudioRolloffMode.Logarithmic;

        ambientSource.Play();
    }

    public static void StopAmbient()
    {
        Initialize();

        ambientSource.Stop();
        ambientSource.clip = null;
    }

    // --------------------------------------------------
    // Temporary sound helper
    // --------------------------------------------------

    private static void CreateTemporarySound(
        AudioClip clip,
        Vector3 position,
        float volume,
        bool is3D)
    {
        GameObject soundObject =
            new GameObject($"Sound - {clip.name}");

        soundObject.transform.position = position;

        AudioSource source =
            soundObject.AddComponent<AudioSource>();

        float randomPitch = Random.Range(
            MinRandomPitch,
            MaxRandomPitch
        );

        float randomIntensity = Random.Range(
            MinRandomIntensity,
            MaxRandomIntensity
        );

        source.clip = clip;

        source.volume = Mathf.Clamp01(
            volume * randomIntensity
        );

        source.pitch = randomPitch;
        source.loop = false;
        source.playOnAwake = false;

        if (is3D)
        {
            source.spatialBlend = 1f;
            source.minDistance = 1f;
            source.maxDistance = 20f;
            source.rolloffMode =
                AudioRolloffMode.Logarithmic;
        }
        else
        {
            source.spatialBlend = 0f;
        }

        source.Play();

        float duration =
            clip.length / Mathf.Abs(randomPitch);

        Object.Destroy(
            soundObject,
            duration + 0.1f
        );
    }
}