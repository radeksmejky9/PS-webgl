using UnityEditor;

[CustomEditor(typeof(Category))]
public class CategoryEditor : Editor
{
    private Category category;

    public void OnEnable()
    {
        category = (Category)target;
    }
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        if (category.Material == null)
        {
            EditorGUILayout.HelpBox("Material cannot be set to null!", MessageType.Error);
        }
    }
}