using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class EDSIconMaker : MonoBehaviour
{
    public Image _image;
    public Camera _camera;
    public GameObject _container;

    public Sprite GetIcon()
    {

        //_camera.orthographicSize = _container.GetComponent<Renderer>().bounds.extents.y + 0.1f;

        // Get our dimensions
        int resX = _camera.pixelWidth;
        int resY = _camera.pixelHeight;

        // Variables for clipping image down to square

        int clipX = 0;
        int clipY = 0;

        if (resX > resY) 
            clipX = resX - resY;
        else if (resY > resX)
            clipY = resY - resX;


        // Initialise all parts.
        //Texture2D tex = new Texture2D(resX - clipX, resY - clipY, TextureFormat.RGBA32, false);
        Texture2D tex = new Texture2D(resX , resY , TextureFormat.RGBA32, false);
        RenderTexture renderTexture = new(resX, resY, 24);
        _camera.targetTexture = renderTexture;
        RenderTexture.active = renderTexture;

        // Grab the icon and stick it in the texture.
        _camera.Render();
        tex.ReadPixels(new Rect(clipX / 2, clipY / 2, resX, resY), 0, 0);
        tex.Apply();

        //Clean up.

        _camera.targetTexture = null;
        RenderTexture.active = null;
        Destroy(renderTexture);

        // Convert Texture2D into Sprite and return it.

        return Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0, 0));

    }

    private void Start()
    {
        _image.sprite = GetIcon();
    }

}
