using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using UnityEditor.PackageManager.UI;
using AmplifyShaderEditor;
using System.Linq;
using UnityEditor.Formats.Fbx.Exporter;
using System.Reflection;
using System;

public class EDSSkinnedMeshBakerEditor : EditorWindow
{


    [SerializeField]
    private SkinnedMeshRenderer skinnedMesh;
    [SerializeField]
    private GameObject defaultGO;
    private Mesh defaultMesh;

    [SerializeField]
    private string meshName;
    private Mesh bakedMesh;
    private MeshFilter meshFilter;
    private bool canBake = false;
    private bool toggle = false;
    private bool isLegal = false;
    [MenuItem("Tools/e-deaStudio/Mesh Baker", false, 2)]
    public static void ShowWindow()
    {
        GetWindow<EDSSkinnedMeshBakerEditor>();
    }

    private void OnGUI()
    {
            
        Texture2D logo = Resources.Load("edea_logo") as Texture2D;
        Texture2D _logo = EDSUtility.ScaleTexture(logo, 50, 50);
        GUISkin esSkin = Resources.Load("EDS_GUISkin") as GUISkin;
        GUI.skin = esSkin;

        #region HEADER
        GUILayout.BeginHorizontal();

        GUIContent content = new();
        content.image = _logo;
        content.text = "  EDS Mesh Baker";
        content.tooltip = "EDS Skinned and Mesh Baker Editor";
        GUILayout.Label(content);

        GUILayout.EndHorizontal();

        EditorGUILayout.Separator();
        toggle = EditorGUILayout.Toggle("Is Skinned Mesh", toggle);

        if (toggle)
        {

            skinnedMesh = (SkinnedMeshRenderer)EditorGUILayout.ObjectField("Skinned Mesh", skinnedMesh, typeof(SkinnedMeshRenderer), true);

            if (!skinnedMesh)
            {
                EditorGUILayout.Separator();
                EditorGUILayout.HelpBox("You must assign a skinned mesh renderer component", MessageType.Warning);
            }

            canBake = skinnedMesh;
        }
        else
        {
            defaultGO = (GameObject)EditorGUILayout.ObjectField("Game Object", defaultGO, typeof(GameObject), true);

            if (!defaultGO)
            {
                EditorGUILayout.Separator();
                EditorGUILayout.HelpBox("You must assign a GameObject", MessageType.Warning);
                
            }
            else
            {
                isLegal = defaultGO.TryGetComponent<MeshFilter>(out meshFilter);

                if (!isLegal)
                {
                    EditorGUILayout.Separator();
                    EditorGUILayout.HelpBox("You must assign a GameObject with the MeshFilter component.", MessageType.Error);
                }
                else
                {
                    defaultMesh = meshFilter.sharedMesh;
                    if (!defaultMesh)
                    {
                        EditorGUILayout.Separator();
                        EditorGUILayout.HelpBox("You must assign a mesh renderer component", MessageType.Warning);
                    }
                }          
            }
            canBake = defaultGO & isLegal; 
        }

        EditorGUILayout.Separator();

        meshName = EditorGUILayout.TextField("Mesh Name", meshName);

        if (meshName is null || meshName.Trim().Length == 0)
        {
            EditorGUILayout.Separator();
            EditorGUILayout.HelpBox("You must assign a name to the new mesh", MessageType.Info);
            canBake &= false;
        }

        
        #endregion

        EditorGUILayout.Separator();

        if (GUILayout.Button("Bake Mesh"))
        {
            if (canBake & toggle)
            {
                BakeSkinnedMesh();
            }
            else if (canBake & !toggle)
            {
                BakeMesh();
            }
        }

        EditorGUILayout.Separator();
        if (GUILayout.Button("Reset"))
        {
            toggle = false;
            defaultGO = null;
            skinnedMesh = null;
            meshName = "";
        }

    }

