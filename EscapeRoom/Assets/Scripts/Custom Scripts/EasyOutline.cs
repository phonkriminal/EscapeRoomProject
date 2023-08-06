using UnityEngine;
using System.Collections;
using EPOOutline;
using AC;

public class EasyOutline : MonoBehaviour
{
    #region Variables

    private Hotspot hotspot = null;

    [SerializeField]
    private Outlinable outlinable = null;
    [SerializeField]
    [ColorUsage(true, true)]
    private Color oulineColor = new(255, 0, 0, 255);
    [SerializeField]
    [Range(0.1f, 5.0f)]
    private float speed = 5f;

    #endregion

    #region UnityStandards

    private void Awake()

    {
        hotspot = GetComponent<Hotspot>();

        //  outlinable = GetComponent<Outlinable>();

        outlinable.RenderStyle = RenderStyle.FrontBack;
        outlinable.enabled = false;
        outlinable.BackParameters.Enabled = false;
        outlinable.FrontParameters.Color = oulineColor;     //  Color.white;
        outlinable.FrontParameters.DilateShift = 0f;
    }

    private void OnEnable()
    {
        EventManager.OnHotspotSelect += OnHotspotSelect;
        EventManager.OnHotspotDeselect += OnHotspotDeselect;
    }

    private void OnDisable()
    {
        EventManager.OnHotspotSelect -= OnHotspotSelect;
        EventManager.OnHotspotDeselect -= OnHotspotDeselect;
    }

    #endregion

    #region PrivateFunctions

    private void OnHotspotSelect(Hotspot hotspot)
    {
        if (this.hotspot == hotspot)
        {
            StopAllCoroutines();
            StartCoroutine(TransitionOn());
        }
    }

    private void OnHotspotDeselect(Hotspot hotspot)
    {
        if (this.hotspot == hotspot)
        {
            StopAllCoroutines();
            StartCoroutine(TransitionOff());
        }
    }

    private IEnumerator TransitionOn()
    {
        outlinable.enabled = true;

        while (outlinable.FrontParameters.DilateShift < 1f)
        {
            float newShift = outlinable.FrontParameters.DilateShift + (speed * Time.deltaTime);
            newShift = Mathf.Clamp01(newShift);
            outlinable.FrontParameters.DilateShift = newShift;

            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator TransitionOff()
    {
        while (outlinable.FrontParameters.DilateShift > 0f)
        {
            float newShift = outlinable.FrontParameters.DilateShift - (speed * Time.deltaTime);
            newShift = Mathf.Clamp01(newShift);
            outlinable.FrontParameters.DilateShift = newShift;
            yield return new WaitForEndOfFrame();
        }

        outlinable.enabled = false;
    }

    #endregion

}