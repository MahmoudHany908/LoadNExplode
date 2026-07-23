using UnityEngine;

public class BillboardYAxis : MonoBehaviour
{
    void LateUpdate()
    {
        Vector3 direction = Camera.main.transform.position - transform.position;
        direction.y = 0; // Flatten to horizontal plane
        direction.Normalize();

        if (direction != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(-direction);
    }
}