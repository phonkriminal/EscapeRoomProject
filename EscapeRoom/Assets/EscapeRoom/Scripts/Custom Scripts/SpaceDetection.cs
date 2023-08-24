using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AC;

public class SpaceDetection : MonoBehaviour
{
  
    [SerializeField]
    private string menuHotspot = string.Empty;
    [SerializeField]
    private string menuInventory = string.Empty;
    [SerializeField]
    private string menuCrafting = string.Empty;
    [SerializeField]
    private string menuInteraction = string.Empty;

    private Hotspot activeHotspot = null;
    private AC.Menu activeMenu = null;

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
                Debug.Log("PRESS BUTTON");

                // No active Hotspot or menu
                KickStarter.playerMenus.CloseInteractionMenus();

                if (activeHotspot != null)
                {
                    KickStarter.playerInteraction.SetActiveHotspot(activeHotspot);
                    activeHotspot.DrawHotspotIcon(false);
                    activeHotspot.SetIconVisibility(true);
                    Debug.Log("INSIDE");
                }
            }
            Debug.Log("Mouse button");
        }
        else if (Input.GetMouseButtonDown(1) && KickStarter.stateHandler.IsInGameplay() && KickStarter.runtimeInventory.SelectedItem != null)
        {
            Debug.Log("Inv selected out!");
        }
        if (Input.GetMouseButtonDown(0) && KickStarter.stateHandler.IsInGameplay() && KickStarter.runtimeInventory.SelectedItem == null)
        {
            // Clicked during gameplay, with no item selected
            //if (KickStarter.playerInteraction.GetActiveHotspot() == null && !KickStarter.playerMenus.IsMouseOverMenu())
            if (KickStarter.playerInteraction.GetActiveHotspot() != null && !KickStarter.playerMenus.IsMouseOverMenu())
            {
                // No active Hotspot or menu
                KickStarter.playerMenus.EnableInteractionMenus(KickStarter.playerInteraction.GetActiveHotspot());
            }
        }

        
        
    }

    private void OnMenuTurnOn(AC.Menu _menu, bool isInstant)
    {
        if (_menu.title == menuInventory || _menu.title == menuCrafting)
        {
            Debug.Log("<color=green>MOUSE OVER</color>");
            AC.KickStarter.settingsManager.hotspotDetection = HotspotDetection.MouseOver;
            AC.KickStarter.settingsManager.interactionMethod = AC_InteractionMethod.ContextSensitive;
        }
        else
        {
            Debug.Log("MENU HOT INT");
        }
    }


    private void OnMenuTurnOff(AC.Menu _menu, bool isInstant)
    {
        if (_menu.title == menuInventory || _menu.title == menuCrafting)
        {
            Debug.Log("<color=white>PLAYER VICINITY</color>");
            AC.KickStarter.settingsManager.hotspotDetection = HotspotDetection.PlayerVicinity;
            AC.KickStarter.settingsManager.interactionMethod = AC_InteractionMethod.ChooseHotspotThenInteraction;
        }
        else
        {
            Debug.Log("<color=red>MENU HOT INT OFF</color>");
        }
    }


    private void OnHotspotSelect(Hotspot _hotspot)
    {
        activeHotspot = _hotspot;
        activeHotspot.SetIconVisibility(true);
    }

    private void OnHotspotDeselect(Hotspot _hotspot)
    {
        _hotspot.SetIconVisibility(false);
        activeHotspot = _hotspot;
    }


}