    private void BakeMesh()
    {
        bakedMesh = defaultMesh;
        
        Material material = defaultGO.GetComponent<Renderer>().sharedMaterial;
        //SaveMesh(bakedMesh, meshName, false, true);
       
        GameObject tempGameObject = new();
        tempGameObject.name = meshName;
        tempGameObject.AddComponent<MeshFilter>();
        tempGameObject.AddComponent<MeshRenderer>();
        tempGameObject.GetComponent<MeshFilter>().mesh = bakedMesh;
        tempGameObject.GetComponent<MeshRenderer>().material = material;

        ExportMesh(tempGameObject, meshName, false, true);
        Debug.Log($"Mesh {meshName} saved correctly.");
        DestroyImmediate(tempGameObject);
    }
    private void BakeSkinnedMesh()
    {
        bakedMesh = new Mesh();
        skinnedMesh.BakeMesh(bakedMesh);
        Material material = skinnedMesh.sharedMaterial;
        
        
        GameObject tempGameObject = new();
        tempGameObject.name = meshName;
        tempGameObject.AddComponent<MeshFilter>();
        tempGameObject.AddComponent<MeshRenderer>();
        tempGameObject.GetComponent<MeshFilter>().mesh = bakedMesh;
        tempGameObject.GetComponent<MeshRenderer>().material = material;

        ExportMesh(tempGameObject, meshName, false, true);
        Debug.Log($"Mesh {meshName} saved correctly.");
        DestroyImmediate(tempGameObject);
    }

    private void ExportMesh(GameObject go, string name, bool makeNewInstance, bool optimizeMesh)
    {
        
        string path = EditorUtility.SaveFilePanel("Save Separate Mesh Asset", "Assets/", name, "fbx");
        if (string.IsNullOrEmpty(path)) return;

        path = FileUtil.GetPhysicalPath(path);
        Debug.Log(path);
        //Mesh meshToSave = makeNewInstance ? UnityEngine.Object.Instantiate(mesh) as Mesh : mesh;

        //if (optimizeMesh)
        //    MeshUtility.Optimize(meshToSave);
        
        ExportBinaryFBX(path, go);
        AssetDatabase.Refresh();

    }

    private void SaveMesh(Mesh mesh, string name, bool makeNewInstance, bool optimizeMesh)
	{
		string path = EditorUtility.SaveFilePanel("Save Separate Mesh Asset", "Assets/", name, "asset");
		if (string.IsNullOrEmpty(path)) return;

		path = FileUtil.GetProjectRelativePath(path);

		Mesh meshToSave = makeNewInstance ? UnityEngine.Object.Instantiate(mesh) as Mesh : mesh;

		if (optimizeMesh)
			MeshUtility.Optimize(meshToSave);
		AssetDatabase.CreateAsset(meshToSave, path);
		AssetDatabase.SaveAssets();
        
	}
    private static void ExportBinaryFBX(string filePath, UnityEngine.Object singleObject)
    {
        // Find relevant internal types in Unity.Formats.Fbx.Editor assembly
        Type[] types = AppDomain.CurrentDomain.GetAssemblies().First(x => x.FullName == "Unity.Formats.Fbx.Editor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null").GetTypes();
        Type optionsInterfaceType = types.First(x => x.Name == "IExportOptions");
        Type optionsType = types.First(x => x.Name == "ExportOptionsSettingsSerializeBase");
        Type optionsPositionType = types.First(x => x.Name == "ExportModelSettingsSerialize");
        // Instantiate a settings object instance
        MethodInfo optionsProperty = typeof(ModelExporter).GetProperty("DefaultOptions", BindingFlags.Static | BindingFlags.NonPublic).GetGetMethod(true);

        object optionsInstance = optionsProperty.Invoke(null, null);
        FieldInfo exportPositionField = optionsPositionType.GetField("objectPosition", BindingFlags.Instance | BindingFlags.NonPublic);
        exportPositionField.SetValue(optionsInstance, 1);

        // Change the export setting from ASCII to binary
        FieldInfo exportFormatField = optionsType.GetField("exportFormat", BindingFlags.Instance | BindingFlags.NonPublic);
        exportFormatField.SetValue(optionsInstance, 1);

        // Invoke the ExportObject method with the settings param
        MethodInfo exportObjectMethod = typeof(ModelExporter).GetMethod("ExportObject", BindingFlags.Static | BindingFlags.NonPublic, Type.DefaultBinder, new Type[] { typeof(string), typeof(UnityEngine.Object), optionsInterfaceType }, null);
        exportObjectMethod.Invoke(null, new object[] { filePath, singleObject, optionsInstance });
    }
}
