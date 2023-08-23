using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AC;

public class SpaceDetection : MonoBehaviour
{
  
    [SerializeField]
    private string menuHotspot = string.Empty;
    private Hotspot activeHotspot = null;

    private void OnEnable()
    {
        EventManager.OnHotspotSelect += OnHotspotSelect;
        EventManager.OnHotspotDeselect += OnHotspotDeselect;
        EventManager.OnMenuTurnOn += OnMenuTurnOn;
        EventManager.OnMenuTurnOff += OnMenuTurnOff;
    }
    private void OnDisable()
    {
        EventManager.OnHotspotSelect -= OnHotspotSelect;
        EventManager.OnHotspotDeselect -= OnHotspotDeselect;
        EventManager.OnMenuTurnOn -= OnMenuTurnOn;
        EventManager.OnMenuTurnOff += OnMenuTurnOff;
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(1) && KickStarter.stateHandler.IsInGameplay() && KickStarter.runtimeInventory.SelectedItem == null)
        {
            // Clicked during gameplay, with no item selected
            //if (KickStarter.playerInteraction.GetActiveHotspot() == null && !KickStarter.playerMenus.IsMouseOverMenu())
            if (!KickStarter.playerMenus.IsMouseOverMenu())
            {
                // No active Hotspot or menu
                KickStarter.playerMenus.CloseInteractionMenus();
            }
        }
    }

    private void OnMenuTurnOn(AC.Menu _menu, bool isInstant)
    {
        if (activeHotspot && (_menu.title == "Interaction" | _menu.title == "Hotspot"))
        {
            activeHotspot.SetIconVisibility(true);
        }
    }


    private void OnMenuTurnOff(AC.Menu _menu, bool isInstant)
    {
        if (activeHotspot && _menu.title == "Interaction")
        {
            activeHotspot.SetIconVisibility(true);
            activeHotspot.UpdateIcon();
            Debug.Log("Interaction");
            return;
        }

        Debug.Log("Turn off");
    }


    private void OnHotspotSelect(Hotspot _hotspot)
    {
        activeHotspot = _hotspot;
        activeHotspot.SetIconVisibility(true);
    }

    private void OnHotspotDeselect(Hotspot _hotspot)
    {
        if (activeHotspot != null && activeHotspot == _hotspot)
        {
            _hotspot.SetIconVisibility(false);
            activeHotspot = null;
            Debug.Log("Deselect");
        }
    }


}
