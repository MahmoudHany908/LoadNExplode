using UnityEngine;

public class FloatyEffect : MonoBehaviour
{
    [Header("Floating (Up & Down)")]
    [Tooltip("How fast the object bobs up and down")]
    [SerializeField] private float floatSpeed = 1.5f;

    [Tooltip("How high the object floats")]
    [SerializeField] private float floatHeight = 0.5f;

    [Header("Rocking (Rotation)")]
    [Tooltip("How fast the object rocks back and forth")]
    [SerializeField] private float rockSpeed = 1.0f;

    [Tooltip("The maximum angle it tilts while floating")]
    [SerializeField] private float rockAngle = 5.0f;

    [Header("Randomization")]
    [Tooltip("Turn this on if you have multiple objects so they don't all float in perfect sync")]
    [SerializeField] private bool randomizeOffset = true;

    private Vector3 startPosition;
    private Quaternion startRotation;
    private float randomOffset;

    void Start()
    {
        // Save the starting position and rotation
        startPosition = transform.position;
        startRotation = transform.rotation;

        // Add a random starting phase so objects don't bob at the exact same time
        if (randomizeOffset)
        {
            randomOffset = Random.Range(0f, Mathf.PI * 2);
        }
    }

    void Update()
    {
        float time = Time.time * floatSpeed + randomOffset;

        // 1. Calculate the new Y position (Bobbing up and down)
        float newY = startPosition.y + Mathf.Sin(time) * floatHeight;

        // 2. Calculate the new rotation (Gentle rocking)
        float rockZ = Mathf.Sin(time * rockSpeed) * rockAngle;
        float rockX = Mathf.Cos(time * rockSpeed * 0.5f) * (rockAngle * 0.5f); // Slight X-axis tilt for 3D feel

        // 3. Apply the changes
        transform.position = new Vector3(startPosition.x, newY, startPosition.z);
        transform.rotation = startRotation * Quaternion.Euler(rockX, 0, rockZ);
    }
}