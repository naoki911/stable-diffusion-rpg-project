using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public float sensitivity = 100.0f;
    public Vector3 offset = new Vector3(0, 5, -10);
    private Vector2 cameraInput;
    private float xRotation = 0.0f;
    private float yRotation = 0.0f;
    public float pitchMin = -60f;
    public float pitchMax = 80f;
    public float targetHeightOffset = 2.0f;
    public LayerMask wallLayer;

    void Start()
    {
        transform.position = target.position + offset;
        transform.LookAt(target.position + Vector3.up * targetHeightOffset);
    }

    void Update()
    {
        float mouseX = cameraInput.x * sensitivity * Time.deltaTime;
        float mouseY = cameraInput.y * sensitivity * Time.deltaTime;

        yRotation += mouseX;
        xRotation -= mouseY;
        
        xRotation = Mathf.Clamp(xRotation, pitchMin, pitchMax);

        Vector3 direction = new Vector3(0, 0, -offset.magnitude);
        Quaternion rotation = Quaternion.Euler(xRotation, yRotation, 0);
        Vector3 desiredPosition = target.position + rotation * direction;

        RaycastHit hit;
        if (Physics.Linecast(target.position + Vector3.up * targetHeightOffset, desiredPosition, out hit, wallLayer))
        {
            desiredPosition = hit.point;
        }

        transform.position = desiredPosition;
        transform.LookAt(target.position + Vector3.up * targetHeightOffset);
    }

    public void OnCameraMove(InputAction.CallbackContext context)
    {
        cameraInput = context.ReadValue<Vector2>();
    }
}