using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AC;

public class InteractionManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()

    {

        // This simply adds Cancel Functionality

        if (KickStarter.playerMenus.IsInteractionMenuOn() && Input.GetMouseButtonDown(1))

        {

            // De-select the current Hotspot

            Hotspot hotspot = KickStarter.playerInteraction.GetActiveHotspot();

            // Close the Interaction menu(s)

            KickStarter.playerMenus.CloseInteractionMenus();
        }

    }
}
