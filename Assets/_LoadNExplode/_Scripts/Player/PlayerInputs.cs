using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputs : MonoBehaviour
{
    private Camera cam;
    public static PlayerInputs Instance { get; private set; }

    private InputActions input;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;


        input = new InputActions();
    }

    void OnEnable()
    {
        input.Enable();
    }

    void OnDisable()
    {
        input.Disable();
    }

    void Start()
    {
        cam = Camera.main;
    }

    internal Vector3 MoveDirection()
    {

        Vector2 moveInput = input.Player.Move.ReadValue<Vector2>();

        Vector3 inputDir = new Vector3(moveInput.x, 0f, moveInput.y).normalized;

        Vector3 camForward = cam.transform.forward;
        Vector3 camRight = cam.transform.right;
        camForward.y = 0f;
        camRight.y = 0f;
        camForward.Normalize();
        camRight.Normalize();

        Vector3 moveDir = camForward * inputDir.z + camRight * inputDir.x;

        return moveDir.normalized;
    }
    // this for player sprite flipping 
    internal float HorizontalMoveDirection()
    {
        return input.Player.Move.ReadValue<Vector2>().x;
    }

    public Vector3 GetMouseWorldPosition()
    {
        if (cam == null)
        {
            Debug.LogError("Main Camera not found. Please ensure there is a camera tagged as 'MainCamera' in the scene.");
            return Vector3.zero;
        }


        Vector2 mouseScreenPos = Mouse.current != null ? Mouse.current.position.ReadValue() : Vector2.zero;

        // Option B: If you specifically created a "MousePosition" action in your input asset, use this instead:
        // Vector2 mouseScreenPos = input.Player.MousePosition.ReadValue<Vector2>();

        Ray ray = cam.ScreenPointToRay(mouseScreenPos);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);

        if (groundPlane.Raycast(ray, out float rayDistance))
        {
            return ray.GetPoint(rayDistance);
        }

        return Vector3.zero;
    }
}