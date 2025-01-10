using AsImpL;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform Model;

    [Header("Orbit Settings")]
    [Tooltip("Maximum orbit speed when farthest from objects.")]
    public float maxOrbitSpeed = 5f;
    [Tooltip("Minimum orbit speed when closest to objects.")]
    public float minOrbitSpeed = 1f;

    [Header("Pan Settings")]
    [Tooltip("Speed of camera panning.")]
    public float panSpeed = 0.5f;

    [Header("Zoom Settings")]
    [Tooltip("Speed of camera zoom.")]
    public float zoomSpeed = 10f;
    [Tooltip("Smoothness of the zoom transition.")]
    public float zoomSmoothness = 0.1f;
    [Tooltip("Minimum distance from the target for zoom.")]
    public float minZoomDistance = 1f;
    [Tooltip("Maximum distance from the target for zoom.")]
    public float maxZoomDistance = 50f;

    [Header("WASD Movement Settings")]
    [Tooltip("Base movement speed for WASD controls.")]
    public float moveSpeed = 10f;
    [Tooltip("Multiplier for movement speed when holding Shift.")]
    public float boostMultiplier = 2f;

    private float distance; // Current distance from the camera to a virtual pivot point.
    private float targetDistance; // Desired distance (for smooth zooming).
    private Vector3 lastMousePosition;

    [SerializeField]
    private float currentOrbitSpeed;

    private bool isActive = false; // Flag to activate controls.

    void Start()
    {
        distance = 10f;
        targetDistance = distance;
        lastMousePosition = Input.mousePosition;
        currentOrbitSpeed = maxOrbitSpeed;
    }

    private void OnEnable()
    {
        MultiObjectImporter.ImportedModel += OnModelImported;
    }

    private void OnDisable()
    {
        MultiObjectImporter.ImportedModel -= OnModelImported;
    }

    private void OnModelImported(GameObject model, string path)
    {
        isActive = true;
    }

    void Update()
    {
        if (!isActive)
            return;

        HandleInput();
        SmoothZoom();
    }

    void HandleInput()
    {
        Vector3 mouseDelta = Input.mousePosition - lastMousePosition;

        // Rotate the camera.
        if (Input.GetMouseButton(1)) // Right mouse button.
        {
            float yaw = mouseDelta.x * maxOrbitSpeed * Time.deltaTime;
            float pitch = -mouseDelta.y * maxOrbitSpeed * Time.deltaTime;
            transform.Rotate(Vector3.up, yaw, Space.World);
            transform.Rotate(Vector3.right, pitch, Space.Self);
        }

        // Pan the camera.
        if (Input.GetMouseButton(2) || (Input.GetMouseButton(1) && Input.GetKey(KeyCode.LeftShift)))
        {
            Vector3 pan = transform.right * -mouseDelta.x * panSpeed * Time.deltaTime
                        + transform.up * -mouseDelta.y * panSpeed * Time.deltaTime;
            transform.position += pan;
        }

        // Zoom the camera.
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0f)
        {
            targetDistance -= scroll * zoomSpeed;
            targetDistance = Mathf.Clamp(targetDistance, minZoomDistance, maxZoomDistance);
        }

        // WASD movement.
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        float moveMultiplier = Input.GetKey(KeyCode.LeftShift) ? boostMultiplier : 1f;

        Vector3 forwardMovement = transform.forward * vertical;
        Vector3 rightMovement = transform.right * horizontal;

        transform.position += moveMultiplier * moveSpeed * Time.deltaTime * (forwardMovement + rightMovement);

        lastMousePosition = Input.mousePosition;
    }

    void SmoothZoom()
    {
        distance = Mathf.Lerp(distance, targetDistance, zoomSmoothness);
        transform.position += transform.forward * (targetDistance - distance);
    }
}
