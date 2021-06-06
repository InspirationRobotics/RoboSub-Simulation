using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(Placeable)), CanEditMultipleObjects]
public class PlaceableEditor : Editor
{
    SerializedProperty debugModeProperty;
    SerializedProperty probingVectorProperty;

    void OnEnable()
    {
        debugModeProperty = serializedObject.FindProperty("debugMode");
        probingVectorProperty = serializedObject.FindProperty("probingVector");
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button("Auto Adjust")) {
            foreach (Object target in targets)
            {
                Undo.RegisterCompleteObjectUndo(target, "Auto adjustment of " + target.name);
                ((Placeable)target).AutoAdjust();
            }
        }
        
        if (debugModeProperty.boolValue)
        {
            EditorGUILayout.HelpBox("There is a white line showing the probing vector and yellow line showing what is the allowed distance in this direction.", MessageType.Info);
            EditorGUILayout.PropertyField(probingVectorProperty, new GUIContent("Probing Vector"));
        }
        
        serializedObject.ApplyModifiedProperties();
    }
}