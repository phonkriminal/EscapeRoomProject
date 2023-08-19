using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

[SelectionBase]
[CanEditMultipleObjects]
[CustomEditor(typeof(EDSRotateObject))]
[ExecuteInEditMode]
public class EDSRotateEditor : Editor
{
    SerializedProperty xRotateSpeed;
    SerializedProperty yRotateSpeed;
    SerializedProperty zRotateSpeed;

    SerializedProperty space;

    SerializedProperty isClamp;
    SerializedProperty clamAngle;

    SerializedProperty axis;

    [Range(1f, 180f)]
    SerializedProperty clampAngle;
    private void OnEnable()
    {
        xRotateSpeed = serializedObject.FindProperty("xRotateSpeed");
        yRotateSpeed = serializedObject.FindProperty("yRotateSpeed");
        zRotateSpeed = serializedObject.FindProperty("zRotateSpeed");
        space = serializedObject.FindProperty("space");
        isClamp = serializedObject.FindProperty("isClamp");
        clampAngle = serializedObject.FindProperty("clampAngle");
        axis = serializedObject.FindProperty("axis");
    }
    public override void OnInspectorGUI()
    {
        EDSRotateObject edsRotate = (EDSRotateObject)target;
        Texture2D logo = Resources.Load("edea_logo") as Texture2D;
        Texture2D _logo = EDSUtility.ScaleTexture(logo, 50, 50);
        GUISkin esSkin = Resources.Load("EDS_GUISkin") as GUISkin;
        GUI.skin = esSkin;

        #region HEADER
        GUILayout.BeginHorizontal();

        GUIContent content = new();
        content.image = _logo;
        content.text = "  ROTATE OBJECT";
        content.tooltip = "EDS Rotate Object Controller Script";
        GUILayout.Label(content);

        GUILayout.EndHorizontal();

        serializedObject.Update();

        EditorGUILayout.HelpBox("This Script provide to Rotate the associated Gameobject.", MessageType.Info);
        EditorGUILayout.PropertyField(xRotateSpeed);
        EditorGUILayout.PropertyField(yRotateSpeed);
        EditorGUILayout.PropertyField(zRotateSpeed);
        EditorGUILayout.PropertyField(space);
        EditorGUILayout.PropertyField(isClamp);
        EditorGUILayout.PropertyField(clampAngle);
        EditorGUILayout.PropertyField(axis);

        serializedObject.ApplyModifiedProperties();
        #endregion
    }
 
}


