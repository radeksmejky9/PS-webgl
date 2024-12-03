using UnityEditor;
using UnityEditor.UI;
using UnityEngine;

[CustomEditor(typeof(ToggleButton))]
public class ToggleButtonEditor : ToggleEditor
{
    private SerializedProperty Label;
    private SerializedProperty StripeImage;

    protected override void OnEnable()
    {
        base.OnEnable();
        Label = serializedObject.FindProperty("Label");
        StripeImage = serializedObject.FindProperty("Image");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        serializedObject.Update();
        EditorGUILayout.PropertyField(Label, new GUIContent("Label"));
        EditorGUILayout.PropertyField(StripeImage, new GUIContent("Stripe Image"));
        serializedObject.ApplyModifiedProperties();
    }
}