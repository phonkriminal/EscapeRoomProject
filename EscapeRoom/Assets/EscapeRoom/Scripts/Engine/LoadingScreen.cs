using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AC;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    public Slider progressSlider;
    public float delayTime = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        progressSlider.value = 0;
    }

    // Update is called once per frame
    void Update()
    {
        progressSlider.value = KickStarter.sceneChanger.GetLoadingProgress();
        if (progressSlider.value == 1)
        {
            Invoke(nameof(SwitchScene), delayTime);
        }
    }

    void SwitchScene()
    {
        KickStarter.sceneChanger.ActivateLoadedScene();
    }

}
