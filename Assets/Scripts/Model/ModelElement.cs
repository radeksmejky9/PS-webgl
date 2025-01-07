using Unity.VisualScripting;
using UnityEngine;

public class ModelElement : MonoBehaviour
{
    [SerializeField] protected Category category;
    private MeshRenderer meshRenderer;

    public Category Category
    {
        get => category;
        set
        {
            category = value;
            Rename();
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

    public virtual void OnClick(Category category)
    {
        Category = category;
    }

    protected virtual void Rename()
    {
        this.gameObject.name = $"{GetType().Name}:{category.Material.name}:";
    }
}