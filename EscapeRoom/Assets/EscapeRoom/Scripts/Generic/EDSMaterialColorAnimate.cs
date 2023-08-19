using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif


[ExecuteInEditMode]

public class EDSMaterialColorAnimate : MonoBehaviour
{
    
    
    // Start is called before the first frame update
    [Tooltip("Select the material to apply FX")]
    public Material material;    
    [Tooltip("How much to smooth out the randomness; lower values = sparks, higher = smooth")]
    [Range(1, 50)]
    public int smoothing = 5;
    [Tooltip("Update the light in the enviroment.")]
    public bool updateGI = false;
    [Tooltip("Activate in Editor.")]
    public bool activateInEditor = false;
    [Tooltip("Choose if you want animate Emissions instead of Base Color.")]
    public bool animateEmissionColor = false;
    [SerializeField]
    private Vector2 emissionIntensity = new(0f, 0f);
    private Color startColor;
    [ColorUsage(true, true)]
    private Color startEmissionColor;
    [ColorUsage(true, true)]
    public Color StartColor;
    public Color32 startHDRcolor;
    [ColorUsage(true, true)]
    public Color EndColor;
    public Color32 endHDRColor;
    public float time;
    bool goingForward;
    bool isCycling;
    Material myMaterial;

    private const byte k_MaxByteForOverexposedColor = 191;


    [ColorUsage(true, true)]
    private Color currentColor;

    private void Awake()
    {
        goingForward = true;
        isCycling = false;
        myMaterial = material;
        startEmissionColor = material.GetColor("_EmissionColor");
        startColor = material.color;
        Decompose();
    }
    private void Decompose()
    {
        DecomposeHdrColor(StartColor, out startHDRcolor, out emissionIntensity.x);
        DecomposeHdrColor(EndColor, out endHDRColor, out emissionIntensity.y);
    }

    public static void DecomposeHdrColor(Color linearColorHdr, out Color32 baseLinearColor, out float exposure)
    {
        baseLinearColor = linearColorHdr;
        var maxColorComponent = linearColorHdr.maxColorComponent;
        // replicate Photoshops's decomposition behaviour
        if (maxColorComponent == 0f || maxColorComponent <= 1f && maxColorComponent >= 1 / 255f)
        {
            exposure = 0f;

            baseLinearColor.r = (byte)Mathf.RoundToInt(linearColorHdr.r * 255f);
            baseLinearColor.g = (byte)Mathf.RoundToInt(linearColorHdr.g * 255f);
            baseLinearColor.b = (byte)Mathf.RoundToInt(linearColorHdr.b * 255f);
        }
        else
        {
            // calibrate exposure to the max float color component
            var scaleFactor = k_MaxByteForOverexposedColor / maxColorComponent;
            exposure = Mathf.Log(255f / scaleFactor) / Mathf.Log(2f);

            // maintain maximal integrity of byte values to prevent off-by-one errors when scaling up a color one component at a time
            baseLinearColor.r = Math.Min(k_MaxByteForOverexposedColor, (byte)Mathf.CeilToInt(scaleFactor * linearColorHdr.r));
            baseLinearColor.g = Math.Min(k_MaxByteForOverexposedColor, (byte)Mathf.CeilToInt(scaleFactor * linearColorHdr.g));
            baseLinearColor.b = Math.Min(k_MaxByteForOverexposedColor, (byte)Mathf.CeilToInt(scaleFactor * linearColorHdr.b));
        }
    }


    private void Update()
    {
        Decompose();
        if (myMaterial == null) return;

#if UNITY_EDITOR
        if (activateInEditor) ChangeColor();
#else
        ChangeColor();
#endif
        if (updateGI) DynamicGI.UpdateEnvironment();

    }

    private void ChangeColor()
    {
        if (animateEmissionColor)
        {
            if (!isCycling)
            {
                if (goingForward)
                    StartCoroutine(CycleEmissions(startHDRcolor, endHDRColor, time, myMaterial));
                else
                    StartCoroutine(CycleEmissions(endHDRColor, startHDRcolor, time, myMaterial));
            }
        }
        else
        {
            if (!isCycling)
            {
                if (goingForward)
                    StartCoroutine(CycleMaterial(StartColor, EndColor, time, myMaterial));
                else
                    StartCoroutine(CycleMaterial(EndColor, StartColor, time, myMaterial));
            }
        }

    }

    private void OnDisable()
    {
        myMaterial.color = startColor;
        myMaterial.SetColor("_EmissionColor", startEmissionColor);
        material = myMaterial;
    }

    IEnumerator CycleEmissions(Color startColor, Color endColor, float cycleTime, Material mat)
    {
        isCycling = true;
        float currentTime = 0;        

        while (currentTime < cycleTime)
        {
            currentTime += Time.deltaTime;
            float t = currentTime / cycleTime;
            Color currentColor32;
            currentColor32 = Color.Lerp(startColor, endColor, t);
            float intensity = Mathf.Lerp(emissionIntensity.x, emissionIntensity.y, t);
            mat.SetColor("_EmissionColor", currentColor32 * intensity);
            yield return null;
        }
        isCycling = false;
        goingForward = !goingForward;

    }

    IEnumerator CycleMaterial(Color startColor, Color endColor, float cycleTime, Material mat)
    {
        isCycling = true;
        float currentTime = 0;
        while (currentTime < cycleTime)
        {
            currentTime += Time.deltaTime;
            float t = currentTime / cycleTime;
            Color currentColor = Color.Lerp(startColor, endColor, t);
            mat.color = currentColor;
            yield return null;
        }
        isCycling = false;
        goingForward = !goingForward;

    }

}
