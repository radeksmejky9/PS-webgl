using System.Collections.Generic;
using UnityEditor;

[CustomEditor(typeof(ModelTypes))]
public class ModelTypesEditor : Editor
{
    public override void OnInspectorGUI()
    {
        ModelTypes modelTypes = (ModelTypes)target;

        DrawDefaultInspector();

        HashSet<string> uniqueTypes = new HashSet<string>();
        foreach (var modelType in modelTypes.Types)
        {
            if (modelType == null) continue;

            if (!uniqueTypes.Add(modelType.Type))
            {
                EditorGUILayout.HelpBox($"Duplicate ModelType: {modelType.Type}", MessageType.Error);
            }
        }

        EditorUtility.SetDirty(modelTypes);
    }
}
