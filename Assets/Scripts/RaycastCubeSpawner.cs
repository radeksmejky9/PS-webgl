using UnityEngine;
using UnityEngine.UI;

public class RaycastCubeSpawner : MonoBehaviour
{
    public GameObject cubePrefab;
    public Material cubeMaterial;
    public Material cubeMaterialGhost;
    public float smoothSpeed = 15f;

    private GameObject spawnedCube;
    private MeshRenderer cubeRenderer;
    private Vector3 targetPosition;
    private bool isGhost = false;
    private bool isGhostEnabled = true;

    private void Start()
    {
        spawnedCube = Instantiate(
               cubePrefab,
               targetPosition,
               Quaternion.identity
           );
        cubeRenderer = spawnedCube.GetComponent<MeshRenderer>();
    }

    void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        bool hitDetected = Physics.Raycast(ray, out RaycastHit hit, 1f);

        if (hitDetected)
        {
            targetPosition = hit.point;
            isGhost = false;
        }
        else
        {
            targetPosition = transform.position + transform.forward * 1f;
            isGhost = true;
        }

        if (hitDetected)
        {
            spawnedCube.SetActive(true);
            cubeRenderer.material = cubeMaterial;
        }
        else if (!hitDetected && !isGhostEnabled)
        {
            spawnedCube.SetActive(false);
        }
        else
        {
            spawnedCube.SetActive(true);
            cubeRenderer.material = isGhost ? cubeMaterialGhost : cubeMaterial;
        }

        if (Input.GetAxis("Vertical") > 0 && isGhost)
        {
            spawnedCube.transform.position = transform.position + transform.forward * 1f;
        }
        else
        {
            spawnedCube.transform.position = Vector3.Lerp(
                spawnedCube.transform.position,
                targetPosition,
                Time.deltaTime * smoothSpeed
            );
        }

        Quaternion targetRotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);
        spawnedCube.transform.rotation = Quaternion.Lerp(
            spawnedCube.transform.rotation,
            targetRotation,
            Time.deltaTime * smoothSpeed
        );
    }

    public void ToggleGhost()
    {
        isGhostEnabled = !isGhostEnabled;
    }
}
