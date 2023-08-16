using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using AC;
using System;


public class EDSRemoveAllComponents : EditorWindow
{
    //This is an editor script that is used to remove all components of a gameobject.
    //To use: Add this script to a gameobject
    //To clear multiple objects from components, mark multiple objects
    //and add the script to them

    Texture2D logo;
    Texture2D _logo;

    GUISkin esSkin;

    Vector2 minRect = new Vector2(50, 590);

    private bool checkAgain = false;

    public GameObject gameObject;

    public List<Component> components;

    int index = 0;
    
    [MenuItem("Tools/e-deaStudio/Remove All Components from GameObject", false, 1)]

    public static void ShowWindow()
    {
        GetWindow<EDSRemoveAllComponents>();
    }

    private void OnEnable()
    {
        logo = Resources.Load("edea_logo") as Texture2D;
        _logo = EDSUtility.ScaleTexture(logo, 50, 50);
        CheckConditions();
    }


    void CheckConditions()
    {
        if (Selection.activeObject)
        {
            gameObject = Selection.activeGameObject;
        }
        if (gameObject)
        {
            checkAgain = false;
        }
        if (!gameObject)
        {
            checkAgain = true;
        }
    }



    void GetCurrentComponents()
    {
        gameObject.GetComponents<Component>(components);
    }

   

    private void OnGUI()
    {
        if (!esSkin)
        {
            esSkin = Resources.Load("EDS_GUISkin") as GUISkin;
        }

        GUI.skin = esSkin;
        //Texture2D preview;

        this.minSize = minRect;
        this.titleContent = new GUIContent("Remove All Components from GameObject", null, "Remove all components");

        GUILayout.BeginVertical("Remove all Components", "window");

        GUIContent content = new();
        content.image = _logo;
        content.text = "  Components Cleaner";
        content.tooltip = "EDS Components Cleaner.";
        GUILayout.Label(content);

        EditorGUILayout.Separator();

        if (checkAgain) CheckConditions();
        
        GUILayout.BeginVertical("box");

        if (!gameObject)
        {
            EditorGUILayout.HelpBox("Select a GameObject!", MessageType.Info);
            checkAgain = true;
        }

        

        gameObject = EditorGUILayout.ObjectField("GameObject ", gameObject, typeof(GameObject), true, GUILayout.ExpandWidth(true)) as GameObject;

        if (gameObject.scene.name == null)
        {
            EditorGUILayout.HelpBox("GameObject cannot be a Prefab!", MessageType.Info);
        }

        GUILayout.EndVertical();

        GUILayout.BeginVertical("List of Components", "box");
        if (gameObject && gameObject.scene.name != null)
        {
            GetCurrentComponents();
            if (components.Count > 0)
            {
                List<string> componentsList = new();
                foreach (Component item in components)
                {
                    string componentName = item.GetType().ToString();
                    //Debug.Log(componentName);
                    if (componentName != "UnityEngine.Transform" && !componentsList.Contains(componentName))
                    {
                        componentsList.Add(componentName);
                    }
                }

                string[] options = componentsList.ToArray();
                index = EditorGUILayout.Popup(index, options);
                if (GUILayout.Button("Clean"))
                {
                    Clean(index, options[index]);
                }
                    //Debug.Log(componentsList.Count());
            }
        }
        GUILayout.EndVertical();
        GUILayout.EndVertical();

    }

    private void Clean(int index, string componentName)
    {

        if (EditorUtility.DisplayDialog("Remove Components?",
                $"Are you sure you want to remove all components of type {componentName}?", "Remove", "Abort"))
        {
            foreach (var item in gameObject.GetComponents<Component>())
            {
                if (item.GetType().ToString() == componentName)
                {
                    DestroyImmediate(item);
                }
            }
            Debug.Log("Clean");
        }
    }

}
