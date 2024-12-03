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
    [Tooltip("Maximum distance for the raycast to adjust orbit speed.")]
    public float raycastMaxDistance = 100f;
    [Tooltip("Number of rays cast in a cone to determine average distance.")]
    public int rayCount = 10;
    [Tooltip("Angle of the cone for multiple raycasts.")]
    public float coneAngle = 15f;

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

    private Vector3 target; // Target point the camera orbits around.
    private float distance; // Current distance from the target.
    private float targetDistance; // Desired distance (for smooth zooming).
    private Vector3 lastMousePosition;

    [SerializeField]
    private float currentOrbitSpeed;

    private bool isActive = false; // Flag to activate controls.

    void Start()
    {
        // Initialize target based on the camera's initial position.
        target = transform.position + transform.forward * 10f;
        distance = Vector3.Distance(transform.position, target);
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


        AdjustOrbitSpeed();
        HandleInput();
        SmoothZoom();
    }

    void AdjustOrbitSpeed()
    {
        float totalHitDistance = 0f;
        int hitCount = 0;

        // Cast multiple rays in a cone-like pattern.
        for (int i = 0; i < rayCount; i++)
        {
            // Generate a random direction within the cone.
            Vector3 randomDirection = Random.insideUnitSphere;
            randomDirection.y = Mathf.Clamp(randomDirection.y, -0.5f, 0.5f); // Restrict vertical spread.
            randomDirection = Vector3.Lerp(transform.forward, randomDirection, coneAngle / 90f).normalized;

            // Perform a raycast.
            if (Physics.Raycast(transform.position, randomDirection, out RaycastHit hit, raycastMaxDistance))
            {
                totalHitDistance += hit.distance;
                hitCount++;
                Debug.DrawLine(transform.position, hit.point, Color.red);
            }
        }

        // Calculate average hit distance if there are valid hits.
        if (hitCount > 0)
        {
            float averageDistance = totalHitDistance / hitCount;
            currentOrbitSpeed = Mathf.Lerp(minOrbitSpeed, maxOrbitSpeed, averageDistance / raycastMaxDistance);
        }
        else
        {
            // Default to max orbit speed if no valid hits.
            currentOrbitSpeed = maxOrbitSpeed;
        }
    }

    void HandleInput()
    {
        Vector3 mouseDelta = Input.mousePosition - lastMousePosition;

        // Orbit around the target.
        if (Input.GetMouseButton(1)) // Right mouse button.
        {
            float yaw = mouseDelta.x * currentOrbitSpeed * Time.deltaTime;
            float pitch = -mouseDelta.y * currentOrbitSpeed * Time.deltaTime;
            transform.RotateAround(target, Vector3.up, yaw);
            transform.RotateAround(target, transform.right, pitch);
        }

        // Pan the camera.
        if (Input.GetMouseButton(2) || (Input.GetMouseButton(1) && Input.GetKey(KeyCode.LeftShift)))
        {
            Vector3 pan = transform.right * -mouseDelta.x * panSpeed * Time.deltaTime
                        + transform.up * -mouseDelta.y * panSpeed * Time.deltaTime;
            target += pan;
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
        float horizontal = Input.GetAxis("Horizontal"); // A/D or Left/Right Arrow.
        float vertical = Input.GetAxis("Vertical");     // W/S or Up/Down Arrow.
        float moveMultiplier = Input.GetKey(KeyCode.LeftShift) ? boostMultiplier : 1f;

        Vector3 forwardMovement = transform.forward * vertical;
        Vector3 rightMovement = transform.right * horizontal;

        transform.position += moveMultiplier * moveSpeed * Time.deltaTime * (forwardMovement + rightMovement);

        // Update target to match position if WASD is used.
        if (horizontal != 0 || vertical != 0)
        {
            target = transform.position + transform.forward * targetDistance;
        }

        lastMousePosition = Input.mousePosition;
    }

    void SmoothZoom()
    {
        distance = Mathf.Lerp(distance, targetDistance, zoomSmoothness);
        Vector3 direction = (transform.position - target).normalized;
        transform.position = target + direction * distance;
    }
}
