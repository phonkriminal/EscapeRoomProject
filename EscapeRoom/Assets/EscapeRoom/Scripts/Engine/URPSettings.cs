using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using HorizonBasedAmbientOcclusion.Universal;

public class URPSettings : MonoBehaviour
{
    [SerializeField]
    private Volume globalVolume;
    public VolumeProfile globalVolumeProfile;

    [SerializeField]
    private HorizonBasedAmbientOcclusion.Universal.HBAO hBAO;
    private List<VolumeComponent> components = new();
    public UnityEngine.UI.Slider aoRadiusSlider;
    public UnityEngine.UI.Slider aoIntensitySlider;
    public UnityEngine.UI.Toggle aoToggleSwitch;
    private bool presetIsOn;
    // Start is called before the first frame update
    void Start()
    {
        components = globalVolumeProfile.components;
        foreach (var item in components)
        {
            if (item.GetType().Equals(typeof(HorizonBasedAmbientOcclusion.Universal.HBAO)))
            {
                hBAO = (HorizonBasedAmbientOcclusion.Universal.HBAO)item;
                Debug.Log(hBAO.ToString() + " TRUE");
                //if (hBAO.GetDebugMode() != HBAO.DebugMode.Disabled) { hBAO.SetDebugMode(HBAO.DebugMode.Disabled); }
                hBAO.SetDebugMode(HBAO.DebugMode.Disabled);
                hBAO.SetAoRadius(aoRadiusSlider.value);
                hBAO.SetAoIntensity(aoIntensitySlider.value);
                presetIsOn = hBAO.active;
                aoToggleSwitch.isOn = presetIsOn;
            }
        }
    }
    private void OnDestroy()
    {
        hBAO.active = presetIsOn;
    }
    public void ToggleHBAO(bool isOn)
    {
        hBAO.active = isOn;
        
        Debug.Log(hBAO.active);
    }

    public void ToggleShowAO() 
    {
        if (hBAO.GetDebugMode() != HBAO.DebugMode.Disabled)
            hBAO.SetDebugMode(HBAO.DebugMode.Disabled);
        else
            hBAO.SetDebugMode(HBAO.DebugMode.AOOnly);
    }

    public void UpdateAoRadius()
    {
        hBAO.SetAoRadius(aoRadiusSlider.value);
    }
    public void UpdateAoIntensity()
    {
        hBAO.SetAoIntensity(aoIntensitySlider.value);
    }
}
