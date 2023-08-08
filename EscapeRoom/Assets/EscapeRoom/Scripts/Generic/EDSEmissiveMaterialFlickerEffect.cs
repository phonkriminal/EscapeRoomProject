using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EDSEmissiveMaterialFlickerEffect : MonoBehaviour
{
    // Start is called before the first frame update
    [Tooltip("Select the material to apply FX")]
    public Material material;
    [Tooltip("Minimum random material emission intensity")]
    [Range(0f, 20f)]
    public float minIntensity = 0f;
    [Tooltip("Maximum random material emission intensity")]
    [Range(0f, 20f)]
    public float maxIntensity = 5f;
    [Tooltip("How much to smooth out the randomness; lower values = sparks, higher = smooth")]
    [Range(1, 50)]
    public int smoothing = 5;
    [Tooltip("Update the light in the enviroment.")]
    public bool updateGI = false;

    // Continuous average calculation via FIFO queue
    // Saves us iterating every time we update, we just change by the delta
    Queue<float> smoothQueue;
    float lastSum = 0;

    private Color startColor;

    /// <summary>
    /// Reset the randomness and start again. You usually don't need to call
    /// this, deactivating/reactivating is usually fine but if you want a strict
    /// restart you can do.
    /// </summary>
    public void Reset()
    {
        if (smoothQueue != null) smoothQueue.Clear();
        lastSum = 0;
        if (material) material.SetColor("_EmissionColor", startColor);

    }

    void Start()
    {
        smoothQueue = new Queue<float>(smoothing);
        // External or internal light?
        if (material)
        {            
            startColor = material.GetColor("_EmissionColor");            
        }
    }


    private void OnDestroy()    
    {
        material.SetColor("_EmissionColor", startColor); 
    }

    void Update()
    {
        if (material == null)
            return;

        // pop off an item if too big
        while (smoothQueue.Count >= smoothing)
        {
            lastSum -= smoothQueue.Dequeue();
        }

        // Generate random new item, calculate new average
        float newVal = Random.Range(minIntensity, maxIntensity);
        smoothQueue.Enqueue(newVal);
        lastSum += newVal;

        // Calculate new smoothed average
        Color color = startColor;

        float intensity = (lastSum / (float)smoothQueue.Count);
               
        material.SetColor("_EmissionColor", color * intensity);
        
        if (updateGI) DynamicGI.UpdateEnvironment();
        
    }


}
