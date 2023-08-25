using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AC;

public class InteractionListUI : MonoBehaviour
{

    #region Variables

    private List<AC.Button> interactions;

    private int interactionOffset;
    private Menu menu;

    [SerializeField] private Canvas canvas;

    [SerializeField] private UnityEngine.UI.Button[] interactionButtons;

    [SerializeField] private UnityEngine.UI.Button interactionUpButton;
    [SerializeField] private UnityEngine.UI.Button interactionDownButton;

    #endregion

    #region UnityStandards

    private void OnEnable()
    {
        if (Menu == null)
        {
            return;
        }

        if (interactions == null)
        {
            interactions = new List<AC.Button>();
        }

        interactions.Clear();
        foreach (AC.Button useButton in KickStarter.playerInteraction.GetActiveHotspot().useButtons)
        {
            
            if (!useButton.isDisabled)
            {
                interactions.Add(useButton);
            }
        }

        interactionOffset = 0;

        RebuildInteractionLabels();
    }

    #endregion

    #region PublicFunctions

    public void OnClickInteraction(int index)
    {
        int iconID = interactions[index + interactionOffset].iconID;
        KickStarter.playerInteraction.GetActiveHotspot().RunUseInteraction(iconID);
    }

    public void OnClickInteractionUp()
    {
        interactionOffset--;
        RebuildInteractionLabels();
    }

    public void OnClickInteractionDown()
    {
        interactionOffset++;
        RebuildInteractionLabels();
    }

    #endregion

    #region PrivateFunctions

    private void RebuildInteractionLabels()
    {
        if (interactions.Count < 3)
        {
            interactionOffset = 0;
        }
        else if (interactionOffset < 0)
        {
            interactionOffset = 0;
        }
        else if (interactionOffset > interactions.Count - 3)
        {
            interactionOffset = interactions.Count - 3;
        }

        for (int i = 0; i < interactionButtons.Length; i++)
        {
            int index = i + interactionOffset;
            if (index < interactions.Count)
            {
                interactionButtons[i].GetComponentInChildren<Text>().text = KickStarter.cursorManager.GetLabelFromID(interactions[index].iconID, Options.GetLanguage());
                interactionButtons[i].gameObject.SetActive(true);
            }
            else
            {
                interactionButtons[i].gameObject.SetActive(false);
            }
        }

        interactionUpButton.gameObject.SetActive(interactionOffset > 0);
        interactionDownButton.gameObject.SetActive(interactionOffset < interactions.Count - 3);

        if (interactions.Count > 0)
        {
            KickStarter.playerMenus.SelectUIElement(interactionButtons[0].gameObject);
        }
    }

    #endregion

    #region GetSet

    private Menu Menu
    {
        get
        {
            if (menu == null)
            {
                menu = KickStarter.playerMenus.GetMenuWithCanvas(canvas);
            }
            return menu;
        }
    }

    #endregion

}