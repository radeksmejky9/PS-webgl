using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class Pointer : MonoBehaviour
{
    public static event Action<ModelElement> OnModelElementUpdated;

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
                if (EventSystem.current.IsPointerOverGameObject()) return;

                modelElement.OnClick(currentCategory);
                OnModelElementUpdated?.Invoke(modelElement);

            }
        }
    }
    private void OnCategorySelected(Category category)
    {
        currentCategory = category;
    }
}
