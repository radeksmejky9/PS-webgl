using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pointer : MonoBehaviour
{
    private Camera cam;
    void Start()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            CastRay();
        }
    }
    private void CastRay()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {

            GameObject hitObject = hit.collider.gameObject;

            if (hitObject.TryGetComponent<ModelElement>(out var modelElement))
            {
                modelElement.OnClick();
            }
        }
    }
}
