using UnityEngine;

public class ModelElement : MonoBehaviour
{
    [SerializeField] protected Category category;
    [SerializeField] private ModelType modelType;
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

    public ModelType ModelType
    {
        get => modelType;
        set => modelType = value;
    }

    public virtual void OnClick(Category category)
    {
        Category = category;
    }

    protected virtual void Rename()
    {
        this.gameObject.name = $"{modelType.Type}:{category.name}:";
    }
}