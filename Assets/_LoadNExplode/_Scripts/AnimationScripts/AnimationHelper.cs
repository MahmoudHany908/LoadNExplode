using UnityEngine;

/// <summary>
/// A static helper class providing safe, clean extension methods for Unity Animations.
/// Attach this script anywhere in your Assets folder (no need to attach to a GameObject).
/// </summary>
public static class AnimationHelper
{


    /// <summary>
    /// Safely crossfades to a new state. Does nothing if the Animator is null.
    /// </summary>

    public static void SafeCrossFade(this Animator animator, string stateName, float transitionDuration, int layer = 0)
    {
        if (animator != null && animator.gameObject.activeInHierarchy)
        {
            animator.CrossFade(stateName, transitionDuration, layer);
        }
        else
        {
            Debug.LogWarning($"[AnimationHelper] SafeCrossFade failed: Animator is null or GameObject is inactive.");
        }
    }

    /// <summary>
    /// Optimized version of SafeCrossFade using State Hashes (faster performance).
    /// </summary>
    public static void SafeCrossFadeHash(this Animator animator, int stateHash, float transitionDuration, int layer = 0)
    {
        if (animator != null && animator.gameObject.activeInHierarchy)
        {
            animator.CrossFade(stateHash, transitionDuration, layer);
        }
    }

    /// <summary>
    /// Safely plays an animation state immediately without blending.
    /// </summary>
    public static void SafePlay(this Animator animator, string stateName, int layer = 0, float normalizedTime = 0f)
    {
        if (animator != null && animator.gameObject.activeInHierarchy)
        {
            animator.Play(stateName, layer, normalizedTime);
        }
    }

    /// <summary>
    /// Checks if the Animator is currently playing a specific state.
    /// </summary>
    public static bool IsPlaying(this Animator animator, string stateName, int layer = 0)
    {
        if (animator == null || !animator.gameObject.activeInHierarchy) return false;

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(layer);
        // Check if the state name matches (and optionally check if it's in the middle of playing)
        return stateInfo.IsName(stateName);
    }

    // =========================================================================
    // LEGACY ANIMATION COMPONENT EXTENSIONS (Optional)
    // Use these only if you are using the old "Animation" component, not "Animator"
    // =========================================================================

    /// <summary>
    /// Safely crossfades using the legacy Animation component.
    /// </summary>
    public static void SafeCrossFade(this Animation animation, string animationName, float fadeLength = 0.3f)
    {
        if (animation != null && animation.gameObject.activeInHierarchy && animation[animationName] != null)
        {
            animation.CrossFade(animationName, fadeLength);
        }
    }

    /// <summary>
    /// Checks if the legacy Animation component is currently playing a specific clip.
    /// </summary>
    public static bool IsPlaying(this Animation animation, string animationName)
    {
        if (animation == null || !animation.gameObject.activeInHierarchy) return false;
        return animation.IsPlaying(animationName);
    }
}