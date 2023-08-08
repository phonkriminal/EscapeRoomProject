using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System;
public class EDSCreateIconFromPrefabEditor : EditorWindow
{
    Texture2D logo;
    Texture2D _logo;

    GUISkin esSkin;

    Vector2 minRect = new Vector2(500, 590);

    private bool checkAgain = false;

    public GameObject charObj;

    Editor humanoidpreview;

    public int sizeIndex = 0;
    
    public Sprite _backGround;
    public GameObject _cameraPrefab;
    public GameObject _itemCanvasPrefab;
    private Texture2D empty;
    [SerializeField]
    [Range(0,32)]
    private int offSet = 10;
    [SerializeField]
    private string iconName;

    [HideInInspector]
    public GameObject _itemCanvas;
    private bool canSave;
    [HideInInspector]
    public Camera _itemCamera;

    public string[] imageSize = new string[] {"64x64", "128x128", "256x256" , "512x512", "1024x1024"};

    private bool isCamera, isCanvas = false;

    private void OnEnable()
    {
        logo = Resources.Load("edea_logo") as Texture2D;
        _logo = EDSUtility.ScaleTexture(logo, 50, 50);
        _itemCanvasPrefab = Resources.Load("Prefabs/IconCanvas") as GameObject;
        _cameraPrefab = Resources.Load("Prefabs/ItemCamera") as GameObject;
        empty = Resources.Load("Empty") as Texture2D;
        CheckConditions();
    }

