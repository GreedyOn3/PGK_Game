using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCamera : MonoBehaviour
{
    public Transform playerCamera;
    [Header("Target")]
    public Transform target;
    public Vector3 targetOffset;

    [Header("Settings")]
    public float distance = 5f;
    public float sensitivity = 0.5f;
    public Vector2 pitchMinMax = new Vector2(-15f, 80f);

    [Header("Collision")]
    public LayerMask ignoreMask;
    public float cameraRadius = 0.4f;

    float yaw;
    float pitch;
    Vector2 lookInput;
    float currDist;

    void Start()
    {
        currDist = distance;
    }

    private void Update()
    {
        currDist = CalculateDistance(target.position + targetOffset);
    }

    void LateUpdate()
    {
        if (target == null) return;

        yaw += lookInput.x * sensitivity;
        pitch -= lookInput.y * sensitivity;
        pitch = Mathf.Clamp(pitch, pitchMinMax.x, pitchMinMax.y);
        playerCamera.eulerAngles = new Vector3(pitch, yaw);

        Vector3 targetPos = target.position + targetOffset;
        playerCamera.position = targetPos - playerCamera.forward * currDist;
    }

    float CalculateDistance(Vector3 targetPos)
    {
        Vector3 castDir = playerCamera.position - targetPos;
        float dist = distance;

        RaycastHit hit;
        if (Physics.SphereCast(targetPos, cameraRadius, castDir, out hit, distance, ~ignoreMask))
            dist = Mathf.Max(0.1f, hit.distance - cameraRadius);

        return dist;
    }

    void OnLook(InputValue val)
    {
        lookInput = val.Get<Vector2>();
    }
}
