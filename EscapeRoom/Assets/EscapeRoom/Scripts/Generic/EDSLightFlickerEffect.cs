using UnityEngine;
using System.Collections.Generic;


/// <summary>
/// Component which will flicker a linked light while active by changing its
/// intensity between the min and max values given. The flickering can be
/// sharp or smoothed depending on the value of the smoothing parameter.
///
/// Just activate / deactivate this component as usual to pause / resume flicker
/// </summary>
public class EDSLightFlickerEffect : MonoBehaviour {
    [Tooltip("External light to flicker; you can leave this null if you attach script to a light")]
    public Light _light = new();
    [Tooltip("Minimum random light intensity")]
    public float minIntensity = 0f;
    [Tooltip("Maximum random light intensity")]
    public float maxIntensity = 1f;
    [Tooltip("How much to smooth out the randomness; lower values = sparks, higher = lantern")]
    [Range(1, 50)]
    public int smoothing = 5;

    // Continuous average calculation via FIFO queue
    // Saves us iterating every time we update, we just change by the delta
    Queue<float> smoothQueue;
    float lastSum = 0;


    /// <summary>
    /// Reset the randomness and start again. You usually don't need to call
    /// this, deactivating/reactivating is usually fine but if you want a strict
    /// restart you can do.
    /// </summary>
    public void Reset() {
        if (smoothQueue != null)
        {
            smoothQueue.Clear();
        }
        
        lastSum = 0;
    }

    public bool GetLightComponent()
    {
        return TryGetComponent<Light>(out _light);
    }

    void Start() 
    {        
        // External or internal light?
     
        if (_light != null) 
        {
            smoothQueue = new Queue<float>(smoothing);
        }
}

    void Update() {
        if (_light == null)
            return;

        // pop off an item if too big
        while (smoothQueue.Count >= smoothing) {
            lastSum -= smoothQueue.Dequeue();
        }

        // Generate random new item, calculate new average
        float newVal = Random.Range(minIntensity, maxIntensity);
        smoothQueue.Enqueue(newVal);
        lastSum += newVal;

        // Calculate new smoothed average
        _light.intensity = lastSum / (float)smoothQueue.Count;
    }

}