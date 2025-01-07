using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pointer : MonoBehaviour
{
    private Camera cam;
    private Category currentCategory = null;
    void Start()
    {
        cam = Camera.main;
        ToggleButtonManager.OnCategorySelected += OnCategorySelected;
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
        if (currentCategory == null)
            return;

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {

            GameObject hitObject = hit.collider.gameObject;

            if (hitObject.TryGetComponent<ModelElement>(out var modelElement))
            {
                modelElement.OnClick(currentCategory);
            }
        }
    }
    private void OnCategorySelected(Category category)
    {
        currentCategory = category;
    }
}
