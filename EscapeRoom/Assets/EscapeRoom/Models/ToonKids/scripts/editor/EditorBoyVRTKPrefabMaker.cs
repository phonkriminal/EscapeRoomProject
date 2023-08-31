using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(BoyVRTKPrefabMaker))]

public class EditorBoyVRTKPrefabMaker : Editor
{
    public override void OnInspectorGUI()
    {
        BoyVRTKPrefabMaker myPrefabMaker = (BoyVRTKPrefabMaker)target;
        if (!myPrefabMaker.allOptions)
        {
            if (GUILayout.Button("LET'S GET DRESS", GUILayout.Width(250), GUILayout.Height(75)))
            {
                myPrefabMaker.Menu();
                myPrefabMaker.Getready();
            }
        }
        else
        {
            if (GUILayout.Button("RANDOMIZE", GUILayout.Width(250), GUILayout.Height(75)))
            {
                myPrefabMaker.Randomize();
            }

            EditorGUILayout.Space();

            GUILayout.BeginHorizontal("box");
            if (GUILayout.Button("HAT", GUILayout.Width(115), GUILayout.Height(20))) { myPrefabMaker.HatOn(); }
            if (myPrefabMaker.hatactive)
            {
                if (GUILayout.Button("<", GUILayout.Width(20), GUILayout.Height(20))) { myPrefabMaker.Nexthatcolor(1); }
                EditorGUILayout.LabelField("  material", GUILayout.Width(65), GUILayout.Height(20));
                if (GUILayout.Button(">", GUILayout.Width(20), GUILayout.Height(20))) { myPrefabMaker.Nexthatcolor(0); }
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal("box");
            if (GUILayout.Button("<", GUILayout.Width(20), GUILayout.Height(20))) { myPrefabMaker.Prevhair(); }
            EditorGUILayout.LabelField("  HAIR", GUILayout.Width(65), GUILayout.Height(20));
            if (GUILayout.Button(">", GUILayout.Width(20), GUILayout.Height(20))) { myPrefabMaker.Nexthair(); }
            if (GUILayout.Button("<", GUILayout.Width(20), GUILayout.Height(20))) { myPrefabMaker.Nexthaircolor(1); }
            EditorGUILayout.LabelField("  material", GUILayout.Width(65), GUILayout.Height(20));
            if (GUILayout.Button(">", GUILayout.Width(20), GUILayout.Height(20))) { myPrefabMaker.Nexthaircolor(0); }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal("box");
            if (GUILayout.Button("<", GUILayout.Width(20), GUILayout.Height(20))) { myPrefabMaker.Nextskincolor(1); }
            EditorGUILayout.LabelField("  SKIN", GUILayout.Width(65), GUILayout.Height(20));
            if (GUILayout.Button(">", GUILayout.Width(20), GUILayout.Height(20))) { myPrefabMaker.Nextskincolor(0); }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal("box");
            if (GUILayout.Button("<", GUILayout.Width(20), GUILayout.Height(20))) { myPrefabMaker.Nexteyescolor(1); }
            EditorGUILayout.LabelField("  EYES", GUILayout.Width(65), GUILayout.Height(20));
            if (GUILayout.Button(">", GUILayout.Width(20), GUILayout.Height(20))) { myPrefabMaker.Nexteyescolor(0); }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal("box");
            if (GUILayout.Button("GLASSES", GUILayout.Width(115), GUILayout.Height(20))) { myPrefabMaker.Glasseson(); }
            if (myPrefabMaker.glassesactive)
            {
                if (GUILayout.Button("<", GUILayout.Width(20), GUILayout.Height(20))) { myPrefabMaker.Nextglasses(1); }
                EditorGUILayout.LabelField("  material", GUILayout.Width(65), GUILayout.Height(20));
                if (GUILayout.Button(">", GUILayout.Width(20), GUILayout.Height(20))) { myPrefabMaker.Nextglasses(0); }
            }
            GUILayout.EndHorizontal();            

            GUILayout.BeginHorizontal("box");
            if (GUILayout.Button("<", GUILayout.Width(20), GUILayout.Height(20))) { myPrefabMaker.Prevchest(); }
            EditorGUILayout.LabelField("   CHEST", GUILayout.Width(65), GUILayout.Height(20));
            if (GUILayout.Button(">", GUILayout.Width(20), GUILayout.Height(20))) { myPrefabMaker.Nextchest(); }
            if (GUILayout.Button("<", GUILayout.Width(20), GUILayout.Height(20))) { myPrefabMaker.Nextchestcolor(1); }
            EditorGUILayout.LabelField("  material", GUILayout.Width(65), GUILayout.Height(20));
            if (GUILayout.Button(">", GUILayout.Width(20), GUILayout.Height(20))) { myPrefabMaker.Nextchestcolor(0); }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal("box");
            if (GUILayout.Button("CREATE COPY", GUILayout.Width(100), GUILayout.Height(50)))
            {
                myPrefabMaker.CreateCopy();
            }
            if (GUILayout.Button("DONE", GUILayout.Width(100), GUILayout.Height(50)))
            {
                myPrefabMaker.FIX();
            }
            GUILayout.EndHorizontal();

            if (GUILayout.Button("RESET", GUILayout.Width(100), GUILayout.Height(50)))
            {
                myPrefabMaker.Resetmodel();
            }
        }
    }
}