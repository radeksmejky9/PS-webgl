using UnityEngine;

[CreateAssetMenu(fileName = "NewCategory", menuName = "Category/Category")]
public class Category : ScriptableObject
{
    [Tooltip("The group this category is part of.")]
    public CategoryGroup CategoryGroup;
    [Tooltip("Material representing this category.")]
    public Material Material;

    public override string ToString()
    {
        return this.name;
    }
}