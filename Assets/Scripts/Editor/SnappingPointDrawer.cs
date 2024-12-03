using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(SnappingPointAttribute), true)]
[CustomPropertyDrawer(typeof(SnappingPoint))]
public class SnappingPointDrawer : PropertyDrawer
{
    bool positionEditable = true;
    bool rotationEditable = true;

    SerializedProperty building;
    SerializedProperty room;
    SerializedProperty position;
    SerializedProperty rotation;
    SerializedProperty url;
    public void OnEnable()
    {
        SnappingPointAttribute snappingPointAttribute = (SnappingPointAttribute)attribute;
        if (snappingPointAttribute != null)
        {
            positionEditable = snappingPointAttribute.positionEditable;
            rotationEditable = snappingPointAttribute.rotationEditable;
        }
    }
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        building = property.FindPropertyRelative("Building");
        room = property.FindPropertyRelative("Room");
        this.position = property.FindPropertyRelative("Position");
        rotation = property.FindPropertyRelative("Rotation");
        url = property.FindPropertyRelative("Url");

        Vector3 tempPosition = new Vector3(
             this.position.FindPropertyRelative("x").floatValue,
             this.position.FindPropertyRelative("y").floatValue,
             this.position.FindPropertyRelative("z").floatValue
        );

        EditorGUILayout.LabelField("Snapping Point", EditorStyles.boldLabel);

        building.stringValue = EditorGUILayout.TextField("Building", building.stringValue);
        room.stringValue = EditorGUILayout.TextField("Room", room.stringValue);

        GUI.enabled = positionEditable;
        tempPosition = EditorGUILayout.Vector3Field("Position", tempPosition);
        GUI.enabled = true;

        GUI.enabled = rotationEditable;
        rotation.floatValue = EditorGUILayout.FloatField("Rotation", rotation.floatValue);
        GUI.enabled = true;

        url.stringValue = EditorGUILayout.TextField("URL", url.stringValue);

        this.position.FindPropertyRelative("x").floatValue = tempPosition.x;
        this.position.FindPropertyRelative("y").floatValue = tempPosition.y;
        this.position.FindPropertyRelative("z").floatValue = tempPosition.z;
        EditorGUI.EndProperty();
    }
}
