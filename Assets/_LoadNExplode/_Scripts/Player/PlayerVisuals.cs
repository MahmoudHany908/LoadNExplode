using UnityEngine;

public class PlayerVisuals : MonoBehaviour
{
    [SerializeField] private Transform playerVisualTransform;

    private Player player;

    public Transform PlayerVisualTransform => playerVisualTransform;
    void Start()
    {
        player = GetComponent<Player>();
    }




    //private void RotatePlayerToMouse()
    //{
    //    Vector3 targetPos = PlayerInputs.Instance.GetMouseWorldPosition();
    //    Vector3 direction = targetPos - transform.position;
    //    direction.y = 0f;

    //    if (direction.sqrMagnitude > 0.01f)
    //    {
    //        Quaternion targetRotation = Quaternion.LookRotation(direction);
    //        playerVisualTransform.rotation = Quaternion.Slerp(playerVisualTransform.rotation, targetRotation, Time.fixedDeltaTime * 10f);
    //    }
    //}
}






// private void RotatePlayerToMouse()
// {
//     Vector3 targetPos = playerInputs.GetMouseWorldPosition();
//     Vector3 direction = targetPos - transform.position;
//     direction.y = 0f;

//     if (direction.sqrMagnitude > 0.01f)
//     {
//         Quaternion targetRotation = Quaternion.LookRotation(direction);
//         player.rotation = Quaternion.Slerp(player.rotation, targetRotation, Time.fixedDeltaTime * 10f);
//     }
// }