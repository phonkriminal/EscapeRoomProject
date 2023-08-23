using System.Collections.Generic;
using UnityEngine;
using AC;

public class CustomInventory : MonoBehaviour
{

    [SerializeField] private List<Hotspot> hotspots = new List<Hotspot>();
    private Menu inventoryMenu;


    private void OnEnable()
    {
        EventManager.OnHotspotInteract += OnHotspotInteract;
    }


    private void OnDisable()
    {
        EventManager.OnHotspotInteract -= OnHotspotInteract;
    }


    private void OnHotspotInteract(Hotspot hotspot, AC.Button button)
    {
        if (hotspots.Contains(hotspot))
        {
            if (inventoryMenu == null) inventoryMenu = PlayerMenus.GetMenuWithName("Inventory");

            HotspotInteractionType interactionType = hotspot.GetButtonInteractionType(button);
            if (interactionType == HotspotInteractionType.Use)
            {
                inventoryMenu.MatchInteractions(hotspot, true);
                inventoryMenu.isLocked = false;
            }
            else if (interactionType == HotspotInteractionType.Inventory)
            {
                inventoryMenu.isLocked = true;
            }
        }
    }

}