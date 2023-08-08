using UnityEngine;
using AC;

public class MultiHotspotLabel : MonoBehaviour
{

    public Hotspot hotspotToShow;
    public Highlight highlightToSync;
    public bool alwaysShowDuringGameplay;
    public LayerMask layerMask = new LayerMask();
    public string menuName = "Hotspot"; // The name of the Hotspot Menu to copy
    public string labelName = "HotspotLabel"; // The Label element of the Hotspot Menu
    private Menu myMenu; // Our own local Menu


    private void OnEnable()
    {
        // Attempt to auto-assign components if not set yet
        if (hotspotToShow == null)
        {
            hotspotToShow = GetComponent<Hotspot>();
        }
        if (highlightToSync == null && hotspotToShow != null)
        {
            highlightToSync = hotspotToShow.highlight;
        }

        if (alwaysShowDuringGameplay)
        {
            EventManager.OnEnterGameState += OnEnterGameState;
            return;
        }

        // Add our own listeners to the Highlight component's onHighlightOn and onHighlightOff events
        if (highlightToSync != null)
        {
            highlightToSync.callEvents = true;
            highlightToSync.onHighlightOn.AddListener(ShowForHotspot);
            highlightToSync.onHighlightOff.AddListener(Hide);
        }
    }


    private void Start()
    {
        if (alwaysShowDuringGameplay)
        {
            ShowForHotspot();
        }
    }


    private void OnEnterGameState(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.Normal:
                ShowForHotspot();
                break;

            default:
                Hide();
                break;
        }
    }


    private void OnDisable()
    {
        if (alwaysShowDuringGameplay)
        {
            EventManager.OnEnterGameState -= OnEnterGameState;
            Hide();
            return;
        }

        // Remove our own listeners from the Highlight component's onHighlightOn and onHighlightOff events
        if (highlightToSync != null)
        {
            highlightToSync.onHighlightOn.RemoveListener(ShowForHotspot);
            highlightToSync.onHighlightOff.RemoveListener(Hide);
        }
    }


    private void Update()
    {
        //if (myMenu) myMenu.HotspotLabelData.SetData(hotspotToShow, string.Empty);

        if (hotspotToShow != null && alwaysShowDuringGameplay && KickStarter.stateHandler.IsInGameplay())
        {
            if (hotspotToShow.limitToCamera != null && KickStarter.mainCamera.attachedCamera != hotspotToShow.limitToCamera)
            {
                // Turn off
                Hide();
                return;
            }

            Vector3 direction = hotspotToShow.GetIconPosition() - Camera.main.transform.position;
            Ray ray = new Ray(Camera.main.transform.position, direction);
            RaycastHit hitInfo = new RaycastHit();
            if (Physics.Raycast(ray, out hitInfo, direction.magnitude, layerMask.value))
            {
                if (hitInfo.collider.gameObject != hotspotToShow.gameObject)
                {
                    // Turn off
                    Hide();
                    return;
                }
            }

            ShowForHotspot();
        }
    }


    private void ShowForHotspot()
    {
        // Call this function to show a new Menu linked to the given Hotspot
        if (myMenu == null)
        {
            // When run for the first time, create a new Menu and use the default Hotspot menu to copy from
            Menu menuToCopy = PlayerMenus.GetMenuWithName(menuName);
            if (menuToCopy == null)
            {
                ACDebug.LogWarning("Cannot find menu with name '" + menuName + "'", this);
                return;
            }
            myMenu = ScriptableObject.CreateInstance<Menu>();

            myMenu.CreateDuplicate(menuToCopy); // Copy from the default Menu
            myMenu.appearType = AppearType.Manual; // Set it to Manual so that we can control it easily
            myMenu.isLocked = false; // Unlock it so that the default can remain locked if necessary
            myMenu.title += this.name;
            myMenu.SetHotspot(hotspotToShow, null); // Link it to the Hotspot

            if (!string.IsNullOrEmpty(labelName))
            {
                (myMenu.GetElementWithName(labelName) as MenuLabel).labelType = AC_LabelType.Normal;
                (myMenu.GetElementWithName(labelName) as MenuLabel).label = hotspotToShow.GetName(Options.GetLanguage());
            }
        }

        // Turn the menu on
        myMenu.TurnOn();

        // Register with PlayerMenus to handle updating
        KickStarter.playerMenus.RegisterCustomMenu(myMenu, true);
    }


    private void Hide()
    {
        // Call this function to hide the Menu
        if (myMenu != null)
        {
            if (KickStarter.eventManager)
            {
                myMenu.TurnOff();
            }
            myMenu = null;
        }
    }

}