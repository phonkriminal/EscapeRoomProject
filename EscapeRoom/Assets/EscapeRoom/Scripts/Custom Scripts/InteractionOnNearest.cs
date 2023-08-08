using UnityEngine;
using AC;

public class InteractionOnNearest : MonoBehaviour
{

    public float minimumSqrDistance = 10f;

    private void Update()
    {
        // Check if there is a player with a hotspot detector
        if (KickStarter.player && KickStarter.player.hotspotDetector)
        {
            // Read the nearest Hotspot to the detector
            Hotspot nearestHotspot = KickStarter.player.hotspotDetector.NearestHotspot;

            if (nearestHotspot && KickStarter.stateHandler.IsInGameplay() && (nearestHotspot.transform.position - KickStarter.player.hotspotDetector.transform.position).sqrMagnitude < minimumSqrDistance)
            {
                // If there is a Hotspot nearby, and it's during gameplay, show Interaction menus for that Hotspot
                KickStarter.playerMenus.EnableInteractionMenus(nearestHotspot);
            }
            else
            {
                // Otherwise, close Interaction menus
                KickStarter.playerMenus.CloseInteractionMenus();
            }
        }

    }

}