    [MenuItem("Tools/e-deaStudio/NPC Icon Generator", false, 1)]
    public static void ShowWindow()
    {
        GetWindow<EDSCreateIconFromPrefabEditor>();
    }
    void CheckConditions()
    {
        if (Selection.activeObject)
        {
            charObj = Selection.activeGameObject;
        }
        if (charObj)
        {
            checkAgain = false;
            humanoidpreview = Editor.CreateEditor(charObj);
        }
        isCamera = _cameraPrefab;
        isCanvas = _itemCanvasPrefab;
    }

   
    public Sprite GetIcon()
    {
        Renderer renderer = charObj.transform.GetComponentInChildren<Renderer>();
        //_itemCamera.orthographicSize = renderer.bounds.extents.y + 0.1f;

        // Get our dimensions
        int resX = _itemCamera.pixelWidth;
        int resY = _itemCamera.pixelHeight;

        // Variables for clipping image down to square

        int clipX = 0;
        int clipY = 0;

        if (resX > resY)
            clipX = resX - resY;
        else if (resY > resX)
            clipY = resY - resX;


        // Initialise all parts.
        Texture2D tex = new Texture2D(resX - clipX , resY - clipY, TextureFormat.RGBA32, false);
        RenderTexture renderTexture = new(resX, resY, 24);
        _itemCamera.targetTexture = renderTexture;
        RenderTexture.active = renderTexture;

        // Grab the icon and stick it in the texture.
        _itemCamera.Render();
        tex.ReadPixels(new Rect(clipX / 2, clipY / 2, resX, resY), 0, 0);
        tex.Apply();

        //Clean up.

        _itemCamera.targetTexture = null;
        RenderTexture.active = null;
        DestroyImmediate(renderTexture);

        // Convert Texture2D into Sprite and return it.

        return Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0, 0));

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
        this.titleContent = new GUIContent("Icon Prefab Creator", null, "Icon Prefab Creator");

        GUILayout.BeginVertical("Icon from Prefab Creator", "window");

        GUIContent content = new();
        content.image = _logo;
        content.text = "  ICON PREFAB EDITOR";
        content.tooltip = "EDS Icon Prefab editor.";
        GUILayout.Label(content);

        EditorGUILayout.Separator();

        if (checkAgain) CheckConditions();


        GUILayout.BeginVertical("box");

        if (!charObj)
        {
            EditorGUILayout.HelpBox("Select FBX model to generate Icon!", MessageType.Info);
            checkAgain = true;
            canSave = false;
        }else if (!isCamera)
        {
            EditorGUILayout.HelpBox("You have to assign the camera prefab. /n You find in Resources/Prefabs", MessageType.Info);
            checkAgain = true;
            canSave = false;
        }else if (!isCanvas)
        {
            EditorGUILayout.HelpBox("You have to assign the canvas prefab. /n You find in Resources/Prefabs", MessageType.Info);
            checkAgain = true;
            canSave = false;
        }

        charObj = EditorGUILayout.ObjectField("Your Model ", charObj, typeof(GameObject), true, GUILayout.ExpandWidth(true)) as GameObject;
        _cameraPrefab = EditorGUILayout.ObjectField("Camera Prefab", _cameraPrefab, typeof(GameObject), true, GUILayout.ExpandWidth(true)) as GameObject;
        _itemCanvasPrefab = EditorGUILayout.ObjectField("Canvas Prefab ", _itemCanvasPrefab, typeof(GameObject), true, GUILayout.ExpandWidth(true)) as GameObject;
        GUI.skin = null;
        _backGround = EditorGUILayout.ObjectField("Background Image", _backGround, typeof(Sprite), true, GUILayout.ExpandWidth(true)) as Sprite;
        GUI.skin = esSkin;
        offSet = EditorGUILayout.IntField("Icon Offset ", offSet, GUILayout.ExpandWidth(true));

        if (offSet < 0)
        {
            EditorGUILayout.HelpBox("The Icon OffSet value can't be negative.", MessageType.Warning);
            offSet = 0;
        }else if (offSet >= 32)
        {
            EditorGUILayout.HelpBox("The Icon OffSet value reaches the maximum value.", MessageType.Warning);
            offSet = 32; 
        }
        EditorGUILayout.Space();

        if (GUI.changed && charObj != null)
        {
            humanoidpreview = Editor.CreateEditor(charObj);
        }

        GUILayout.EndVertical();

        if (charObj != null)
        {
            GUILayout.BeginVertical("window");
            DrawHumanoidPreview();
            GUILayout.EndVertical();

            GUILayout.Space(10);

            sizeIndex = EditorGUILayout.Popup(sizeIndex, imageSize);


            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
          
            if (isCamera && isCanvas)
            {
                if (GUILayout.Button("Set Up"))
                {
                    ShowCameraRender();
                }
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.FlexibleSpace();
            iconName = EditorGUILayout.TextField("Icon Name", iconName);

            if (iconName is null || iconName.Trim().Length == 0)
            {
                EditorGUILayout.Separator();
                EditorGUILayout.HelpBox("You must assign a name to the icon file", MessageType.Info);               
            }
            if (canSave)
            {
                if (GUILayout.Button("Create Icon"))
                {
                    CreateIcon();
                }
            }
        }
        GUILayout.EndVertical();
    }


    public virtual void ShowCameraRender()
    {
        GameObject _cameraContainer = InstantiateNewObject(_cameraPrefab);
        _itemCamera =  _cameraContainer.GetComponent<Camera>();
        _itemCanvas = InstantiateNewObject(_itemCanvasPrefab);
        canSave = true;
    }

    public virtual void CreateIcon()
    {
        int iconSize = 0;

        switch (sizeIndex)
        {
            case 0:
                iconSize = 64;
                break;
            case 1:
                iconSize = 128;
                break;
            case 2:
                iconSize = 256;
                break;
            case 3:
                iconSize = 512;
                break;
            case 4:
                iconSize = 1024;
                break;
        }

        GameObject imageBG = new("BG");
        RectTransform rectTransform = imageBG.AddComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(iconSize, iconSize);
        Image sourceImage = imageBG.AddComponent<Image>();
        //        imageBG.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        if (_backGround) sourceImage.sprite = _backGround;
        imageBG.transform.SetParent(_itemCanvas.transform, false);
        GameObject imageModel = new("Icon");
        RectTransform rectTransform2 = imageModel.AddComponent<RectTransform>();
        rectTransform2.sizeDelta = new Vector2(iconSize, iconSize);
        Image sourceImage2 = imageModel.AddComponent<Image>();
        sourceImage2.sprite = GetIcon();
        imageModel.transform.SetParent(imageBG.transform, false);
        if (_backGround) SaveIconAsset(sourceImage.sprite.ConvertSpriteToTexture(), sourceImage2.sprite.ConvertSpriteToTexture(), iconSize);
        else if (!_backGround) SaveIconAsset(empty, sourceImage2.sprite.ConvertSpriteToTexture(), iconSize);
    }

   public virtual void SaveIconAsset(Texture2D bottom, Texture2D top, int iconSize)
    {

        Texture2D _bottom = EDSUtility.ScaleTexture(bottom, iconSize, iconSize);
        Texture2D _top = EDSUtility.ScaleTexture(top, iconSize, iconSize);
        Texture2D combined = _bottom.AlphaBlend(_top);
        SaveTexture(combined);
        CleanScene();
    }

    private void CleanScene()
    {
        
    }

    private void SaveTexture(Texture2D texture)
    {
        byte[] bytes = texture.EncodeToPNG();

//        if (iconName is null || iconName.Trim().Length == 0) iconName = "R_" + UnityEngine.Random.Range(0, 100000);

         string path = EditorUtility.SaveFilePanel("Save Icon image file", "Assets/", iconName, "png");
        
        if (string.IsNullOrEmpty(path)) return;

        path = FileUtil.GetPhysicalPath(path);

        //System.IO.File.WriteAllBytes(path + "/" + iconName + ".png", bytes);
        System.IO.File.WriteAllBytes(path, bytes);

        Debug.Log(bytes.Length / 1024 + "Kb was saved as: " + path);
#if UNITY_EDITOR
        UnityEditor.AssetDatabase.Refresh();
#endif
    }
    private GameObject InstantiateNewObject(GameObject selected)
    {
        if (selected == null)
        {
            return selected;
        }

        if (selected.scene.IsValid())
        {
            return selected;
        }

        return PrefabUtility.InstantiatePrefab(selected) as GameObject;
    }
    public virtual void DrawHumanoidPreview()
    {
       // GUILayout.FlexibleSpace();

        if (humanoidpreview != null)
        {
            humanoidpreview.OnInteractivePreviewGUI(GUILayoutUtility.GetRect(100, 200), "window");
        }
    }

}

