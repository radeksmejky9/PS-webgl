using UnityEngine;

[CreateAssetMenu(fileName = "NewCategoryGroup", menuName = "Category/Category Group")]
public class CategoryGroup : ScriptableObject
{
    public override string ToString()
    {
        return this.name;
    }
}