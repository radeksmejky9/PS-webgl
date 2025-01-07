using UnityEditor;
using UnityEditor.UI;
using UnityEngine;

[CustomEditor(typeof(CategoryButton))]
public class CategoryButtonEditor : ToggleEditor
{
    private SerializedProperty ToggleButton;
    private SerializedProperty SelectionPanel;

    protected override void OnEnable()
    {
        base.OnEnable();
        ToggleButton = serializedObject.FindProperty("ToggleButton");
        SelectionPanel = serializedObject.FindProperty("SelectionPanel");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        serializedObject.Update();
        EditorGUILayout.PropertyField(ToggleButton, new GUIContent("Toggle Button"));
        EditorGUILayout.PropertyField(SelectionPanel, new GUIContent("SelectionPanel"));
        serializedObject.ApplyModifiedProperties();
    }
}