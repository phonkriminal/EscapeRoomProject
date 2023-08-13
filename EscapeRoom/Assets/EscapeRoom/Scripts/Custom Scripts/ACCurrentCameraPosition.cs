using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AC;


public class ACCurrentCameraPosition : MonoBehaviour
{

    private _Camera currentCamera;
    private Vector3 localPosition;
    private Quaternion localRotation;

    private void OnEnable()
    {
        EventManager.OnSwitchCamera += OnSwitchCamera;
    }

  
    private void OnDisable()
    {
        EventManager.OnSwitchCamera -= OnSwitchCamera;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentCamera)
        {
            currentCamera.gameObject.transform.GetLocalPositionAndRotation(out localPosition, out localRotation);
            this.transform.SetLocalPositionAndRotation(localPosition, localRotation);
        }
    }

    private void OnSwitchCamera(_Camera fromCamera, _Camera toCamera, float transitionTime)
    {
        currentCamera = toCamera;
        if (currentCamera) Debug.Log(currentCamera.gameObject.name);
    }
}
