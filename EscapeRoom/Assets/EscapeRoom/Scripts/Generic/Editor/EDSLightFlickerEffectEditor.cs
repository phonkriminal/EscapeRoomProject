using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

[SelectionBase]
[CanEditMultipleObjects]
[CustomEditor(typeof(EDSLightFlickerEffect))]

public class EDSLightFlickerEffectEditor : Editor
{
    SerializedProperty light;
    SerializedProperty minIntensity;
    SerializedProperty maxIntensity;
    SerializedProperty smoothing;
    private void OnEnable()
    {
        light = serializedObject.FindProperty("_light");
        minIntensity = serializedObject.FindProperty("minIntensity");
        maxIntensity = serializedObject.FindProperty("maxIntensity");
        smoothing = serializedObject.FindProperty("smoothing");
    }
    public override void OnInspectorGUI()
    {
        EDSLightFlickerEffect lightFlicker = (EDSLightFlickerEffect)target;
        Texture2D logo = Resources.Load("edea_logo") as Texture2D;
        Texture2D _logo = EDSUtility.ScaleTexture(logo, 50, 50);
        GUISkin esSkin = Resources.Load("EDS_GUISkin") as GUISkin;
        GUI.skin = esSkin;

        #region HEADER
        GUILayout.BeginHorizontal();

        GUIContent content = new();
        content.image = _logo;
        content.text = "  LIGHT FLICKER FX";
        content.tooltip = "EDS Light Flicker Effect";
        GUILayout.Label(content);

        GUILayout.EndHorizontal();

        serializedObject.Update();
        EditorGUILayout.HelpBox("This Script provide to apply a Flicker FX to the associated light.", MessageType.Info);
        EditorGUILayout.PropertyField(light);
        
        
        if (!lightFlicker._light) 
        {
            if (!lightFlicker.GetLightComponent())
            {
                EditorGUILayout.HelpBox("This Script needs a light component.", MessageType.Warning);
            }
        }
        else
        {
            EditorGUILayout.PropertyField(minIntensity);
            EditorGUILayout.PropertyField(maxIntensity);
            EditorGUILayout.PropertyField(smoothing);            
        }
        serializedObject.ApplyModifiedProperties();



        #endregion
    }

}
