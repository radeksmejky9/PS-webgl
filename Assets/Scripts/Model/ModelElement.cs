using Unity.VisualScripting;
using UnityEngine;

public class ModelElement : MonoBehaviour
{
    [SerializeField] private Category category;
    private MeshRenderer meshRenderer;

    public Category Category
    {
        get => category;
        set
        {
            category = value;
            if (meshRenderer != null)
            {
                meshRenderer.material = category.Material;
                return;
            }

            if (TryGetComponent(out MeshRenderer renderer))
            {
                meshRenderer = renderer;
                meshRenderer.material = category.Material;
            }
        }
    }
}