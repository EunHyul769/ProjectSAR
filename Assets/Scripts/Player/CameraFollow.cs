using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;   // 플레이어
    public float smoothSpeed = 5f;
    public Vector3 offset;     // 카메라 위치 보정(약간 위에서 보게 하고 싶을 때)

    private void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothed = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

        transform.position = new Vector3(smoothed.x, smoothed.y, transform.position.z); 
    }
}